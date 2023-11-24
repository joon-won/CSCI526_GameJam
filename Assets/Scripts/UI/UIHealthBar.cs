using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace CSCI526GameJam {
    public class UIHealthBar : MonoBehaviour {

        #region Fields
        [ClassHeader(typeof(UIHealthBar))]

        [MandatoryFields]
        [SerializeField] private RectTransform health;
        [SerializeField] private TMP_Text healthText;
        #endregion

        #region Publics
        public void Refresh(float current, float max) {
            var value = current / max;
            value = Mathf.Clamp01(value);

            health.localScale =
                new Vector3(
                    value,
                    health.localScale.y,
                    health.localScale.z);

            healthText.text = $"{(int)current} / {(int)max}";
        }
        #endregion

        #region Internals
        #endregion

        #region Unity Methods
        #endregion
    }
}
