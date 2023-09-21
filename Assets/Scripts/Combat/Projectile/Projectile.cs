using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSCI526GameJam {

    public abstract class Projectile : MonoBehaviour {

        #region Fields
        [ClassHeader(typeof(Projectile))]

        [MandatoryFields]
        [SerializeField] protected float radius;

        [ComputedFields]
        [SerializeField] protected float damage;
        [SerializeField] protected float impact;
        [SerializeField] protected LayerMask targetLayerMask;
        #endregion

        #region Publics
        public float Radius => radius;

        /// <summary>
        /// Set up the basic properties. 
        /// </summary>
        public virtual void Setup(Vector3 position, float damage, float impact, LayerMask targetLayerMask) {
            transform.position = position;
            this.damage = damage;
            this.impact = impact;
            this.targetLayerMask = targetLayerMask;
        }

        #endregion

        #region Internals
        protected virtual void Dispose() {
            OnDispose();
            ProjectilePooler.Instance.Release(this);
        }

        protected virtual void OnDispose() {
            //damage = 0f;
        }
        #endregion

        #region Unity Methods
        protected virtual void OnDrawGizmos() {
            Gizmos.DrawWireSphere(transform.position, radius);
        }
        #endregion
    }
}
