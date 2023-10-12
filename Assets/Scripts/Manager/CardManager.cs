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
		[SerializeField] private int maxOnHand = 20;

	    [ComputedFields]
	    [SerializeField] private List<Card> cardConfigs;
	    [SerializeField] private List<Card> deck = new();
		[SerializeField] private List<Card> onHand = new();
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
			return onHand[index];
        }

		public void Draw(int num) {
			if (num <= 0) {
				Debug.LogWarning($"{num} is not a positive num. ");
				return;
			}

			if (onHand.Count >= maxOnHand) {
				Debug.Log("TODO: onHand is full. ");
				return;
			}

			for (int i = 0; i < num; i++) {
				if (deck.Count == 0) return;

				var randIndex = Random.Range(0, deck.Count - 1);
				InsertCard(deck[randIndex]);
				deck.RemoveAt(randIndex);
			}
		}

		public void Select(int index) {
			selectedIndices.Add(index);
			OnCardSelected?.Invoke(index);
        }

		public void Unselect(int index) {
			if (selectedIndices.Remove(index)) {
				OnCardUnselected?.Invoke(index);
			}
		}

		public void ToggleSelected(int index) {
			if (selectedIndices.Contains(index)) {
				Unselect(index);
			}
			else {
				Select(index);
			}
        }

		public void PlaySelected() {
			if (selectedIndices.Count == 0) return;

			var selected = selectedIndices.Select(index => onHand[index]).ToList();
			selected.OrderBy(c => c.Cost);
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

			selected.ForEach(c => onHand.Remove(c));
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
			Draw(10); // TODO: alg instead
        }

		private void InsertCard(Card card) {
			var index = onHand.FindLastIndex(c => c == card);
			if (index == -1) {
				index = onHand.FindIndex(c => c.Cost > card.Cost);
				if (index == -1) {
					index = onHand.Count;
					onHand.Add(card);
                }
				else {
					onHand.Insert(index, card);
                }
			}
			else {
				index++;
				onHand.Insert(index, card);
            }

			OnCardInserted?.Invoke(index);
        }

		private Pattern CheckPattern(List<Card> cards) {
			var n = cards.Count;

			// Check for ABCD
			if (n >= 4) {
				var i = 1;
				for (i = 1; i < n; i++) {
					if (cards[i].Cost != cards[i - 1].Cost + 1) break;
				}
				if (i == n - 1) return Pattern.ABCD;
			}

			// Check others
			switch (n) {
				case 1:
					return Pattern.X;

				case 2:
					return cards[0].Cost == cards[1].Cost ? Pattern.XX : Pattern.None;

				case 4:
					if (cards.All(c => c.Cost == cards[0].Cost)) return Pattern.XXXX;
					if (cards.Take(3).All(c => c.Cost == cards[0].Cost)) return Pattern.XXXY;
					return Pattern.None;

				case 5:
					if (cards.Take(3).All(c => c.Cost == cards[0].Cost)
						&& cards[n - 1].Cost == cards[n - 2].Cost) return Pattern.XXXYY;
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
