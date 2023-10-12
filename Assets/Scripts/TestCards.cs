using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CSCI526GameJam {
    public class TestCards : MonoBehaviour {

        #region Fields
        [SerializeField] private TestCard testCardPrefab;
        [SerializeField] private List<TestCard> cards = new();
        #endregion

        #region Publics
        #endregion

        #region Internals
        private void Insert(int index) {
            var card = Instantiate(testCardPrefab, transform);
            card.Init(CardManager.Instance.Get(index));
            card.transform.SetSiblingIndex(index);
            card.GetComponent<Button>().onClick.AddListener(
                () => CardManager.Instance.ToggleSelected(cards.IndexOf(card)));

            cards.Insert(index, card);
        }

        private void Select(int index) {
            cards[index].GetComponent<Image>().color = Color.red;
        }

        private void Unselect(int index) {
            cards[index].GetComponent<Image>().color = Color.white;
        }

        private void Play(List<int> indices) {
            indices.Sort((i, j) => j.CompareTo(i));
            for (int i = 0; i < indices.Count; i++) {
                Destroy(cards[indices[i]].gameObject);
                cards.RemoveAt(indices[i]);
            }
        }
        #endregion

        #region Unity Methods
        private void OnEnable() {
            CardManager.Instance.OnCardInserted += Insert;
            CardManager.Instance.OnCardSelected += Select;
            CardManager.Instance.OnCardUnselected += Unselect;
            CardManager.Instance.OnCardsPlayed += Play;
        }
        #endregion
    }
}
