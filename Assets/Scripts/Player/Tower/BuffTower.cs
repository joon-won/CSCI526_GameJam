using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CSCI526GameJam.Buffs;

namespace CSCI526GameJam {

    public class BuffTower : Tower {

        #region Fields
        [ClassHeader(typeof(BuffTower))]

        [MandatoryFields]
        [SerializeField] private BuffConfig buffConfig;
        #endregion

        #region Publics
        #endregion

        #region Internals
        protected override bool CanAttack() {
            return true;
        }

        protected override void PerformAttack() {
            var colliders = Physics2D.OverlapCircleAll(transform.position, attackRange, targetLayerMask);
            foreach (var collider in colliders) {
                var tower = collider.GetComponent<TowerEntity>()?.Tower;
                if (!tower) continue;
                if (tower is BuffTower) continue;

                tower.AddBuff(buffConfig.ToBuff(tower));
            }
        }

        protected override void PerformUpdate() {
            return;
        }
        #endregion

        #region Unity Methods
        protected override void Awake() {
            base.Awake();
            targetLayerMask = LayerMask.GetMask("Tower");
        }
        #endregion
    }
}
