using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSCI526GameJam {

    [CreateAssetMenu(menuName = "Config/Card/Increase Crit Damage")]
    public class IncreaseCritDamageConfig : CardConfig {

        #region Fields
        [SerializeField] private IncCritDamageConfig lv1;
        [SerializeField] private IncCritDamageConfig lv2;
        [SerializeField] private IncCritDamageConfig lv3;
        #endregion

        #region Publics
        public override void PlayLv1() {
            EffectManager.Instance.AddEffect(lv1.ToEffect());
        }

        public override void PlayLv2() {
            EffectManager.Instance.AddEffect(lv2.ToEffect());
        }

        public override void PlayLv3() {
            EffectManager.Instance.AddEffect(lv3.ToEffect());
        }
        #endregion

        #region Internals
        #endregion

        #region Unity Methods
        #endregion
    }
}
