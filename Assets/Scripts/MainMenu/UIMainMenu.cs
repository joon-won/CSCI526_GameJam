using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CSCI526GameJam {
    public class UIMainMenu : MonoBehaviour {

        #region Fields
        [MandatoryFields]
        [SerializeField] private Button startButton;
        [SerializeField] private Toggle tutorialToggle;
        #endregion

        #region Publics
        #endregion

        #region Internals
        #endregion

        #region Unity Methods
        private void Awake() {
            startButton.onClick.AddListener(() => SceneManager.LoadScene(Configs.GameplaySceneIndex));
            tutorialToggle.onValueChanged.AddListener(isToggled => GameManager.Instance.DoTutorial = isToggled);
            GameManager.Instance.DoTutorial = tutorialToggle.isOn;
        }
        #endregion
    }
}
