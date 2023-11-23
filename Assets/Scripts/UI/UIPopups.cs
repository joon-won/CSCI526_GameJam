using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace CSCI526GameJam {
    public class UIPopups : MonoBehaviourSingleton<UIPopups> {
        
        #region Fields
        [MandatoryFields]
        [SerializeField] private UITowerInfo towerInfo;
        #endregion

        #region Publics
        public UITowerInfo TowerInfo => towerInfo;
        #endregion

        #region Internals
        #endregion

        #region Unity Methods
        protected override void Awake() {
            base.Awake();

            towerInfo.Init();
        }
        #endregion
    }
}
