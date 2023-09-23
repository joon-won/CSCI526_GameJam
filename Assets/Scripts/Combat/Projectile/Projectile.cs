using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSCI526GameJam {

    public abstract class Projectile : MonoBehaviour {

        #region Fields
        [ClassHeader(typeof(Projectile))]

        [ComputedFields]
        [SerializeField] protected float damage;
        //[SerializeField] protected float impact;
        #endregion

        #region Publics

        /// <summary>
        /// Set up the basic properties. 
        /// </summary>
        public virtual void Setup(Vector3 position, float damage) {
            transform.position = position;
            this.damage = damage;
        }

        #endregion

        #region Internals
        protected abstract void OnDisposed();

        protected void Dispose() {
            OnDisposed();
            ProjectilePooler.Instance.Release(this);
        }

        #endregion

        #region Unity Methods
        #endregion
    }
}
