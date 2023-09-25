using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CSCI526GameJam {

    public class GunTower : Tower {

        #region Fields
        [ClassHeader(typeof(GunTower))]

        [ComputedFields]
        [SerializeField] protected Enemy target;

        [SerializeField] protected float rotateSpeed = 60f;
        [SerializeField] protected float attackDegree = 30f;
        [SerializeField] protected float bulletSpeed = 30f;
        [SerializeField] protected float bulletSpread = 30f;
        //[SerializeField] protected float bulletImpact = 1f;
        #endregion

        #region Publics
        #endregion

        #region Internals
        protected override bool CanAttack() {
            if (!target) return false;

            var direction = target.transform.position - transform.position;
            var angleDiff = Vector3.Angle(entity.transform.up, direction);
            return angleDiff < attackDegree;
        }

        protected override void PerformAttack() {
            // make a bullet
            var bullet = ProjectilePooler.Instance.Get<Bullet>();

            // calculate spread and fire
            var randomDegree = Random.Range(-bulletSpread * 0.5f, bulletSpread * 0.5f);
            var rot = Quaternion.AngleAxis(randomDegree, Vector3.forward);
            bullet.Setup(transform.position, attackDamage);
            bullet.Fire(rot * entity.transform.up, bulletSpeed, attackRange);
        }

        protected override void PerformUpdate() {
            // Find a target. 
            if (!target) {
                target = transform.position.FindClosestByAngle(transform.up, attackRange, targetLayerMask);
            }
            if (!target) return;

            // Rotate to target. 
            entity.transform.FaceTo(target.transform, rotateSpeed * Time.deltaTime);
        }
        #endregion

        #region Unity Methods
        private void OnDrawGizmos() {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
        #endregion
    }
}
