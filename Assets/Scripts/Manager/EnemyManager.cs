using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CSCI526GameJam {
    public class EnemyManager : MonoBehaviourSingleton<EnemyManager>, IAssetDependent {

        #region Fields
        [ComputedFields]
        [SerializeField] private int numSpawnSpots;
        [SerializeField] private List<Spot> spawnSpots = new();

        [SerializeField] private GameObject enemyHolder;
        [ShowInInspector] private HashSet<Enemy> enemyInstances = new();

        [SerializeField] private List<Enemy> enemyPrefabs;
        #endregion

        #region Publics
        public event Action OnEnemiesClear;

        public int NumEnemies => enemyInstances.Count;

#if UNITY_EDITOR
        [EditorOnlyFields]
        [FolderPath, SerializeField]
        private string enemyPrefabsPath;
        public void FindAssets() {
            enemyPrefabs = Utility.FindRefsInFolder<Enemy>(enemyPrefabsPath, AssetType.Prefab);
            Debug.Log($"Found {enemyPrefabs.Count} enemy prefabs under {enemyPrefabsPath}. ");
        }
#endif
        #endregion

        #region Internals
        private void GenerateSpawnSpots() {
            numSpawnSpots = 3; // TODO: Growth algorithm

            spawnSpots.Clear();
            var size = MapManager.Instance.MapSize;
            for (int i = 0; i < numSpawnSpots; i++) {
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
                if (spawnSpots.Contains(spot)) {
                    i--;
                    continue;
                }
                spawnSpots.Add(spot);
            }
        }

        private void GenerateEnemies() {
            foreach (var spot in spawnSpots) {
                StartCoroutine(EnemyWaveRoutine(spot));
            }
        }

        private IEnumerator EnemyWaveRoutine(Spot start) {
            var num = 10;
            var interval = 0.5f;
            var elapsed = interval;

            var path = new Path(start, TowerManager.Instance.PlayerBase.Spot);
            while (num > 0) {
                elapsed += Time.deltaTime;
                if (elapsed > interval) {
                    var enemy = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Count)], start.Position, Quaternion.identity, enemyHolder.transform);
                    enemy.Follow(path);
                    enemy.onDeath += () => OnEnemyDied(enemy);
                    enemyInstances.Add(enemy);

                    elapsed = 0f;
                    num--;
                }
                yield return null;
            }
        }

        private void OnEnemyDied(Enemy enemy) {
            enemyInstances.Remove(enemy);
            if (enemyInstances.Count == 0) {
                OnEnemiesClear?.Invoke();
            }
        }
        #endregion

        #region Unity Methods
        protected override void Awake() {
            base.Awake();
            enemyHolder = new GameObject("EnemyHolder");
            enemyHolder.transform.SetParent(transform);
        }

        private void OnEnable() {
            GameManager.Instance.OnBuyingStarted += GenerateSpawnSpots;
            GameManager.Instance.OnFightingStarted += GenerateEnemies;
        }

        private void OnDisable() {
            if (!GameManager.IsApplicationQuitting) {
                GameManager.Instance.OnBuyingStarted -= GenerateSpawnSpots;
                GameManager.Instance.OnFightingStarted -= GenerateEnemies;
            }
        }

        private void OnDrawGizmos() {
            foreach (var spot in spawnSpots) {
                Gizmos.color = Color.red;
                Gizmos.DrawCube(spot.Position, Vector3.one * Configs.CellSize);
            }
        }
        #endregion
    }
}
