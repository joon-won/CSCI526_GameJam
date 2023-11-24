using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace CSCI526GameJam {
    public class UILevelInfo : MonoBehaviour {

        #region Fields
        [MandatoryFields]
        [SerializeField] private TMP_Text levelText;

        private Coroutine showRoutine;
        #endregion

        #region Publics
        public void Show(int level, float duration) {
            gameObject.SetActive(true);

            if (showRoutine != null) {
                StopCoroutine(showRoutine);
            }
            showRoutine = StartCoroutine(ShowRoutine(level, duration));
        }
        #endregion

        #region Internals
        private IEnumerator ShowRoutine(int level, float duration) {
            levelText.text = $"Level {level}";
            yield return new WaitForSeconds(duration);

            gameObject.SetActive(false);
        }
        #endregion

        #region Unity Methods
        private void Awake() {
            gameObject.SetActive(false);
        }
        #endregion
    }
}
