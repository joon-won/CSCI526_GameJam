using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSCI526GameJam {
    public class Bullet : Projectile {

        #region Fields
        [ClassHeader(typeof(Bullet))]

        [ComputedFields]
        [SerializeField] private float speed;
        [SerializeField] private float elapsed = 0f;
        [SerializeField] private float duration;

        [SerializeField] private bool isPiercing = false;
        [SerializeField] private int piercingNum = 0;
        //private HashSet<Enemy> hits = new HashSet<Enemy>();
        #endregion

        #region Publics

        /// <summary>
        /// Fire the bullet. 
        /// </summary>
        /// <param name="direction">Shooting direction. </param>
        /// <param name="speed">Bullet speed. </param>
        /// <param name="range">Max shooting distance. </param>
        public void Fire(Vector3 direction, float speed, float range) {
            transform.FaceTo(direction.normalized);
            this.speed = speed;
            duration = range / speed;
            elapsed = 0f;
        }

        /// <summary>
        /// Set this bullet's piercing property. 
        /// </summary>
        /// <param name="piercingNum">Max number of target to pierce. </param>
        public void SetPiercing(int piercingNum) {
            isPiercing = true;
            this.piercingNum = piercingNum;
        }
        #endregion

        #region Internals
        protected override void OnDispose() {
            base.OnDispose();
            speed = 0f;
            elapsed = 0f;
            duration = 0f;
            //hits.Clear();
        }
        #endregion

        #region Unity Methods
        private void Update() {
            if (elapsed > duration) {
                Dispose();
                return;
            }

            elapsed += Time.deltaTime;

            transform.position += transform.up * speed * Time.deltaTime;

            //if (!isPiercing) {
            //    var target = transform.position.FindClosestByDistance(radius, targetLayerMask);
            //    if (!target) return;

            //    target.TakeDamage(damage);
            //    target.HitBack(transform.position, impact);
            //    Dispose();
            //}
            //else {
            //    var targets = transform.position.FindAllByDistance(radius, targetLayerMask);
            //    foreach (var target in targets) {
            //        if (hits.Contains(target)) continue;

            //        hits.Add(target);
            //        target.TakeDamage(damage);
            //        target.HitBack(transform.position, impact);

            //        // if piercingNum is set to negative, no limit
            //        if (piercingNum < 0) continue;

            //        // else, decrease num
            //        piercingNum--;
            //        if (piercingNum >= 0) continue;

            //        Dispose();
            //        return;
            //    }
            //}
        }
        #endregion

    }
}
