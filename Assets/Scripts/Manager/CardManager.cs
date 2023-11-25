using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CSCI526GameJam {

    public class CardManager : MonoBehaviourSingleton<CardManager>, IAssetDependent {

        public enum Pattern {
            None,
            X,
            XX,
            XXXY,
            XXXYY,
            XXXX,
            ABCD,
        }

        #region Fields
        [MandatoryFields]
        [SerializeField] private int numPerCard = 4;
        [SerializeField] private int numDrawPerLevel = 6;

        [ComputedFields]
        [SerializeField] private Pattern currentPattern;
        [SerializeField] private List<Card> deck = new();
        [SerializeField] private List<Card> hand = new();
        [SerializeField] private List<Card> selectedCards = new();
        [SerializeField] private List<CardConfig> cardConfigs;

        private Dictionary<CardConfig, int> cardConfigToUsageNum = new();
        #endregion

        #region Publics
        public event Action<Card[]> OnCardsInserted;
        public event Action<Card> OnCardSelected;
        public event Action<Card> OnCardUnselected;
        public event Action<Card[]> OnCardsPlayed;
        public event Action OnForceClearOnHand;

        public int NumInDeck => deck.Count;
        public Pattern CurrentPattern => currentPattern;
        public int CurrentCost => selectedCards.Sum(x => x.Cost);
        public int NumSelected => selectedCards.Count;
        public int NumOnHand => hand.Count;
        public Dictionary<CardConfig, int> CardConfigToUsageNum => cardConfigToUsageNum;

        /// <summary>
        /// Get a card on hand. 
        /// </summary>
        /// <param name="index">Index of the card. </param>
        /// <returns>The card. </returns>
        public Card Get(int index) {
            return hand[index];
        }

        /// <summary>
        /// Get the index of a card. 
        /// </summary>
        /// <param name="card">Card to get index. </param>
        /// <returns>The index. </returns>
        public int IndexOf(Card card) {
            return hand.IndexOf(card);
        }

        /// <summary>
        /// Draw cards from the deck. 
        /// </summary>
        /// <param name="numCards">Num of cards to draw. </param>
        public void DrawFromDeck(int numCards) {
            if (numCards <= 0) {
                Debug.LogWarning($"{numCards} is not a positive num. ");
                return;
            }

            var cards = new List<Card>();
            for (int i = 0; i < numCards; i++) {
                if (deck.Count == 0) break;

                var index = Random.Range(0, deck.Count);
                cards.Add(deck[index]);
                deck.RemoveAt(index);
            }

            InsertCards(cards.ToArray());
        }

        /// <summary>
        /// Select a card on hand. 
        /// </summary>
        /// <param name="index">Index of the card to select. </param>
        public void Select(int index) {
            var card = hand[index];
            selectedCards.Add(card);
            UpdatePattern();
            OnCardSelected?.Invoke(card);
        }

        /// <summary>
        /// Unselect a card on hand. 
        /// </summary>
        /// <param name="index">Index of the card to unselect. </param>
        public void Unselect(int index) {
            var card = hand[index];
            if (!selectedCards.Remove(card)) {
                Debug.LogWarning($"Trying to remove a non-existing card on hand. ");
                return;
            }

            UpdatePattern();
            OnCardUnselected?.Invoke(card);
        }

        public void UnselectAll() {
            var indices = selectedCards.Select(x => hand.IndexOf(x)).ToArray();
            indices.ForEach(x => Unselect(x));
        }

        /// <summary>
        /// Toggle if a card is selected on hand. 
        /// </summary>
        /// <param name="index">Index of the card to toggle. </param>
        public void ToggleSelected(int index) {
            var card = hand[index];
            if (selectedCards.Contains(card)) {
                Unselect(index);
            }
            else {
                Select(index);
            }
        }

        /// <summary>
        /// Play the selected cards. 
        /// </summary>
        public bool PlaySelected() {
            if (selectedCards.Count == 0) return false;
            if (currentPattern == Pattern.None) return false;

            var cost = selectedCards.Sum(x => x.Cost);
            if (!Player.Instance.TryPay(cost)) return false;

            selectedCards.ForEach(x => x.Play());

            // For analytics. 
            foreach (var card in selectedCards) {
                if (cardConfigToUsageNum.TryGetValue(card.Config, out var num)) {
                    num++;
                    cardConfigToUsageNum[card.Config] = num;
                    continue;
                }
                cardConfigToUsageNum[card.Config] = 1;
            }

            var playedCards = selectedCards.ToArray();
            selectedCards.ForEach(x => hand.Remove(x));
            selectedCards.Clear();

            OnCardsPlayed?.Invoke(playedCards);
            return true;
        }

        public void LoadTutorial(TutorialInfo info) {
            deck.Clear();
            foreach (var config in info.Deck) {
                deck.Add(new(config));
            }

            OnForceClearOnHand?.Invoke();
            hand.Clear();
            InsertCards(info.OnHandCards.Select(x => new Card(x)).ToArray());
        }

        public void EndTutorial() {

            deck.Clear();
            foreach (var config in cardConfigs) {
                for (int i = 0; i < numPerCard; i++) {
                    deck.Add(new(config));
                }
            }

            OnForceClearOnHand?.Invoke();
            hand.Clear();
        }

#if UNITY_EDITOR
        [EditorOnlyFields]
        [FolderPath, SerializeField]
        private string cardsPath;
        public void FindAssets() {
            cardConfigs = Utility.FindRefsInFolder<CardConfig>(cardsPath, AssetType.ScriptableObject);
            Debug.Log($"Found {cardConfigs.Count} cards under {cardsPath}. ");
        }
#endif
        #endregion

        #region Internals
        [ContextMenu("Draw")]
        private void DrawOnNewRound() {
            DrawFromDeck(numDrawPerLevel); // TODO: alg instead
        }

        private void InsertCards(Card[] cards) {
            cards.ForEach(x => InsertToCorrectIndex(x));

            OnCardsInserted?.Invoke(cards);
        }

        private void InsertToCorrectIndex(Card card) {
            var index = hand.FindLastIndex(c => c.Config == card.Config);
            if (index == -1) {
                index = hand.FindIndex(c => c.Config.Cost > card.Config.Cost);
                if (index == -1) {
                    index = hand.Count;
                    hand.Add(card);
                }
                else {
                    hand.Insert(index, card);
                }
            }
            else {
                index++;
                hand.Insert(index, card);
            }
        }

        private void UpdatePattern() {
            hand.ForEach(x => x.Reset());

            var selected = selectedCards
                .OrderBy(c => c.Cost)
                .ToList();

            var n = selected.Count;

            var pattern = Pattern.None;
            // Check for ABCD
            if (n >= 4) {
                var i = 1;
                for (i = 1; i < n; i++) {
                    if (selected[i].Cost != selected[i - 1].Cost + 1) break;
                }
                if (i == n) pattern = Pattern.ABCD;
            }

            // Check others
            switch (n) {
                case 1:
                    pattern = Pattern.X;
                    break;

                case 2:
                    pattern = selected[0].Cost == selected[1].Cost ? Pattern.XX : Pattern.None;
                    break;

                case 4:
                    if (selected.Count(c => c.Cost == selected[0].Cost) == 4) pattern = Pattern.XXXX;
                    if (selected.Count(c => c.Cost == selected[0].Cost) == 3
                        || selected.Count(c => c.Cost == selected[n - 1].Cost) == 3) pattern = Pattern.XXXY;

                    break;

                case 5:
                    if (selected.Count(c => c.Cost == selected[0].Cost) == 3
                        && selected.Count(c => c.Cost == selected[n - 1].Cost) == 2
                        || selected.Count(c => c.Cost == selected[n - 1].Cost) == 3
                        && selected.Count(c => c.Cost == selected[0].Cost) == 2) pattern = Pattern.XXXYY;

                    break;

                default:
                    break;
            }

            currentPattern = pattern;
            if (currentPattern == Pattern.None) return;

            switch (currentPattern) {
                case Pattern.X:
                    selected[0].SetLevel(Card.Level.One);
                    break;

                case Pattern.XX:
                    selected[0].SetLevel(Card.Level.Two);
                    selected[1].SetLevel(Card.Level.Two);
                    break;

                case Pattern.XXXY:
                    // XXXY
                    if (selected[0].Cost == selected[1].Cost) {
                        selected[3].HalveCost();
                    }
                    // YXXX
                    else {
                        selected[0].HalveCost();
                    }

                    selected[0].SetLevel(Card.Level.One);
                    selected[1].SetLevel(Card.Level.One);
                    selected[2].SetLevel(Card.Level.One);
                    selected[3].SetLevel(Card.Level.One);
                    break;

                case Pattern.XXXYY:
                    // XXXYY
                    if (selected[1].Cost == selected[2].Cost) {
                        selected[3].HalveCost();
                        selected[4].HalveCost();
                    }
                    // YYXXX
                    else {
                        selected[0].HalveCost();
                        selected[1].HalveCost();
                    }

                    selected[0].SetLevel(Card.Level.One);
                    selected[1].SetLevel(Card.Level.One);
                    selected[2].SetLevel(Card.Level.One);
                    selected[3].SetLevel(Card.Level.Two);
                    selected[4].SetLevel(Card.Level.Two);
                    break;

                case Pattern.XXXX:
                    selected[0].SetLevel(Card.Level.Three);
                    selected[1].SetLevel(Card.Level.Three);
                    selected[2].SetLevel(Card.Level.Three);
                    selected[3].SetLevel(Card.Level.Three);
                    break;

                case Pattern.ABCD:
                    selected.ForEach(c => c.SetLevel(Card.Level.Two));
                    break;

                default:
                    Debug.LogWarning($"Undefined pattern {currentPattern}");
                    return;
            }
        }
        #endregion

        #region Unity Methods
        protected override void Awake() {
            base.Awake();
            foreach (var config in cardConfigs) {
                for (int i = 0; i < numPerCard; i++) {
                    deck.Add(new(config));
                }
            }

            GameManager.Instance.OnPreparationStarted += DrawOnNewRound;

            GameManager.Instance.OnCurrentSceneExiting += () => {
                GameManager.Instance.OnPreparationStarted -= DrawOnNewRound;
            };
        }
        #endregion
    }
}
