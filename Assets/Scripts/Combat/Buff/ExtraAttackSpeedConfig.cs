using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSCI526GameJam.Buffs.TowerBuffs {

    [CreateAssetMenu(menuName = "Buffs/Tower Buffs/Increase Attack Speed")]
    public class ExtraAttackSpeedConfig : BuffConfig {

        #region Fields
        [ClassHeader(typeof(ExtraAttackSpeedConfig))]

        [SerializeField] private float extraScalar;
        #endregion

        #region Publics
        public float ExtraScalar => extraScalar;

        public override Buff Make(Buffable target) {
            return new ExtraAttackSpeed(this, target);
        }
        #endregion

        #region Internals
        #endregion

        #region Unity Methods
        #endregion
    }

    [Serializable]
    public class ExtraAttackSpeed : Buff {

        #region Fields
        [ClassHeader(typeof(ExtraAttackSpeed))]

        [SerializeField] private Tower tower;
        #endregion

        #region Publics
        public ExtraAttackSpeed(BuffConfig config, Buffable target) : base(config, target) {
            tower = target.TryCast<Tower>();
        }
        #endregion

        #region Internals
        protected override void ApplyEffect() {
            tower.AttackSpeed.IncreaseScalar(((ExtraAttackSpeedConfig)config).ExtraScalar);
        }

        protected override void EndEffect() {
            tower.AttackSpeed.DecreaseScalar(((ExtraAttackSpeedConfig)config).ExtraScalar);
        }

        protected override void UpdateEffect() {
            return;
        }
        #endregion

        #region Unity Methods
        #endregion
    }
}
