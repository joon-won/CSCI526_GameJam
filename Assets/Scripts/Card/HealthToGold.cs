using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSCI526GameJam {

    [CreateAssetMenu(menuName = "Config/Card/Health To Gold")]
    public class HealthToGold : CardConfig {

        #region Fields
        [SerializeField] private int healthCost;

        [SerializeField] private int lv1Gold;
        [SerializeField] private int lv2Gold;
        [SerializeField] private int lv3Gold;
        #endregion

        #region Publics
        public override void PlayLv1() {
            TowerManager.Instance.PlayerBase.TakeDamage(healthCost);
            Player.Instance.AddGold(lv1Gold);
        }

        public override void PlayLv2() {
            TowerManager.Instance.PlayerBase.TakeDamage(healthCost);
            Player.Instance.AddGold(lv2Gold);
        }

        public override void PlayLv3() {
            TowerManager.Instance.PlayerBase.TakeDamage(healthCost);
            Player.Instance.AddGold(lv3Gold);
        }
        #endregion

        #region Internals
        #endregion

        #region Unity Methods
        #endregion
    }
}
