using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CSCI526GameJam {

    public class TowerManager : MonoBehaviourSingleton<TowerManager> {

        public enum Mode {
            None,

            Build,
            Demolish,
        }

        #region Fields
        [ComputedFields]
        [SerializeField] private Mode mode;

        [SerializeField] private Tower hoveredTower;
        [SerializeField] private Tower selectedTower;

        [SerializeField] private List<Tower> towerInstances = new();

        [SerializeField] private PlayerBase basePrefab;
        [SerializeField] private List<Tower> towerPrefabs;

        private TowerPlacer placer;
        private GameObject towersHolder;
        private Dictionary<TowerConfig, Tower> configToPrefab = new();

        private Action changeModeToNone;
        private Action changeModeToDemolish;


#if UNITY_EDITOR
        [EditorOnlyFields]
        [FolderPath, SerializeField]
        private string towerPrefabsPath;

        [Button("Find Tower Prefabs", ButtonSizes.Large)]
        private void FindAssets() {
            towerPrefabs = Utility.FindRefsInFolder<Tower>(towerPrefabsPath, AssetType.Prefab);
            Debug.Log($"Found {towerPrefabs.Count} tower prefabs under {towerPrefabsPath}. ");

            var foundBase = Utility.FindRefsInFolder<PlayerBase>(towerPrefabsPath, AssetType.Prefab);
            if (foundBase.Count == 0) {
                Debug.LogWarning($"Player base prefab is not found under {towerPrefabsPath}. ");
                return;
            }
            basePrefab = foundBase[0];
            Debug.Log($"Player base prefab is found under {towerPrefabsPath}. ");
        }
#endif
        #endregion

        #region Publics
        public TowerPlacer Placer => placer;
        //public Core Core { get; private set; }

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
            towerInstances.Add(tower);
            tower.OnDemolish += () => towerInstances.Remove(tower);
            return tower;
        }

        /// <summary>
        /// Preview a tower. 
        /// </summary>
        /// <param name="config">Config of the tower to preview. </param>
        public void Preview(TowerConfig config) {
            if (!placer.TowerConfig || placer.TowerConfig != config) {
                placer.StartPreview(config);
                ChangeMode(Mode.Build);
            }
            else {
                placer.CancelPreview();
            }
        }

        /// <summary>
        /// Refund and demolish a tower. 
        /// </summary>
        /// <param name="tower">Tower to refund. </param>
        public void Refund(Tower tower) {
            //if (building == Core) return;

            // TODO: refund

            tower.Demolish();
        }

        /// <summary>
        /// Change current mode. 
        /// </summary>
        /// <param name="mode">Target mode. </param>
        public void ChangeMode(Mode mode) {
            if (this.mode == mode) return;

            this.mode = mode;
            switch (mode) {
                case Mode.None:
                    placer.CancelPreview();
                    //Player.Instance.Inventory.PutbackHeld();
                    //MapManager.Instance.ConstructionTilemap.Hide();
                    break;

                case Mode.Build:
                    //MapManager.Instance.ConstructionTilemap.Show();
                    break;

                case Mode.Demolish:
                    //MapManager.Instance.ConstructionTilemap.Show();
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Perform current mode action. 
        /// </summary>
        public void PerformMode() {
            var building = hoveredTower;
            switch (mode) {
                case Mode.None:
                    if (selectedTower) {
                        //selectedBuilding.OnUnselect();
                        selectedTower = null;
                    }

                    if (building) {
                        // inspect
                        selectedTower = building;
                    }
                    else {

                    }
                    break;

                case Mode.Build:
                    placer.Place();
                    break;

                case Mode.Demolish:
                    if (!building) return;

                    Refund(building);
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Generate the base. 
        /// </summary>
        public void GenerateBase() {
            var playerBase = Instantiate(basePrefab, towersHolder.transform);
            playerBase.transform.position = MapManager.Instance.MapCenter;
        }
        #endregion

        #region Internals
        #endregion

        #region Unity Methods
        protected override void Awake() {
            base.Awake();

            placer = GetComponentInChildren<TowerPlacer>();

            towersHolder = new GameObject("Building Instances");
            towersHolder.transform.SetParent(transform);

            foreach (var prefab in towerPrefabs) {
                configToPrefab.Add(prefab.Config, prefab);
            }

            changeModeToNone = () => ChangeMode(Mode.None);
            changeModeToDemolish = () => ChangeMode(Mode.Demolish);
        }

        private void OnEnable() {
            InputManager.Instance.OnMouseLeftDown += PerformMode;
            InputManager.Instance.OnMouseRightDown += changeModeToNone;

            InputManager.Instance.OnDemolishKeyDown += changeModeToDemolish;
        }

        private void OnDisable() {
            if (!GameManager.IsApplicationQuitting) {
                InputManager.Instance.OnMouseLeftDown -= PerformMode;
                InputManager.Instance.OnMouseRightDown -= changeModeToNone;

                InputManager.Instance.OnDemolishKeyDown -= changeModeToDemolish;
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
            hoveredTower = MapManager.Instance.MouseSpot.Tower;
        }
        #endregion
    }
}