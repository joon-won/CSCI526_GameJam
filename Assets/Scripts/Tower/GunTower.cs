using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CSCI526GameJam {

    public class GunTower : Defense {

        #region Fields
        [ClassHeader(typeof(GunTower))]

        [MandatoryFields]
        [SerializeField] protected float rotateSpeed = 60f;
        [SerializeField] protected float attackDegree = 30f;
        [SerializeField] protected float bulletSpeed = 30f;
        [SerializeField] protected float bulletSpread = 30f;
        [SerializeField] protected float bulletImpact = 1f;
        [SerializeField] protected ProductConfig ammo;

        [ComputedFields]
        [SerializeField] protected Enemy target;
        #endregion

        #region Publics
        #endregion

        #region Internals
        protected override bool CanAttack() {
            return target && Vector3.Angle(entity.transform.up, target.transform.position - transform.position) < attackDegree;
        }

        protected override void PerformAttack() {
            // make a bullet
            var bullet = GetAmmo<Bullet>(ammo);
            if (!bullet) return;

            // calculate spread and fire
            var randomDegree = Random.Range(-bulletSpread * 0.5f, bulletSpread * 0.5f);
            var rot = Quaternion.AngleAxis(randomDegree, Vector3.forward);
            bullet.Setup(transform.position, ComputeFinalDamage(), bulletImpact, targetLayerMask);
            bullet.Fire(rot * entity.transform.up, bulletSpeed, attackRange);
        }

        protected override void PerformUpdate() {
            // find target
            if (!target) {
                target = transform.position.FindClosestByAngle(transform.up, attackRange, targetLayerMask) as Enemy;
            }
            if (!target) return;

            // rotate to target
            entity.transform.FaceTo(target.transform, rotateSpeed * Time.deltaTime);
        }
        #endregion

        #region Unity Methods
        #endregion
    }
}
