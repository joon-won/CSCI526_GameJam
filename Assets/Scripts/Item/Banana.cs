using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSCI526GameJam {

    /// <summary>
    /// Item for test. Inscrease tower attack speed scalar. 
    /// </summary>
    [CreateAssetMenu(menuName = "Config/Item/Test/Banana")]
    public class Banana : ItemConfig {

        #region Fields
        [ClassHeader(typeof(Banana))]

        [MandatoryFields]
        [SerializeField] private float bonusTowerASScalar;
        #endregion

        #region Publics
        public override void OnAdd() {
            Stats.Instance.Tower.AttackSpeed.Scalar.IncreaseValue(bonusTowerASScalar);
        }

        public override void OnRemove() {
            Stats.Instance.Tower.AttackSpeed.Scalar.DecreaseValue(bonusTowerASScalar);
        }
        #endregion

        #region Internals
        #endregion

        #region Unity Methods
        #endregion
    }
}
