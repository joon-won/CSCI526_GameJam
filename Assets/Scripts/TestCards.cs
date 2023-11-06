using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace CSCI526GameJam {
    public class TestCards : MonoBehaviour {

        #region Fields
        [SerializeField] private TestCard testCardPrefab;
        [SerializeField] private List<TestCard> uiCards = new();
        #endregion

        #region Publics
        #endregion

        #region Internals
        private void Insert(Card insertedCard) {
            var card = Instantiate(testCardPrefab, transform);
            card.Init(insertedCard);

            var index = CardManager.Instance.IndexOf(insertedCard);
            card.transform.SetSiblingIndex(index);
            card.GetComponent<Button>().onClick.AddListener(
                () => CardManager.Instance.ToggleSelected(uiCards.IndexOf(card)));

            uiCards.Insert(index, card);
        }

        private void Select(Card card) {
            GetUICard(card).GetComponent<Image>().color = Color.red;
        }

        private void Unselect(Card card) {
            GetUICard(card).GetComponent<Image>().color = Color.white;
        }

        private void Play(List<Card> cards) {
            foreach (var card in cards) {
                var uiCard = GetUICard(card);
                if (!uiCard) {
                    Debug.LogWarning($"{card.Config.name} is not found in UI cards. ");
                    continue;
                }
                Destroy(uiCard.gameObject);
                uiCards.Remove(uiCard);
            }
        }

        private TestCard GetUICard(Card card) {
            return uiCards.FirstOrDefault(uiCard => uiCard.Card == card);
        }
        #endregion

        #region Unity Methods
        private void Awake() {
            CardManager.Instance.OnCardInserted += Insert;
            CardManager.Instance.OnCardSelected += Select;
            CardManager.Instance.OnCardUnselected += Unselect;
            CardManager.Instance.OnCardsPlayed += Play;
        }
        #endregion
    }
}
