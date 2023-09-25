using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSCI526GameJam {

    public abstract class Projectile : MonoBehaviour {

        #region Fields
        [ClassHeader(typeof(Projectile))]

        [ComputedFields]
        [SerializeField] private bool isDisposed;
        [SerializeField] protected float damage;
        //[SerializeField] protected float impact;
        #endregion

        #region Publics
        /// <summary>
        /// Set up the basic properties. 
        /// </summary>
        public virtual void Setup(Vector3 position, float damage) {
            isDisposed = false;
            transform.position = position;
            this.damage = damage;
        }

        #endregion

        #region Internals
        protected abstract void OnDisposed();

        protected void Dispose() {
            if (isDisposed) return;

            isDisposed = true;
            OnDisposed();
            ProjectilePooler.Instance.Release(this);
        }

        #endregion

        #region Unity Methods
        #endregion
    }
}
