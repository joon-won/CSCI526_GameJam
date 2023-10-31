using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Random = UnityEngine.Random;

namespace CSCI526GameJam {
    public class EnemyManager : MonoBehaviourSingleton<EnemyManager>, IAssetDependent {

        #region Fields
        [MandatoryFields]
        [SerializeField] private EnemyPathIndicator pathIndicatorPrefab;

        [ComputedFields]
        [SerializeField] private int numPaths;
        [ShowInInspector]
        private Dictionary<Spot, Path> spawnSpotToPath = new();

        [SerializeField] private GameObject enemyHolder;
        [ShowInInspector] private HashSet<Enemy> enemyInstances = new();

        [SerializeField] private GameObject indicatorHolder;
        [SerializeField] private List<EnemyPathIndicator> indicatorInstances = new();

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
        private void UpdateSpawnSpots() {
            numPaths = 3; // TODO: Growth algorithm

            spawnSpotToPath.Clear();
            var size = MapManager.Instance.MapSize;
            for (int i = 0; i < numPaths; i++) {
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
                if (spawnSpotToPath.ContainsKey(spot)) {
                    i--;
                    continue;
                }
                spawnSpotToPath[spot] = null;
            }
        }

        private void UpdateEnemyPaths() {
            foreach (var start in spawnSpotToPath.Keys.ToArray()) {
                var path = spawnSpotToPath[start];
                if (path != null
                    && path.Spots[0].Index == start.Index
                    && path.IsValid())
                    continue;

                spawnSpotToPath[start] = new(start, TowerManager.Instance.PlayerBase.Spot);
            }

            indicatorInstances.ForEach(x => Destroy(x.gameObject));
            indicatorInstances.Clear();

            spawnSpotToPath.Values.ToList().ForEach(path => {
                var indicator = Instantiate(pathIndicatorPrefab, indicatorHolder.transform);
                indicator.SetPath(path);
                indicatorInstances.Add(indicator);
            });
        }

        // NOTE: Temp fix for wave clear detection. 
        [SerializeField] private int totalNum;
        private void GenerateEnemies() {
            totalNum = 0;
            foreach (var spot in spawnSpotToPath.Values) {
                var num = 10;
                var routine = StartCoroutine(EnemyWaveRoutine(spot, num));
                totalNum += num;
            }
        }

        private IEnumerator EnemyWaveRoutine(Path path, int num) {
            var interval = 0.5f;
            var elapsed = interval;

            while (num > 0) {
                elapsed += Time.deltaTime;
                if (elapsed > interval) {
                    var enemy = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Count)], path.Spots[0].Position, Quaternion.identity, enemyHolder.transform);
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
            totalNum--;
            enemyInstances.Remove(enemy);
            if (totalNum == 0) {
                OnEnemiesClear?.Invoke();
            }
        }

        private void OnPreparationStarted() {
            UpdateSpawnSpots();
            UpdateEnemyPaths();
        }
        #endregion

        #region Unity Methods
        protected override void Awake() {
            base.Awake();
            enemyHolder = new GameObject("Enemy Instances");
            enemyHolder.transform.SetParent(transform);

            indicatorHolder = new GameObject("Indicator Instances");
            indicatorHolder.transform.SetParent(transform);
        }

        private void Start() {
            Player.Instance.OnTowerPlaced += config => UpdateEnemyPaths();
        }

        private void OnEnable() {
            GameManager.Instance.OnPreparationStarted += OnPreparationStarted;
            GameManager.Instance.OnCombatStarted += GenerateEnemies;
        }

        private void OnDisable() {
            if (!GameManager.IsApplicationQuitting) {
                GameManager.Instance.OnPreparationStarted -= OnPreparationStarted;
                GameManager.Instance.OnCombatStarted -= GenerateEnemies;
            }
        }

        private void OnDrawGizmos() {
            foreach (var path in spawnSpotToPath.Values) {
                Gizmos.color = Color.red;
                Gizmos.DrawCube(path.Spots[0].Position, Vector3.one * Configs.CellSize);
            }
        }
        #endregion
    }
}
