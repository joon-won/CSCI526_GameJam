using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CSCI526GameJam {

    public class CardManager : MonoBehaviourSingleton<CardManager>, IAssetDependent {

		private enum Pattern {
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
		[SerializeField] private int maxOnHand = 20;
		[SerializeField] private int numDrawPerLevel = 6;

	    [ComputedFields]
	    [SerializeField] private List<Card> cardConfigs;
	    [SerializeField] private List<Card> deck = new();
		[SerializeField] private List<Card> hand = new();
		[SerializeField] private List<int> selectedIndices = new();
		#endregion

		#region Publics
		public event Action<int> OnCardInserted;
		public event Action<int> OnCardSelected;
		public event Action<int> OnCardUnselected;
		public event Action<List<int>> OnCardsPlayed;

		/// <summary>
		/// Get a card on hand. 
		/// </summary>
		/// <param name="index">Index of the card. </param>
		/// <returns>The card. </returns>
		public Card Get(int index) {
			return hand[index];
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

			if (hand.Count >= maxOnHand) {
				Debug.Log("TODO: onHand is full. ");
				return;
			}

			for (int i = 0; i < numCards; i++) {
				if (deck.Count == 0) return;

				var randIndex = Random.Range(0, deck.Count - 1);
				InsertCard(deck[randIndex]);
				deck.RemoveAt(randIndex);
			}
		}

		/// <summary>
		/// Select a card on hand. 
		/// </summary>
		/// <param name="index">Index of the card to select. </param>
		public void Select(int index) {
			selectedIndices.Add(index);
			OnCardSelected?.Invoke(index);
        }

		/// <summary>
		/// Unselect a card on hand. 
		/// </summary>
		/// <param name="index">Index of the card to unselect. </param>
		public void Unselect(int index) {
			if (selectedIndices.Remove(index)) {
				OnCardUnselected?.Invoke(index);
			}
		}

		/// <summary>
		/// Toggle if a card is selected on hand. 
		/// </summary>
		/// <param name="index">Index of the card to toggle. </param>
		public void ToggleSelected(int index) {
			if (selectedIndices.Contains(index)) {
				Unselect(index);
			}
			else {
				Select(index);
			}
        }

		/// <summary>
		/// Play the selected cards. 
		/// </summary>
		public void PlaySelected() {
			if (selectedIndices.Count == 0) return;

			var selected = selectedIndices.Select(index => hand[index]).ToList();
			var pattern = CheckPattern(selected);
			if (pattern == Pattern.None) return;

			var cost = 0;
			switch (pattern) {
				case Pattern.X:
					if (!Player.Instance.TryPay(selected[0].Cost)) return;

					selected[0].PlayLv1();
					break;

				case Pattern.XX:
					if (!Player.Instance.TryPay(selected[0].Cost * 2)) return;

					selected[0].PlayLv2();
					selected[1].PlayLv2();
					break;

				case Pattern.XXXY:
					cost = selected[0].Cost * 3 + selected[3].Cost / 2;
					if (!Player.Instance.TryPay(cost)) return;

					selected[0].PlayLv1();
					selected[1].PlayLv1();
					selected[2].PlayLv1();
					selected[3].PlayLv1();
					break;

				case Pattern.XXXYY:
					cost = selected[0].Cost * 3 + selected[3].Cost;
					if (!Player.Instance.TryPay(cost)) return;

					selected[0].PlayLv1();
					selected[1].PlayLv1();
					selected[2].PlayLv1();
					selected[3].PlayLv2();
					selected[4].PlayLv2();
					break;

				case Pattern.XXXX:
					if (!Player.Instance.TryPay(selected[0].Cost * 4)) return;

					selected[0].PlayLv3();
					selected[1].PlayLv3();
					selected[2].PlayLv3();
					selected[3].PlayLv3();
					break;

				case Pattern.ABCD:
					cost = selected.Sum(c => c.Cost);
					if (!Player.Instance.TryPay(cost)) return;

					selected.ForEach(c => c.PlayLv2());
					break;

				default:
					Debug.LogWarning($"Undefined pattern {pattern}");
					return;
			}

			selected.ForEach(c => hand.Remove(c));
			OnCardsPlayed?.Invoke(selectedIndices);
			selectedIndices.Clear();
        }

#if UNITY_EDITOR
		[EditorOnlyFields]
		[FolderPath, SerializeField]
		private string cardsPath;
		public void FindAssets() {
			cardConfigs = Utility.FindRefsInFolder<Card>(cardsPath, AssetType.ScriptableObject);
			Debug.Log($"Found {cardConfigs.Count} cards under {cardsPath}. ");
		}
#endif
		#endregion

		#region Internals
		private void DrawOnNewRound() {
			DrawFromDeck(numDrawPerLevel); // TODO: alg instead
        }

		private void InsertCard(Card card) {
			var index = hand.FindLastIndex(c => c == card);
			if (index == -1) {
				index = hand.FindIndex(c => c.Cost > card.Cost);
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

			OnCardInserted?.Invoke(index);
        }

		private Pattern CheckPattern(List<Card> cards) {
			var n = cards.Count;
			cards.Sort((x, y) => x.Cost.CompareTo(y.Cost));

			// Check for ABCD
			if (n >= 4) {
				var i = 1;
				for (i = 1; i < n; i++) {
					if (cards[i].Cost != cards[i - 1].Cost + 1) break;
				}
				if (i == n) return Pattern.ABCD;
			}

			// Check others
			switch (n) {
				case 1:
					return Pattern.X;

				case 2:
					return cards[0].Cost == cards[1].Cost ? Pattern.XX : Pattern.None;

				case 4:
					if (cards.Count(c => c.Cost == cards[0].Cost) == 4) return Pattern.XXXX;
					if (cards.Count(c => c.Cost == cards[0].Cost) == 3 
						|| cards.Count(c => c.Cost == cards[n - 1].Cost) == 3) return Pattern.XXXY;

					return Pattern.None;

				case 5:
					if (cards.Count(c => c.Cost == cards[0].Cost) == 3 
						&& cards.Count(c => c.Cost == cards[n - 1].Cost) == 2
						|| cards.Count(c => c.Cost == cards[n - 1].Cost) == 3
						&& cards.Count(c => c.Cost == cards[0].Cost) == 2) return Pattern.XXXYY;

					return Pattern.None;

				default:
					return Pattern.None;
            }
        }
	    #endregion

	    #region Unity Methods
	    protected override void Awake() {
		    base.Awake();
		    foreach (var config in cardConfigs) {
			    for (int i = 0; i < numPerCard; i++) {
				    deck.Add(config);
			    }
		    }
	    }

        private void OnEnable() {
			GameManager.Instance.OnPreparationStarted += DrawOnNewRound;
        }

        private void OnDisable() {
            if (!GameManager.IsApplicationQuitting) {
				GameManager.Instance.OnPreparationStarted -= DrawOnNewRound;
			}
        }
        #endregion
    }
}
