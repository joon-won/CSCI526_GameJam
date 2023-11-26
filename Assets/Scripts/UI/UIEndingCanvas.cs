using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace CSCI526GameJam {
    public class UIEndingCanvas : MonoBehaviour {

        #region Fields
        [MandatoryFields]
        [SerializeField] private Button restartButton;
        [SerializeField] private Button mainMenuButton;
        #endregion

        #region Publics
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

            restartButton.onClick.AddListener(() => GameManager.Instance.LoadScene(Configs.GameplaySceneIndex));
            mainMenuButton.onClick.AddListener(() => GameManager.Instance.LoadScene(Configs.MainMenuSceneIndex));
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
