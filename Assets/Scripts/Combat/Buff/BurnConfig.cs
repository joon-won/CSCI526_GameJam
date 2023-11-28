using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSCI526GameJam.Buffs.EnemyBuffs {

    [CreateAssetMenu(menuName = "Buffs/Enemy/Burn")]
    public class BurnConfig : BuffConfig {

        #region Fields
        [SerializeField] private float totalDamage;
        [SerializeField] private float damageInterval;
        #endregion

        #region Publics
        public override Buff ToBuff(Buffable target) {
            return new Burn(this, target, totalDamage, damageInterval);
        }
        #endregion

        #region Internals
        #endregion

        #region Unity Methods
        #endregion


        [Serializable]
        private class Burn : Buff {

            #region Fields
            [SerializeField] private Enemy enemy;
            [SerializeField] private float damagePerTick;
            [SerializeField] private float damageInterval;
            [SerializeField] private float damageElapsed;
            #endregion

            #region Publics
            public Burn(BurnConfig config, Buffable target, float totalDamage, float interval) : base(config, target) {
                enemy = target.TryCast<Enemy>();

                damageInterval = Mathf.Min(config.Duration, interval);

                var numTicks = config.Duration / damageInterval;
                damagePerTick = totalDamage / numTicks;
            }
            #endregion

            #region Internals
            protected override void ApplyEffect() {

            }

            protected override void EndEffect() {

            }

            protected override void UpdateEffect() {
                damageElapsed += Time.deltaTime;
                if (damageElapsed > damageInterval) {
                    enemy.TakeDamage(damagePerTick);
                    damageElapsed = 0f;
                }
            }
            #endregion

            #region Unity Methods
            #endregion
        }
    }
}
