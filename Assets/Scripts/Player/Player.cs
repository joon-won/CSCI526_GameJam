using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Sirenix.OdinInspector;

namespace CSCI526GameJam {
    public class Player : MonoBehaviourSingleton<Player> {

        private enum Mode {
            None,

            Build,
            Demolish,
        }

        #region Fields
        [ComputedFields]
        [SerializeField] private bool isLocked;
        [SerializeField] private Mode mode;
        [SerializeField] private Tower hoveredTower;
        [SerializeField] private Tower selectedTower;

        [SerializeField] private int gold;
        [SerializeField] private int towerPoolSize = 3;
        [ShowInInspector] private Dictionary<int, Tower> indexToTower = new(); // Tower pool

        private TowerPlacer placer;

        private Action changeModeToNone;
        private Action changeModeToDemolish;
        #endregion

        #region Publics
        public event Action<int> OnGoldChanged;
        public event Action OnTowerPoolChanged;

        public int Gold => gold;
        public int PoolSize => indexToTower.Count;

        /// <summary>
        /// Check if gold is enough to pay a price. 
        /// </summary>
        /// <param name="value">Price value. </param>
        /// <returns></returns>
        public bool CanPay(int value) {
            return gold >= value;
        }

        /// <summary>
        /// Try to pay a price. 
        /// </summary>
        /// <param name="value">Price value. </param>
        /// <returns>True if the payment is successfully, else false. </returns>
        public bool TryPay(int value) {
            if (!CanPay(value)) return false;

            gold -= value;
            OnGoldChanged?.Invoke(gold);
            return true;
        }

        /// <summary>
        /// Add gold. 
        /// </summary>
        /// <param name="value">Value to be added. </param>
        public void AddGold(int value) {
            gold += value;
            OnGoldChanged?.Invoke(gold);
        }

        /// <summary>
        /// Get a tower config from the tower pool by index. 
        /// </summary>
        /// <param name="index">Index of the tower in pool. </param>
        /// <returns></returns>
        public TowerConfig GetTowerConfig(int index) {
            return indexToTower[index].Config;
        }

        /// <summary>
        /// Pick and preview a tower from the tower pool by index. 
        /// </summary>
        /// <param name="index">Index of the tower in pool. </param>
        public void PickTower(int index) {
            var tower = indexToTower[index];
            if (!placer.IsPreviewing) {
                placer.StartPreview(tower);
                ChangeMode(Mode.Build);
            }
            else {
                ChangeMode(Mode.None);
            }
        }
        #endregion

        #region Internals
        private void FillTowerPool(int index) {
            var configs = TowerManager.Instance.TowerConfigs;
            var config = configs[Random.Range(0, configs.Length)]; // TODO: Random algorithm
            var tower = TowerManager.Instance.CreateTower(config);
            tower.OnBuild += () => FillTowerPool(index);
            indexToTower[index] = tower;
            OnTowerPoolChanged?.Invoke();
        }

        private void ChangeMode(Mode mode) {
            if (isLocked) return;
            if (this.mode == mode) return;

            this.mode = mode;
            switch (mode) {
                case Mode.None:
                    placer.CancelPreview();
                    break;

                case Mode.Build:
                    break;

                case Mode.Demolish:
                    break;

                default:
                    break;
            }
        }

        private void PerformMode() {
            if (isLocked) return;

            switch (mode) {
                case Mode.None:
                    if (selectedTower) {
                        //selectedBuilding.OnUnselect();
                        selectedTower = null;
                    }

                    if (hoveredTower) {
                        // inspect
                        selectedTower = hoveredTower;
                    }
                    else {

                    }
                    break;

                case Mode.Build:
                    if (!placer.TryPlace()) return;

                    ChangeMode(Mode.None);
                    break;

                case Mode.Demolish:
                    if (!hoveredTower) return;

                    TowerManager.Instance.Refund(hoveredTower);
                    break;

                default:
                    break;
            }
        }

        private void Lock() {
            ChangeMode(Mode.None);
            isLocked = true;
        }

        private void Unlock() {
            isLocked = false;
            ChangeMode(Mode.None);
        }
        #endregion

        #region Unity Methods
        protected override void Awake() {
            base.Awake();

            isLocked = false;
            changeModeToNone = () => ChangeMode(Mode.None);
            changeModeToDemolish = () => ChangeMode(Mode.Demolish);

            placer = GetComponentInChildren<TowerPlacer>();

            for (int i = 0; i < towerPoolSize; i++) {
                FillTowerPool(i);
            }

            // Debug
            gold = 1000000;
            Debug.Log($"<Debug> Gold is set to {gold}. ");
        }

        private void OnEnable() {
            GameManager.Instance.OnPreparationStarted += Unlock;
            GameManager.Instance.OnCombatStarted += Lock;
            InputManager.Instance.OnMouseLeftDown += PerformMode;
            InputManager.Instance.OnMouseRightDown += changeModeToNone;
            InputManager.Instance.OnDemolishKeyDown += changeModeToDemolish;
        }

        private void OnDisable() {
            if (!GameManager.IsApplicationQuitting) {
                GameManager.Instance.OnPreparationStarted -= Unlock;
                GameManager.Instance.OnCombatStarted -= Lock;
                InputManager.Instance.OnMouseLeftDown -= PerformMode;
                InputManager.Instance.OnMouseRightDown -= changeModeToNone;
                InputManager.Instance.OnDemolishKeyDown -= changeModeToDemolish;
            }
        }
        // Set the json output to get the in game values.
        private void Update() {
            hoveredTower = MapManager.Instance.MouseSpot.Tower;
        }
        #endregion
    }
}
