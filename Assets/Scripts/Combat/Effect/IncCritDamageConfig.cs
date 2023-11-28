using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSCI526GameJam {

    [CreateAssetMenu(menuName = "Effects/Increase Crit Damage")]
    public class IncCritDamageConfig : EffectConfig {

        #region Fields
        [ClassHeader(typeof(DecArmorConfig))]

        [SerializeField] private float critDmgInc;
        #endregion

        #region Publics
        public override Effect ToEffect() {
            return new IncCritDamage(this);
        }
        #endregion

        #region Internals
        #endregion

        #region Unity Methods
        #endregion


        [Serializable]
        public class IncCritDamage : Effect<IncCritDamageConfig> {

            #region Fields
            #endregion

            #region Publics
            public IncCritDamage(IncCritDamageConfig config) : base(config) {

            }
            #endregion

            #region Internals
            protected override void ApplyEffect() {
                StatSystem.Instance.Tower.CritDamage.Value.IncreaseValue(Config.critDmgInc);
            }

            protected override void EndEffect() {
                StatSystem.Instance.Tower.CritDamage.Value.DecreaseValue(Config.critDmgInc);
            }

            protected override void UpdateEffect() {

            }
            #endregion

            #region Unity Methods
            #endregion
        }
    }
}
