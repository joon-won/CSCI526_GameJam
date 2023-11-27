using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CSCI526GameJam {
    public class UIEndingCanvas : MonoBehaviour, IInitializable {

        #region Fields
        [MandatoryFields]
        [SerializeField] private TMP_Text messageText;
        [SerializeField] private Button restartButton;
        [SerializeField] private Button mainMenuButton;
        #endregion

        #region Publics
        public void Init() {
            GameManager.Instance.OnGameOver += OnGameOverHandler;
            GameManager.Instance.OnGameWon += OnGameWonHandler;
            GameManager.Instance.OnCurrentSceneExiting += () => {
                GameManager.Instance.OnGameOver -= OnGameOverHandler;
                GameManager.Instance.OnGameWon -= OnGameWonHandler;
            };
            gameObject.SetActive(false);

            restartButton.onClick.AddListener(() => GameManager.Instance.LoadScene(Configs.GameplaySceneIndex));
            mainMenuButton.onClick.AddListener(() => GameManager.Instance.LoadScene(Configs.MainMenuSceneIndex));
        }
        #endregion

        #region Internals
        private void OnGameWonHandler() {
            messageText.text = "YOU WIN";
            gameObject.SetActive(true);
        }
        
        private void OnGameOverHandler() {
            messageText.text = "YOU LOSE";
            gameObject.SetActive(true);
        }
        #endregion

        #region Unity Methods
        private void OnEnable() {
            Time.timeScale = 0f;
        }

        private void OnDisable() {
            Time.timeScale = 1f;
        }
        #endregion
    }
}
