using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CSCI526GameJam {

    public class TowerManager : MonoBehaviourSingleton<TowerManager>, IAssetDependent {

        #region Fields
        [ComputedFields]
        [SerializeField] private PlayerBase baseInstance;
        [SerializeField] private List<Tower> towerInstances = new();

        [SerializeField] private PlayerBase basePrefab;
        [SerializeField] private List<Tower> towerPrefabs;

        private GameObject towersHolder;
        private Dictionary<TowerConfig, Tower> configToPrefab = new();
        #endregion

        #region Publics
        public PlayerBase PlayerBase => baseInstance;
        public TowerConfig[] TowerConfigs => configToPrefab.Keys.ToArray();
        public int NumTowers => towerInstances.Count;

        /// <summary>
        /// Create an instance from the given tower config. 
        /// </summary>
        /// <param name="config">Config of the tower to create. </param>
        /// <param name="isActive">If the new instance is active. </param>
        /// <returns></returns>
        public Tower CreateTower(TowerConfig config, bool isActive = false) {
            if (!configToPrefab.ContainsKey(config)) {
                Debug.LogWarning($"{config.name} does not exist in the constructable list. ");
                return null;
            }

            var tower = Instantiate(configToPrefab[config], towersHolder.transform);
            tower.gameObject.SetActive(isActive);

            tower.OnBuild += () => towerInstances.Add(tower);
            tower.OnDemolish += () => towerInstances.Remove(tower);
            return tower;
        }

        /// <summary>
        /// Refund and demolish a tower. 
        /// </summary>
        /// <param name="tower">Tower to refund. </param>
        public void Refund(Tower tower) {
            if (tower == PlayerBase) return;

            Player.Instance.AddTower(tower.Config);
            tower.Demolish();
        }

        /// <summary>
        /// Generate the base. 
        /// </summary>
        public void GenerateBase() {
            var playerBase = Instantiate(basePrefab, towersHolder.transform);
            var mapSize = MapManager.Instance.MapSize;
            var centerSpot = MapManager.Instance.Get(mapSize / 2, mapSize / 2);
            playerBase.Build(centerSpot);
            baseInstance = playerBase;
        }

#if UNITY_EDITOR
        [EditorOnlyFields]
        [FolderPath, SerializeField]
        private string towerPrefabsPath;
        public void FindAssets() {
            towerPrefabs = Utility.FindRefsInFolder<Tower>(towerPrefabsPath, AssetType.Prefab);

            // Validate prefabs. 
            var isBaseFound = false;
            for (int i = 0; i < towerPrefabs.Count; i++) {
                var prefab = towerPrefabs[i];
                if (prefab is not CSCI526GameJam.PlayerBase) continue;

                if (isBaseFound) {
                    Debug.LogWarning($"There are duplicate prefabs of {typeof(PlayerBase)}. Fix it! ");
                }
                else {
                    isBaseFound = true;
                    basePrefab = prefab as PlayerBase;
                    towerPrefabs.Remove(prefab);
                }
            }

            if (!isBaseFound) {
                Debug.LogWarning($"{typeof(PlayerBase)} prefab is not found under {towerPrefabsPath}. Fix it! ");
            }
            else {
                Debug.Log($"Found {towerPrefabs.Count + 1} building prefabs under {towerPrefabsPath}. ");
            }
        }
#endif
        #endregion

        #region Internals
        #endregion

        #region Unity Methods
        protected override void Awake() {
            base.Awake();

            towersHolder = new GameObject("Building Instances");
            towersHolder.transform.SetParent(transform);

            foreach (var prefab in towerPrefabs) {
                configToPrefab.Add(prefab.Config, prefab);
            }
        }

        private void Update() {
            //if (UIManager.IsMouseOverUI) {
            //    if (hoveredBuilding) {
            //        hoveredBuilding.OnUnhover();
            //    }
            //}
            //else {
            //    var spot = MapManager.Instance.MouseSpot;
            //    if (spot) {
            //        if (hoveredBuilding) {
            //            hoveredBuilding.OnUnhover();
            //        }
            //        hoveredBuilding = spot.Building;
            //        if (hoveredBuilding) {
            //            hoveredBuilding.OnHover();
            //        }
            //    }
            //}
        }
        #endregion
    }
}