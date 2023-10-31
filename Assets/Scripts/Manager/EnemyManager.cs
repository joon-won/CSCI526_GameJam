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
        private Dictionary<Spot, EnemyWave> spawnSpotToWave = new();

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

            spawnSpotToWave.Clear();
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
                if (spawnSpotToWave.ContainsKey(spot)) {
                    i--;
                    continue;
                }

                var enemyNum = 10;

                var wave = new EnemyWave(enemyPrefabs, enemyNum);
                spawnSpotToWave[spot] = wave;
            }
        }

        private void UpdateEnemyPaths() {
            foreach (var start in spawnSpotToWave.Keys.ToArray()) {
                var path = spawnSpotToWave[start].Path;
                if (path != null
                    && path.GroundSpots[0].Index == start.Index
                    && path.IsValid())
                    continue;

                spawnSpotToWave[start].GeneratePath(start);
            }

            indicatorInstances.ForEach(x => Destroy(x.gameObject));
            indicatorInstances.Clear();

            foreach (var wave in spawnSpotToWave.Values) {
                var groundIndicator = Instantiate(pathIndicatorPrefab, indicatorHolder.transform);
                groundIndicator.SetPath(wave.Path);
                indicatorInstances.Add(groundIndicator);

                if (wave.HasFlyingEnemies) {
                    var airIndicator = Instantiate(pathIndicatorPrefab, indicatorHolder.transform);
                    airIndicator.SetPath(wave.Path, true);
                    indicatorInstances.Add(airIndicator);
                }
            }
        }

        // NOTE: Temp fix for wave clear detection. 
        [SerializeField] private int totalNum;
        private void GenerateEnemies() {
            totalNum = 0;
            foreach (var wave in spawnSpotToWave.Values) {
                StartCoroutine(EnemyWaveRoutine(wave));
                totalNum += wave.Enemies.Count;
            }
        }

        private IEnumerator EnemyWaveRoutine(EnemyWave wave) {
            var interval = 0.5f;
            var elapsed = interval;

            while (wave.Enemies.Count > 0) {
                elapsed += Time.deltaTime;
                if (elapsed > interval) {
                    var enemy = wave.SpawnEnemy();
                    enemy.transform.SetParent(enemyHolder.transform);
                    enemy.Follow(wave.Path);
                    enemy.onDeath += () => OnEnemyDied(enemy);
                    enemyInstances.Add(enemy);

                    elapsed = 0f;
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
            foreach (var wave in spawnSpotToWave.Values) {
                Gizmos.color = Color.red;
                Gizmos.DrawCube(wave.Path.GroundSpots[0].Position, Vector3.one * Configs.CellSize);
            }
        }
        #endregion

        private class EnemyWave {
            private List<Enemy> enemies = new();
            private Path path;
            private bool hasFlyingEnemies = false;

            public List<Enemy> Enemies => enemies;
            public Path Path => path;
            public bool HasFlyingEnemies => hasFlyingEnemies;

            public EnemyWave(List<Enemy> enemyPrefabs, int num) {
                for (int i = 0; i < num; i++) {
                    var prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];
                    enemies.Add(prefab);

                    if (prefab.IsFlying) {
                        hasFlyingEnemies = true;
                    }
                }
            }

            public void GeneratePath(Spot startSpot) {
                path = new(startSpot, TowerManager.Instance.PlayerBase.Spot);
            }

            public Enemy SpawnEnemy() {
                if (path == null) return null;

                var enemyPrefab = enemies[^1];
                enemies.RemoveAt(enemies.Count);

                var enemy = Instantiate(enemyPrefab, path.GroundSpots[0].Position, Quaternion.identity);
                return enemy;
            }
        }
    }
}
