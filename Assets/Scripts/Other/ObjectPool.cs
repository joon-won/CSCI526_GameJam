using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSCI526GameJam {

    /// <summary>
    /// Generic object pool. Support instances tracking. 
    /// </summary>

    [Serializable]
    public class ObjectPool<T> where T : class {

        #region Fields
        [SerializeField] private UnityEngine.Pool.ObjectPool<T> pool;
        [SerializeField] private List<T> instances;
        #endregion

        #region Publics
        public ObjectPool(
            Func<T> createFunc,
            Action<T> actionOnGet = null,
            Action<T> actionOnRelease = null,
            Action<T> actionOnDestroy = null,
            bool collectionCheck = true,
            int defaultCapacity = 10,
            int maxSize = 10000) {

            actionOnDestroy += obj => instances.Remove(obj);
            pool = new UnityEngine.Pool.ObjectPool<T>(
                createFunc,
                actionOnGet,
                actionOnRelease,
                actionOnDestroy,
                collectionCheck,
                defaultCapacity,
                maxSize);
            instances = new List<T>();

            for (int i = 0; i < defaultCapacity; i++) {
                var obj = createFunc();
                Release(obj);
            }
        }

        public int Count => instances.Count;
        public List<T> Instances => instances;

        /// <summary>
        /// Get an object from the pool and track it. 
        /// </summary>
        /// <returns>Pooled object. </returns>
        public T Get() {
            var obj = pool.Get();
            instances.Add(obj);
            return obj;
        }

        /// <summary>
        /// Return an object to the pool. 
        /// </summary>
        /// <param name="obj">Object to return. </param>
        public void Release(T obj) {
            pool.Release(obj);
            instances.Remove(obj);
        }

        /// <summary>
        /// Return all objects to the pool. 
        /// </summary>
        public void ReleaseAll() {
            foreach (var instance in instances) {
                pool.Release(instance);
            }
            instances.Clear();
        }

        /// <summary>
        /// Clear the pool. actionOnDestroy will be called for each object. 
        /// </summary>
        public void Clear() {
            pool.Clear();
            instances.Clear();
        }
        #endregion

        #region Internals
        #endregion
    }
}
