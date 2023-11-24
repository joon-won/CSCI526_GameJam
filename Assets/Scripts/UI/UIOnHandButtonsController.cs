using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace CSCI526GameJam {
    public class UIOnHandButtonsController : MonoBehaviour {

        #region Fields
        [MandatoryFields]
        [SerializeField] private TMP_Text patternText;
        [SerializeField] private TMP_Text costText;
        [SerializeField] private Button playButton;
        [SerializeField] private Button unselectButton;

        private CardManager cardManager;
        private Player player;
        #endregion

        #region Publics
        #endregion

        #region Internals
        private void OnCardSelectedHandler(Card card) {
            gameObject.SetActive(true);
            RefreshInfo();
        }

        private void OnCardUnselectedHandler(Card card) {
            if (cardManager.NumSelected == 0) {
                gameObject.SetActive(false);
                return;
            }
            gameObject.SetActive(true);
            RefreshInfo();
        }

        private void RefreshInfo() {
            var pattern = cardManager.CurrentPattern;
            if (pattern == CardManager.Pattern.None) {
                patternText.text = "Invalid";
                patternText.color = Color.red;
            }
            else {
                patternText.text = cardManager.CurrentPattern.ToString();
                patternText.color = Color.white;
            }

            var cost = cardManager.CurrentCost;
            costText.text = cost.ToString();
            costText.color = player.CanPay(cost) ? Color.white : Color.red;
        }
        #endregion

        #region Unity Methods
        private void Awake() {
            gameObject.SetActive(false);

            cardManager = CardManager.Instance;
            player = Player.Instance;

            cardManager.OnCardSelected += OnCardSelectedHandler;
            cardManager.OnCardUnselected += OnCardUnselectedHandler;

            playButton.onClick.AddListener(
                    () => {
                        if (cardManager.PlaySelected()) {
                            gameObject.SetActive(false);
                        }
                    }
                );
            unselectButton.onClick.AddListener(() => cardManager.UnselectAll());
        }
        #endregion
    }
}
