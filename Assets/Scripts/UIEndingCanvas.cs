using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CSCI526GameJam {
    public class UIEndingCanvas : MonoBehaviour {

        #region Fields
        [MandatoryFields]
        [SerializeField] private Button restartButton;
        #endregion

        #region Publics
        public void RestartGame() {
            GameManager.Instance.LoadGameplayScene();
        }
        #endregion

        #region Internals
        private void Show() {
            gameObject.SetActive(true);
        }
        #endregion

        #region Unity Methods
        private void Awake() {
            GameManager.Instance.OnGameOver += Show;
            GameManager.Instance.OnGameWon += Show;
            GameManager.Instance.OnCurrentSceneExiting += () => {
                GameManager.Instance.OnGameOver -= Show;
                GameManager.Instance.OnGameWon -= Show;
            };
            gameObject.SetActive(false);

            restartButton.onClick.AddListener(RestartGame);
        }

        private void OnEnable() {
            Time.timeScale = 0f;
        }

        private void OnDisable() {
            Time.timeScale = 1f;
        }
        #endregion
    }
}
