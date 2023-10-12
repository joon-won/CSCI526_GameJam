using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSCI526GameJam {

    /// <summary>
    /// Item for test. Increase tower crit chance value. 
    /// </summary>
    [CreateAssetMenu(menuName = "Config/Card/Test/Cat")]
    public class Cat : Card {

        #region Fields
        [ClassHeader(typeof(Cat))]

        [SerializeField] private float lv1BonusTowerCCValue;
        [SerializeField] private float lv2BonusTowerCCValue;
        [SerializeField] private float lv3BonusTowerCCValue;
        #endregion

        #region Publics
        public override void PlayLv1() {
            Stats.Instance.Tower.CritChance.Value.IncreaseValue(lv1BonusTowerCCValue);
        }

        public override void PlayLv2() {
            Stats.Instance.Tower.CritChance.Value.IncreaseValue(lv2BonusTowerCCValue);
        }

        public override void PlayLv3() {
            Stats.Instance.Tower.CritChance.Value.IncreaseValue(lv3BonusTowerCCValue);
        }
        #endregion

        #region Internals
        #endregion

        #region Unity Methods
        #endregion
    }
}
