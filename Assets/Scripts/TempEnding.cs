using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSCI526GameJam {
    public class TempEnding : MonoBehaviour {

        #region Fields
        #endregion

        #region Publics
        public void PlayAgain() {
            GameManager.Instance.LoadGameplayScene();
        }

        public void Quit() {
            ApplicationHelper.Quit();
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
        }
        #endregion
    }
}
