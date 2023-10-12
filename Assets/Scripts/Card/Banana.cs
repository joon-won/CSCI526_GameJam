using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSCI526GameJam {

    /// <summary>
    /// Item for test. Inscrease tower attack speed scalar. 
    /// </summary>
    [CreateAssetMenu(menuName = "Config/Card/Test/Banana")]
    public class Banana : Card {

        #region Fields
        [ClassHeader(typeof(Banana))]

        [SerializeField] private float lv1BonusTowerASScalar;
        [SerializeField] private float lv2BonusTowerASScalar;
        [SerializeField] private float lv3BonusTowerASScalar;
        #endregion

        #region Publics
        public override void PlayLv1() {
            Stats.Instance.Tower.AttackSpeed.Scalar.IncreaseValue(lv1BonusTowerASScalar);
        }

        public override void PlayLv2() {
            Stats.Instance.Tower.AttackSpeed.Scalar.IncreaseValue(lv2BonusTowerASScalar);
        }

        public override void PlayLv3() {
            Stats.Instance.Tower.AttackSpeed.Scalar.IncreaseValue(lv3BonusTowerASScalar);
        }
        #endregion

        #region Internals
        #endregion

        #region Unity Methods
        #endregion
    }
}