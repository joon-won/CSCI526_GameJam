using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CSCI526GameJam {
    public class UICombatInfo : MonoBehaviour {

        #region Fields
        [MandatoryFields]
        [SerializeField] private Button startCombatButton;

        [SerializeField] private Transform combatInfo;
        [SerializeField] private TMP_Text numEnemiesText;
        #endregion

        #region Publics
        #endregion

        #region Internals
        private void OnCombatEnded() {
            startCombatButton.gameObject.SetActive(true);
            combatInfo.gameObject.SetActive(false);
        }
        #endregion

        #region Unity Methods
        private void Awake() {
            startCombatButton.onClick.AddListener(() => {
                GameManager.Instance.StartCombat();
                startCombatButton.gameObject.SetActive(false);
                combatInfo.gameObject.SetActive(true);
            });

            startCombatButton.gameObject.SetActive(true);
            combatInfo.gameObject.SetActive(false);

            Player.Instance.OnModeChanged += mode => {
                var hideStartCombatButton = mode == Player.Mode.Build || mode == Player.Mode.Demolish;
                startCombatButton.gameObject.SetActive(!hideStartCombatButton);
            };

            GameManager.Instance.OnPreparationStarted += OnCombatEnded;
            GameManager.Instance.OnCurrentSceneExiting += () => {
                GameManager.Instance.OnPreparationStarted -= OnCombatEnded;
            };
        }

        private void Update() {
            if (combatInfo.gameObject.activeSelf) {
                numEnemiesText.text = EnemyManager.Instance.NumEnemies.ToString();
            }
        }
        #endregion
    }
}
