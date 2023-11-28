using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSCI526GameJam {

    [CreateAssetMenu(menuName = "Effects/Decrease Armor")]
    public class DecArmorConfig : EffectConfig {

        #region Fields
        [ClassHeader(typeof(DecArmorConfig))]

        [SerializeField] private float armorDec;
        #endregion

        #region Publics
        public override Effect ToEffect() {
            return new DecArmor(this);
        }
        #endregion

        #region Internals
        #endregion

        #region Unity Methods
        #endregion


        [Serializable]
        public class DecArmor : Effect<DecArmorConfig> {

            #region Fields
            #endregion

            #region Publics
            public DecArmor(DecArmorConfig config) : base(config) {

            }
            #endregion

            #region Internals
            protected override void ApplyEffect() {
                StatSystem.Instance.Enemy.Armor.Scalar.DecreaseValue(Config.armorDec);
            }

            protected override void EndEffect() {
                StatSystem.Instance.Enemy.Armor.Scalar.IncreaseValue(Config.armorDec);
            }

            protected override void UpdateEffect() {

            }
            #endregion

            #region Unity Methods
            #endregion
        }
    }
}
