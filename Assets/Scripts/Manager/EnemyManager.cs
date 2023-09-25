using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CSCI526GameJam {
    public class EnemyManager : MonoBehaviourSingleton<EnemyManager> {

        #region Fields
        [ComputedFields]
        [SerializeField] private GameObject enemyHolder;
        [ShowInInspector] private HashSet<Enemy> enemyInstances = new();

        [SerializeField] private List<Enemy> enemyPrefabs;

#if UNITY_EDITOR
        [EditorOnlyFields]
        [FolderPath, SerializeField]
        private string enemyPrefabsPath;

        [Button("Find Enemy Prefabs", ButtonSizes.Large)]
        private void FindAssets() {
            enemyPrefabs = Utility.FindRefsInFolder<Enemy>(enemyPrefabsPath, AssetType.Prefab);
            Debug.Log($"Found {enemyPrefabs.Count} enemy prefabs under {enemyPrefabsPath}. ");
        }
#endif
        #endregion

        #region Publics
        public void StartEnemyWave() {
            var size = MapManager.Instance.MapSize;

            int row;
            int col;
            if (Random.value < 0.5f) {
                row = Random.Range(0, 2) == 0 ? 0 : size - 1;
                col = Random.Range(0, size);
            }
            else {
                row = Random.Range(0, size);
                col = Random.Range(0, 2) == 0 ? 0 : size - 1;
            }
            var spot = MapManager.Instance.Get(col, row);
            StartCoroutine(EnemyWaveRoutine(spot));
        }
        #endregion

        #region Internals
        private IEnumerator EnemyWaveRoutine(Spot spot) {
            var num = 10;
            var interval = 0.5f;
            var elapsed = interval;

            var path = new Path(spot, TowerManager.Instance.PlayerBase.Spot);
            while (num > 0) {
                elapsed += Time.deltaTime;
                if (elapsed > interval) {
                    var enemy = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Count)], spot.Position, Quaternion.identity, enemyHolder.transform);
                    enemy.Follow(path);
                    enemyInstances.Add(enemy);

                    elapsed = 0f;
                    num--;
                }
                yield return null;
            }
        }
        #endregion

        #region Unity Methods
        protected override void Awake() {
            base.Awake();
            enemyHolder = new GameObject("EnemyHolder");
            enemyHolder.transform.SetParent(transform);
        }
        #endregion
    }
}
