using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace CSCI526GameJam {
    public class UIPlayerInfo : MonoBehaviour {

        #region Fields
        [MandatoryFields]
        [SerializeField] private UIHealthBar healthBar;
        [SerializeField] private TMP_Text goldText;
        #endregion

        #region Publics
        #endregion

        #region Internals
        #endregion

        #region Unity Methods
        private void Update() {
            var playerBase = TowerManager.Instance.PlayerBase;
            healthBar.Refresh(playerBase.Health, playerBase.MaxHealth);

            goldText.text = Player.Instance.Gold.ToString();
        }
        #endregion
    }
}
