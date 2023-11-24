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
        [SerializeField] private float duration;

        private Coroutine showRoutine;
        private GameManager gameManager;
        #endregion

        #region Publics
        #endregion

        #region Internals
        private void Show() {
            gameObject.SetActive(true);

            if (showRoutine != null) {
                StopCoroutine(showRoutine);
            }
            showRoutine = StartCoroutine(ShowRoutine(duration));
        }

        private IEnumerator ShowRoutine(float duration) {
            var level = gameManager.IsInTutorial ? gameManager.TutorialLevel : gameManager.Level;
            level++;
            levelText.text = gameManager.IsInTutorial ? $"Tutorial {level}" : $"Level {level}";
            yield return new WaitForSeconds(duration);

            gameObject.SetActive(false);
        }
        #endregion

        #region Unity Methods
        private void Awake() {
            gameObject.SetActive(false);
            gameManager = GameManager.Instance;

            gameManager.OnPreparationStarted += Show;
            gameManager.OnCurrentSceneExiting += () => {
                gameManager.OnPreparationStarted -= Show;
            };
        }
        #endregion
    }
}
