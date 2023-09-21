using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSCI526GameJam {

    /// <summary>
    /// Manager that pools projectiles. 
    /// </summary>
    
    public class ProjectilePooler : MonoBehaviourSingleton<ProjectilePooler> {

        [Serializable]
        private class PoolData {
            [MandatoryFields]
            public Projectile prefab;
            public int defualtAmount;

            [ComputedFields]
            public ObjectPool<Projectile> pool;
        }

        #region Fields
        [MandatoryFields]
        [SerializeField] private PoolData[] poolData;

        [ComputedFields]
        private Dictionary<string, PoolData> keyToPoolData = new();
        #endregion

        #region Publics
        /// <summary>
        /// Get a pooled projectile. 
        /// </summary>
        /// <typeparam name="T">Projectile type. </typeparam>
        /// <returns>Projectile from pool. </returns>
        public T Get<T>() where T : Projectile {
            var key = ToKey(typeof(T));
            if (!keyToPoolData.ContainsKey(key)) {
                Debug.LogWarning($"Projectile of key {key} is not pooled. ");
                return null;
            }
            return keyToPoolData[key].pool.Get() as T;
        }

        /// <summary>
        /// Release a projectile. 
        /// </summary>
        /// <param name="projectile">Projectile to release. </param>
        public void Release(Projectile projectile) {
            var key = ToKey(projectile.GetType());
            if (!keyToPoolData.ContainsKey(key)) {
                Debug.LogWarning($"Projectile of key {key} is not pooled. ");
                return;
            }
            keyToPoolData[key].pool.Release(projectile);
        }
        #endregion

        #region Internals
        private string ToKey(Type type) {
            return type.Name;
        }
        #endregion

        #region Unity Methods
        protected override void Awake() {
            base.Awake();

            foreach (var data in poolData) {
                var holder = new GameObject(data.prefab.name + " Holder");
                holder.transform.SetParent(transform);

                data.pool = new ObjectPool<Projectile>(
                    () => {
                        return Instantiate(data.prefab, holder.transform);
                    },
                    obj => {
                        obj.gameObject.SetActive(true);
                    },
                    obj => {
                        obj.gameObject.SetActive(false);
                    },
                    obj => {
                        Destroy(obj);
                    }, true, data.defualtAmount);

                var key = ToKey(data.prefab.GetType());
                keyToPoolData.TryGetValue(key, out var addedData);
                if (addedData != null) {
                    Debug.LogWarning($"Projectile {data.prefab.name} has the same key of {key} with projectile {addedData.prefab.name}. ");
                    return;
                }
                keyToPoolData.Add(key, data);
            }
        }
        #endregion
    }
}
