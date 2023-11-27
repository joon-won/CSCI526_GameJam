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
        [ShowInInspector]
        private Dictionary<Spot, EnemyWave> spawnSpotToWave = new();

        [SerializeField] private GameObject enemyHolder;
        [ShowInInspector] private HashSet<Enemy> enemyInstances = new();

        [SerializeField] private GameObject indicatorHolder;
        [SerializeField] private List<EnemyPathIndicator> indicatorInstances = new();

        [SerializeField] private List<Enemy> enemyPrefabs = new();
        [ShowInInspector] private Dictionary<EnemyConfig, Enemy> enemyConfigToPrefab = new();
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

        public bool CanEnemiesReachBase(params Spot[] blocks) {
            return spawnSpotToWave.Keys.All(
                spot => {
                    var path = new Path(spot, TowerManager.Instance.PlayerBase.Spot, blocks);
                    return path.GroundSpots.Count > 0;
                });
        }

        [ContextMenu("Kill All")]
        public void Debug_killAll() {
            enemyInstances
                .ToArray()
                .ForEach(x => x.TakeDamage(float.PositiveInfinity));
        }
        #endregion

        #region Internals
        private void UpdateSpawnSpots() {
            var levelInfo = GameManager.Instance.GetCurrentLevelInfo();
            var numPaths = levelInfo.NumWaves;

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

                var waveInfo = levelInfo.WaveInfos[i];
                var wave = new EnemyWave(waveInfo);
                spawnSpotToWave[spot] = wave;
            }
        }

        private void UpdateEnemyPaths() {
            foreach (var start in spawnSpotToWave.Keys.ToArray()) {
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

        private void GenerateEnemies() {
            foreach (var wave in spawnSpotToWave.Values) {
                StartCoroutine(EnemyWaveRoutine(wave));
            }
        }

        private IEnumerator EnemyWaveRoutine(EnemyWave wave) {
            var interval = 0.5f;
            var elapsed = interval;

            while (wave.Num > 0) {
                elapsed += Time.deltaTime;
                yield return null;
                if (elapsed > interval) {
                    var config = wave.ExtractEnemy();
                    if (!config) break;

                    var enemy = Instantiate(enemyConfigToPrefab[config], wave.Path.GroundSpots[0].Position, Quaternion.identity);
                    enemy.transform.SetParent(enemyHolder.transform);
                    enemy.Follow(wave.Path);
                    enemy.onDeath += () => OnEnemyDied(enemy);
                    enemyInstances.Add(enemy);

                    elapsed = 0f;
                }
            }
        }

        private void OnEnemyDied(Enemy enemy) {
            enemyInstances.Remove(enemy);
            if (spawnSpotToWave.Values.All(x => x.Num == 0)
                && enemyInstances.Count == 0) {
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

            enemyConfigToPrefab = enemyPrefabs.ToDictionary(x => x.Config, x => x);

            enemyHolder = new GameObject("Enemy Instances");
            enemyHolder.transform.SetParent(transform);

            indicatorHolder = new GameObject("Indicator Instances");
            indicatorHolder.transform.SetParent(transform);

            GameManager.Instance.OnPreparationStarted += OnPreparationStarted;
            GameManager.Instance.OnCombatStarted += GenerateEnemies;
            GameManager.Instance.OnCurrentSceneExiting += () => {
                GameManager.Instance.OnPreparationStarted -= OnPreparationStarted;
                GameManager.Instance.OnCombatStarted -= GenerateEnemies;
            };
        }

        private void Start() {
            Player.Instance.OnTowerPlaced += _ => UpdateEnemyPaths();
            Player.Instance.OnTowerDemolished += _ => UpdateEnemyPaths();
        }

        private void OnDrawGizmos() {
            foreach (var wave in spawnSpotToWave.Values) {
                Gizmos.color = Color.red;
                Gizmos.DrawCube(wave.Path.GroundSpots[0].Position, Vector3.one * Configs.CellSize);
            }
        }
        #endregion

        private class EnemyWave {
            private List<EnemyConfig> enemies = new();
            private Path path;
            private bool hasFlyingEnemies = false;

            public int Num => enemies.Count;
            public Path Path => path;
            public bool HasFlyingEnemies => hasFlyingEnemies;

            public EnemyWave(WaveInfo waveInfo) {
                enemies = waveInfo.Enemies.Reverse().ToList();

                hasFlyingEnemies = enemies.Any(x => x.IsFlying);
            }

            public void GeneratePath(Spot startSpot) {
                var newPath = new Path(startSpot, TowerManager.Instance.PlayerBase.Spot);
                if (path == null
                    || !path.IsValid()
                    || newPath.GroundSpots.Count < path.GroundSpots.Count
                    || newPath.AirSpots.Count < path.AirSpots.Count) {
                    path = newPath;
                }
            }

            public EnemyConfig ExtractEnemy() {
                if (path == null) return null;
                if (enemies.Count == 0) return null;

                var enemy = enemies[^1];
                enemies.RemoveAt(enemies.Count - 1);
                return enemy;
            }
        }
    }
}
