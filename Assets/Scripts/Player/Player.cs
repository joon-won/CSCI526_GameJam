using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Sirenix.OdinInspector;
using UnityEngine.EventSystems;

namespace CSCI526GameJam {
    public class Player : MonoBehaviourSingleton<Player> {

        private enum Mode {
            None,

            Build,
            Demolish,
        }

        #region Fields

        public static bool IsMouseOverUI { get; private set; }

        [MandatoryFields]
        [SerializeField] private int initialGold;

        [SerializeField] private Transform rangeIndicator;
        [SerializeField] private SpriteRenderer spotIndicator;
        [SerializeField] private Sprite highlightSprite;
        [SerializeField] private Sprite demolishSprite;

        [ComputedFields]
        [SerializeField] private bool isLocked;
        [SerializeField] private Mode mode;
        [SerializeField] private Tower hoveredTower;
        [SerializeField] private Tower selectedTower;

        [SerializeField] private int gold;
        [ShowInInspector] private Dictionary<TowerConfig, int> towerConfigToNum = new();

        private TowerPlacer placer;

        private Action changeModeToNone;
        private Action changeModeToDemolish;
        #endregion

        #region Publics
        public event Action<int> OnGoldChanged;
        public event Action<TowerConfig, int> OnTowerNumChanged;
        public event Action<TowerConfig> OnTowerPlaced;
        public event Action<TowerConfig> OnTowerDemolished;

        public int Gold => gold;

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
        /// Get num of a tower. 
        /// </summary>
        /// <param name="config">Config of the tower. </param>
        /// <returns>Num of the tower. </returns>
        public int GetTowerNum(TowerConfig config) {
            if (towerConfigToNum.TryGetValue(config, out var num)) {
                return num;
            }
            return 0;
        }

        /// <summary>
        /// Pick and preview a tower from the tower pool by index. 
        /// </summary>
        /// <param name="index">Index of the tower in pool. </param>
        public void PickTower(TowerConfig config) {
            if (GetTowerNum(config) <= 0) return;

            var tower = TowerManager.Instance.CreateTower(config);
            if (!placer.IsPreviewing) {
                placer.StartPreview(tower);
                ChangeMode(Mode.Build);
            }
            else {
                ChangeMode(Mode.None);
            }

            OnTowerNumChanged?.Invoke(config, towerConfigToNum[config]);
        }

        /// <summary>
        /// Add tower num. 
        /// </summary>
        /// <param name="config">Config of the tower to add. </param>
        public void AddTower(TowerConfig config, int num = 1) {
            if (num <= 0) {
                Debug.LogWarning($"Trying to add a non-positive num {num}");
                return;
            }

            if (towerConfigToNum.ContainsKey(config)) {
                towerConfigToNum[config] += num;
                return;
            }
            towerConfigToNum[config] = num;
        }

        public void LoadTutorial(TutorialInfo info) {
            gold = info.Gold;
            OnGoldChanged?.Invoke(gold);
        }

        public void EndTutorial() {
            gold = initialGold;
            towerConfigToNum.Clear();
            OnGoldChanged?.Invoke(gold);
        }
        #endregion

        #region Internals
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
            if (IsMouseOverUI) return;
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
                    OnTowerDemolished?.Invoke(hoveredTower.Config);
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

        private void ConsumeOnPlaced(TowerConfig config) {
            if (!towerConfigToNum.ContainsKey(config)) {
                Debug.LogWarning($"{config} is not in the dict. ");
                return;
            }
            towerConfigToNum[config]--;
        }

        [ContextMenu("<Debug> Add all towers")]
        private void Debug_addAllTowers() {
            foreach (var config in TowerManager.Instance.TowerConfigs) {
                towerConfigToNum[config] = 100;
            }
        }
        #endregion

        #region Unity Methods
        protected override void Awake() {
            base.Awake();

            isLocked = false;
            changeModeToNone = () => ChangeMode(Mode.None);
            changeModeToDemolish = () => ChangeMode(Mode.Demolish);

            placer = GetComponentInChildren<TowerPlacer>();
            placer.OnPlaced += ConsumeOnPlaced;
            placer.OnPlaced += config => OnTowerPlaced?.Invoke(config);

            GameManager.Instance.OnPreparationStarted += Unlock;
            GameManager.Instance.OnCombatStarted += Lock;
            InputManager.Instance.OnMouseLeftDown += PerformMode;
            InputManager.Instance.OnMouseRightDown += changeModeToNone;
            InputManager.Instance.OnDemolishKeyDown += changeModeToDemolish;

            GameManager.Instance.OnCurrentSceneExiting += () => {
                GameManager.Instance.OnPreparationStarted -= Unlock;
                GameManager.Instance.OnCombatStarted -= Lock;
                InputManager.Instance.OnMouseLeftDown -= PerformMode;
                InputManager.Instance.OnMouseRightDown -= changeModeToNone;
                InputManager.Instance.OnDemolishKeyDown -= changeModeToDemolish;
            };
        }

        // Set the json output to get the in game values.
        private void Update() {
            IsMouseOverUI = EventSystem.current.IsPointerOverGameObject();

            hoveredTower = MapManager.Instance.MouseSpot.Tower;
            if (hoveredTower && hoveredTower is not PlayerBase) {
                spotIndicator.gameObject.SetActive(true);
                spotIndicator.transform.position = hoveredTower.Spot.Position;
                spotIndicator.sprite = mode == Mode.Demolish ? demolishSprite : highlightSprite;
                spotIndicator.color = mode == Mode.Demolish ? Color.red : Color.green;

                rangeIndicator.gameObject.SetActive(true);
                rangeIndicator.transform.position = hoveredTower.Spot.Position;
                rangeIndicator.localScale = hoveredTower.Config.AttackRange * 2f * Vector3.one;
            }
            else {
                spotIndicator.gameObject.SetActive(false);
                rangeIndicator.gameObject.SetActive(false);
            }
        }
        #endregion
    }
}
