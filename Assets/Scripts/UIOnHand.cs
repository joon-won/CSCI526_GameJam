using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using DG.Tweening;

namespace CSCI526GameJam {
    public class UIOnHand : MonoBehaviour {

        #region Fields
        [MandatoryFields]
        [SerializeField] private UICard uiCardPrefab;

        [SerializeField] private RectTransform cardAnimStart;
        [SerializeField] private float cardStartAnimInterval;

        [ComputedFields]
        [SerializeField] private List<UICard> uiCards = new();
        #endregion

        #region Publics
        public void ForceClearUICards() {
            uiCards.ForEach(x => Destroy(x.gameObject));
            uiCards.Clear();
        }
        #endregion

        #region Internals
        private void Insert(Card[] insertedCards) {
            insertedCards = insertedCards.OrderBy(x => CardManager.Instance.IndexOf(x)).ToArray();
            for (int i = 0; i < insertedCards.Length; i++) {
                StartCoroutine(InsertRoutine(insertedCards[i], cardStartAnimInterval * i));
            }
        }

        private IEnumerator InsertRoutine(Card insertedCard, float delay) {
            var index = CardManager.Instance.IndexOf(insertedCard);

            var card = Instantiate(uiCardPrefab, transform);
            card.transform.SetSiblingIndex(index);
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)transform);

            card.Init(insertedCard,
                () => CardManager.Instance.ToggleSelected(uiCards.IndexOf(card)));
            uiCards.Insert(index, card);

            yield return new WaitForSeconds(delay);

            if (!card) yield break;

            card.DoStartAnim(cardAnimStart.position);
        }

        private void Select(Card card) {
            GetUICard(card).Select();
            uiCards.ForEach(x => x.Refresh());
        }

        private void Unselect(Card card) {
            GetUICard(card).UnSelect();
            uiCards.ForEach(x => x.Refresh());
        }

        private void Play(Card[] cards) {
            foreach (var card in cards) {
                var uiCard = GetUICard(card);
                if (!uiCard) {
                    Debug.LogWarning($"{card.Config.name} is not found in UI cards. ");
                    continue;
                }
                uiCard.DoPlayAnim(() => {
                    Destroy(uiCard.gameObject);
                    uiCards.Remove(uiCard);
                });
            }
        }

        private void Discard(Card[] cards) {
        }

        private UICard GetUICard(Card card) {
            return uiCards.FirstOrDefault(uiCard => uiCard.Card == card);
        }


        #endregion

        #region Unity Methods
        private void Awake() {
            CardManager.Instance.OnCardsInserted += Insert;
            CardManager.Instance.OnCardSelected += Select;
            CardManager.Instance.OnCardUnselected += Unselect;
            CardManager.Instance.OnCardsPlayed += Play;
            CardManager.Instance.OnForceClearOnHand += ForceClearUICards;
        }
        #endregion
    }
}
