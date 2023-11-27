using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace CSCI526GameJam {
    public class UIPopups : MonoBehaviourSingleton<UIPopups> {
        
        #region Fields
        [MandatoryFields]
        [SerializeField] private UITowerInfoPopup towerInfo;
        [SerializeField] private UIEffectInfoPopup effectInfo;
        #endregion

        #region Publics
        public UITowerInfoPopup TowerInfo => towerInfo;
        public UIEffectInfoPopup EffectInfo => effectInfo;
        #endregion

        #region Internals
        #endregion

        #region Unity Methods
        protected override void Awake() {
            base.Awake();

            towerInfo.Init();
            effectInfo.Init();
        }
        #endregion
    }
}
