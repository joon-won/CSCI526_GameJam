using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSCI526GameJam {

    public class FlameTower : Tower {

        #region Fields
        [MandatoryFields]
        [SerializeField] protected Transform muzzle;
        [SerializeField] private float flameSpeed = 3f;

        [ComputedFields]
        [SerializeField] private Enemy target;
        #endregion

        #region Publics
        #endregion

        #region Internals
        protected override bool CanAttack() {
            return target && Vector3.Distance(target.transform.position, transform.position) < attackRange;
        }

        protected override void PerformAttack() {
            // make a bullet
            var flame = ProjectilePooler.Instance.Get<Flame>();

            // calculate spread and fire
            flame.Setup(muzzle.position, attackDamage);
            flame.Fire(target.transform.position - entity.transform.position, flameSpeed, attackRange);
        }

        protected override void PerformUpdate() {
            // find target
            if (!target) {
                target = transform.position.FindClosestByDistance(attackRange, targetLayerMask);
            }
        }
        #endregion

        #region Unity Methods
        #endregion
    }
}
