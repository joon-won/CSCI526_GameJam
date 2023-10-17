using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSCI526GameJam {

    /// <summary>
    /// Item for test. Increase tower damage scalar. 
    /// </summary>
    [CreateAssetMenu(menuName = "Config/Card/Test/Apple")]
    public class Apple : Card {

        #region Fields
        [ClassHeader(typeof(Apple))]
        [SerializeField] private float lv1BonusTowerADScalar;
        [SerializeField] private float lv2BonusTowerADScalar;
        [SerializeField] private float lv3BonusTowerADScalar;
        #endregion

        #region Publics
        public override void PlayLv1() {
            EffectManager.Instance.AddEffect(
                () => Stats.Instance.Tower.AttackDamage.Scalar.IncreaseValue(lv1BonusTowerADScalar),
                () => Stats.Instance.Tower.AttackDamage.Scalar.DecreaseValue(lv1BonusTowerADScalar));
        }

        public override void PlayLv2() {
            EffectManager.Instance.AddEffect(
                () => Stats.Instance.Tower.AttackDamage.Scalar.IncreaseValue(lv2BonusTowerADScalar),
                () => Stats.Instance.Tower.AttackDamage.Scalar.DecreaseValue(lv2BonusTowerADScalar));
        }

        public override void PlayLv3() {
            EffectManager.Instance.AddEffect(
                () => Stats.Instance.Tower.AttackDamage.Scalar.IncreaseValue(lv3BonusTowerADScalar),
                () => Stats.Instance.Tower.AttackDamage.Scalar.DecreaseValue(lv3BonusTowerADScalar));
        }
        #endregion

        #region Internals
        #endregion

        #region Unity Methods
        #endregion
    }
}
