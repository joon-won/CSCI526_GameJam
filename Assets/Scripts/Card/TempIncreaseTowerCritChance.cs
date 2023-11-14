using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSCI526GameJam {

    [CreateAssetMenu(menuName = "Config/Card/<Temp> Increase Tower Crit Chance")]
    public class TempIncreaseTowerCritChance : CardConfig {

        #region Fields
        [ClassHeader(typeof(TempIncreaseTowerCritChance))]

        [SerializeField] private float lv1Increase;
        [SerializeField] private float lv2Increase;
        [SerializeField] private float lv3Increase;
        #endregion

        #region Publics
        public override void PlayLv1() {
            EffectManager.Instance.AddEffect(
                () => Stats.Instance.Tower.CritChance.Scalar.IncreaseValue(lv1Increase),
                () => Stats.Instance.Tower.CritChance.Scalar.DecreaseValue(lv1Increase),
                duration: 1
            );
        }

        public override void PlayLv2() {
            EffectManager.Instance.AddEffect(
                () => Stats.Instance.Tower.CritChance.Scalar.IncreaseValue(lv2Increase),
                () => Stats.Instance.Tower.CritChance.Scalar.DecreaseValue(lv2Increase),
                duration: 1
            );
        }

        public override void PlayLv3() {
            EffectManager.Instance.AddEffect(
                () => Stats.Instance.Tower.CritChance.Scalar.IncreaseValue(lv3Increase),
                () => Stats.Instance.Tower.CritChance.Scalar.DecreaseValue(lv3Increase)
            );
        }
        #endregion

        #region Internals
        #endregion

        #region Unity Methods
        #endregion
    }
}
