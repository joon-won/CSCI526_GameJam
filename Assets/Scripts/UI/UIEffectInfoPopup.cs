using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace CSCI526GameJam {
    public class UIEffectInfoPopup : MonoBehaviour {

        #region Fields
        [SerializeField] private TMP_Text descriptionText;
        [SerializeField] private TMP_Text remainingText;
        [SerializeField] private TMP_Text stackText;

        private RectTransform rectTransform;
        #endregion

        #region Publics
        public void Init() {
            rectTransform = transform as RectTransform;
            Hide();
        }

        public void Show(Effect effect, RectTransform anchor) {
            gameObject.SetActive(true);

            rectTransform.pivot = anchor.pivot;
            rectTransform.position = anchor.position;

            descriptionText.text = effect.Config.Description;

            if (effect.Config.IsPermanent) {
                remainingText.text = "Permanent";
            }
            else {
                var remaining = effect.Duration - effect.Elapsed;
                var roundStr = remaining == 1 ? "round" : "rounds";
                remainingText.text = $"{remaining} {roundStr} remain";
            }

            stackText.text = $"x {effect.NumStacks}";
        }

        public void Hide() {
            gameObject.SetActive(false);
        }
        #endregion

        #region Internals
        #endregion

        #region Unity Methods
        #endregion
    }
}
