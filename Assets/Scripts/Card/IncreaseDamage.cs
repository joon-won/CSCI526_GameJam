using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSCI526GameJam {

    [CreateAssetMenu(menuName = "Config/Card/Increase Damage")]
    public class IncreaseDamage : CardConfig {

        #region Fields
        [ClassHeader(typeof(IncreaseDamage))]

        [SerializeField] private EffectConfig lv1Config;
        [SerializeField] private EffectConfig lv2Config;
        [SerializeField] private EffectConfig lv3Config;
        #endregion

        #region Publics
        public override void PlayLv1() {
            EffectManager.Instance.AddEffect(lv1Config.ToEffect());
        }

        public override void PlayLv2() {
            EffectManager.Instance.AddEffect(lv2Config.ToEffect());
        }

        public override void PlayLv3() {
            EffectManager.Instance.AddEffect(lv3Config.ToEffect());
        }
        #endregion

        #region Internals
        #endregion

        #region Unity Methods
        #endregion
    }
}
