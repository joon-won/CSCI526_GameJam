using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSCI526GameJam {

    [CreateAssetMenu(menuName = "Config/Card/Restore HP")]
    public class RestoreHp : CardConfig {

        #region Fields
        [ClassHeader(typeof(RestoreHp))]

        [SerializeField] private int lv1Heal;
        [SerializeField] private int lv2Heal;
        [SerializeField] private int lv3Heal;
        #endregion

        #region Publics
        public override void PlayLv1() {
            TowerManager.Instance.PlayerBase.Heal(lv1Heal);
        }

        public override void PlayLv2() {
            TowerManager.Instance.PlayerBase.Heal(lv2Heal);
        }

        public override void PlayLv3() {
            TowerManager.Instance.PlayerBase.Heal(lv3Heal);
        }
        #endregion

        #region Internals
        #endregion

        #region Unity Methods
        #endregion
    }
}
