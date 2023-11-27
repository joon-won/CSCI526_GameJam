using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSCI526GameJam.Effects {

    [CreateAssetMenu(menuName = "Effects/Increase Damage")]
    public class IncDamageConfig : EffectConfig {

        #region Fields
        [ClassHeader(typeof(IncDamageConfig))]

        [SerializeField] private float damagePercentage;
        #endregion

        #region Publics
        public float DamagePercentage => damagePercentage;

        public override Effect ToEffect() {
            return new IncDamage(this);
        }
        #endregion

        #region Internals
        #endregion

        #region Unity Methods
        #endregion


        [Serializable]
        public class IncDamage : Effect<IncDamageConfig> {

            #region Fields
            #endregion

            #region Publics
            public IncDamage(IncDamageConfig config) : base(config) {

            }
            #endregion

            #region Internals
            protected override void ApplyEffect() {
                StatSystem.Instance.Tower.AttackDamage.Scalar.IncreaseValue(Config.DamagePercentage);
            }

            protected override void EndEffect() {
                StatSystem.Instance.Tower.AttackDamage.Scalar.DecreaseValue(Config.DamagePercentage);
            }

            protected override void UpdateEffect() {

            }
            #endregion

            #region Unity Methods
            #endregion
        }
    }
}
