using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSCI526GameJam {

    public class IceTower : Tower {

        #region Fields
        [ClassHeader(typeof(IceTower))]

        [MandatoryFields]
        [SerializeField] protected Transform muzzle;
        [SerializeField] private float iceSpeed = 3f;

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
            var ice = ProjectilePooler.Instance.Get<Ice>();

            // calculate spread and fire
            ice.Setup(muzzle.position, attackDamage);
            ice.Fire(target.transform.position - entity.transform.position, iceSpeed, attackRange);
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
