using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CSCI526GameJam {
    public class UIStartCombatButton : MonoBehaviour {

        #region Fields
        [MandatoryFields]
        [SerializeField] private Button startCombatButton;
        [SerializeField] private UILevelInfo levelInfo;
        [SerializeField] private float levelDisplayDuration;
        #endregion

        #region Publics
        #endregion

        #region Internals
        private void OnCombatEnded() {
            gameObject.SetActive(true);
        }
        #endregion

        #region Unity Methods
        private void Awake() {
            startCombatButton.onClick.AddListener(() => {
                GameManager.Instance.StartCombat();
                levelInfo.Show(GameManager.Instance.Level, levelDisplayDuration);
                startCombatButton.gameObject.SetActive(false);
            });

            gameObject.SetActive(true);

            GameManager.Instance.OnPreparationStarted += OnCombatEnded;
            GameManager.Instance.OnCurrentSceneExiting += () => {
                GameManager.Instance.OnPreparationStarted -= OnCombatEnded;
            };
        }
        #endregion
    }
}
