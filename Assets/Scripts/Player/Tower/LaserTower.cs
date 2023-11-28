using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSCI526GameJam {
    public class LaserTower : Tower {

        #region Fields
        [MandatoryFields]
        [SerializeField] private Transform muzzle;
        [SerializeField] private LineRenderer laserShooter;

        [ComputedFields]
        [SerializeField] private Enemy target;
        #endregion

        #region Publics
        #endregion

        #region Internals
        protected override bool CanAttack() {
            return target;
        }

        protected override void PerformAttack() {
            target.TakeDamage(attackDamage);
        }

        protected override void PerformUpdate() {
            if (!target) {
                laserShooter.enabled = false;
                // find target
                target = transform.position.FindClosestByDistance(attackRange, targetLayerMask);
            }
            else {
                laserShooter.enabled = true;
                laserShooter.SetPosition(0, muzzle.position);
                laserShooter.SetPosition(1, target.transform.position);
            }
        }
        #endregion

        #region Unity Methods
        protected override void Awake() {
            base.Awake();
            laserShooter.positionCount = 2;
        }
        #endregion
    }
}
