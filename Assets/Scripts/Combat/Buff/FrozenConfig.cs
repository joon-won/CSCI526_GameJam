using System;
using System.Collections;
using System.Collections.Generic;
using CSCI526GameJam.Buffs.TowerBuffs;
using UnityEngine;

namespace CSCI526GameJam.Buffs.EnemyBuffs {

    [CreateAssetMenu(menuName = "Buffs/Enemy/Frozen")]
    public class FrozenConfig : BuffConfig {

        #region Fields
        [ClassHeader(typeof(FrozenConfig))]
        [SerializeField] private float slowRatio;
        #endregion

        #region Publics
        public override Buff ToBuff(Buffable target) {
            return new Frozen(this, target);
        }
        #endregion

        #region Internals
        #endregion

        #region Unity Methods
        #endregion


        [Serializable]
        public class Frozen : Buff {

            #region Fields
            [ClassHeader(typeof(Frozen))]
            [SerializeField] private Enemy enemy;
            #endregion

            #region Publics
            public Frozen(FrozenConfig config, Buffable target) : base(config, target) {
                enemy = target.TryCast<Enemy>();
            }
            #endregion

            #region Internals
            protected override void ApplyEffect() {
                enemy.MoveSpeed.DecreaseScalar(((FrozenConfig)config).slowRatio);
            }

            protected override void EndEffect() {
                enemy.MoveSpeed.IncreaseScalar(((FrozenConfig)config).slowRatio);
            }

            protected override void UpdateEffect() {
                return;
            }
            #endregion

            #region Unity Methods
            #endregion
        }
    }
}
