using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CSCI526GameJam {
    public class UIPauseMenu : MonoBehaviour, IInitializable {
        
        #region Fields
        [MandatoryFields]
        [SerializeField] private Button continueButton;
        [SerializeField] private Button restartButton;
        [SerializeField] private Button mainMenuButton;
        #endregion

        #region Publics
        #endregion

        #region Internals
        public void Init() {
            gameObject.SetActive(false);

            continueButton.onClick.AddListener(() => gameObject.SetActive(false));
            restartButton.onClick.AddListener(() => GameManager.Instance.LoadScene(Configs.GameplaySceneIndex));
            mainMenuButton.onClick.AddListener(() => GameManager.Instance.LoadScene(Configs.MainMenuSceneIndex));

            InputManager.Instance.OnEscapeDown += Toggle;
            GameManager.Instance.OnCurrentSceneExiting += () => {
                InputManager.Instance.OnEscapeDown -= Toggle;
            };
        }

        private void Toggle() {
            gameObject.SetActive(!gameObject.activeSelf);
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
