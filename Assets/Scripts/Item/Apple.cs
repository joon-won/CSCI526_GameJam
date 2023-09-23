using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSCI526GameJam {

    /// <summary>
    /// Item for test. Increase tower damage scalar. 
    /// </summary>
    [CreateAssetMenu(menuName = "Config/Item/Test/Apple")]
    public class Apple : ItemConfig {

        #region Fields
        [ClassHeader(typeof(Apple))]

        [MandatoryFields]
        [SerializeField] private float bonusTowerADScalar;
        #endregion

        #region Publics
        public override void OnAdd() {
            Stats.Instance.Tower.AttackDamage.Scalar.IncreaseValue(bonusTowerADScalar);
        }

        public override void OnRemove() {
            Stats.Instance.Tower.AttackDamage.Scalar.DecreaseValue(bonusTowerADScalar);
        }
        #endregion

        #region Internals
        #endregion

        #region Unity Methods
        #endregion
    }
}
