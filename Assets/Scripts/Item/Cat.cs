using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSCI526GameJam {

    /// <summary>
    /// Item for test. Increase tower crit chance value. 
    /// </summary>
    [CreateAssetMenu(menuName = "Config/Item/Test/Cat")]
    public class Cat : ItemConfig {

        #region Fields
        [ClassHeader(typeof(Cat))]

        [MandatoryFields]
        [SerializeField] private int bonusTowerCCValue;
        #endregion

        #region Publics
        public override void OnAdd() {
            Stats.Instance.Tower.CritChance.Value.IncreaseValue(bonusTowerCCValue);
        }

        public override void OnRemove() {
            Stats.Instance.Tower.CritChance.Value.IncreaseValue(bonusTowerCCValue);
        }
        #endregion

        #region Internals
        #endregion

        #region Unity Methods
        #endregion
    }
}
