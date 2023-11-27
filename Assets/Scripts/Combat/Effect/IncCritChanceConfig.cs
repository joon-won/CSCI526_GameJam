using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSCI526GameJam.Effects {

    [CreateAssetMenu(menuName = "Effects/Increase Crit Chance")]
    public class IncCritChanceConfig : EffectConfig {

        #region Fields
        [ClassHeader(typeof(IncCritChanceConfig))]

        [SerializeField] private float critChance;
        #endregion

        #region Publics
        public float CritChance => critChance;

        public override Effect ToEffect() {
            return new IncCritChance(this);
        }
        #endregion

        #region Internals
        #endregion

        #region Unity Methods
        #endregion


        [Serializable]
        public class IncCritChance : Effect<IncCritChanceConfig> {

            #region Fields
            #endregion

            #region Publics
            public IncCritChance(IncCritChanceConfig config) : base(config) {

            }
            #endregion

            #region Internals
            protected override void ApplyEffect() {
                StatSystem.Instance.Tower.CritChance.Value.IncreaseValue(Config.CritChance);
            }

            protected override void EndEffect() {
                StatSystem.Instance.Tower.CritChance.Value.DecreaseValue(Config.CritChance);
            }

            protected override void UpdateEffect() {

            }
            #endregion

            #region Unity Methods
            #endregion
        }
    }
}
