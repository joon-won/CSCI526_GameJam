using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CSCI526GameJam {
    public class Shop : MonoBehaviourSingleton<Shop> {

        #region Fields
        [ComputedFields]
        [SerializeField] private int gold;
        [ShowInInspector] private Dictionary<ItemConfig, Item> configToItem = new();
        [ShowInInspector] private Dictionary<ItemRank, List<Item>> rankToItem = new();

        [SerializeField] private List<ItemConfig> itemConfigs;
#if UNITY_EDITOR
        [EditorOnlyFields]
        [FolderPath, SerializeField]
        private string itemConfigsPath;

        [Button("Find Item Configs", ButtonSizes.Large)]
        private void FindAssets() {
            itemConfigs = Utility.FindRefsInFolder<ItemConfig>(itemConfigsPath);
            Debug.Log($"Found {itemConfigs.Count} item configs under {itemConfigsPath}. ");
        }
#endif
        #endregion

        #region Publics
        public event Action<Item> OnItemAdded;
        public event Action<int> OnGoldChanged;

        public int Gold => gold;

        /// <summary>
        /// Get a random item from given rank. 
        /// </summary>
        /// <param name="rank">Rank of the item. </param>
        /// <returns>A random item. </returns>
        public ItemConfig GetRandom(ItemRank rank) {
            var items = rankToItem[rank];
            var index = Random.Range(0, items.Count);
            return items[index].Config;
        }

        /// <summary>
        /// Try to buy a item. 
        /// </summary>
        /// <param name="config">Config of the item. </param>
        public bool Buy(ItemConfig config) {
            if (config.Price > gold) return false;

            var item = configToItem[config];
            if (item.NumAvailable == 0) {
                Debug.LogWarning($"Trying to buy an item {config.ItemName} with 0 numAvailable. ");
                return false;
            }

            gold -= config.Price;
            item.Add();

            OnItemAdded?.Invoke(item);
            OnGoldChanged?.Invoke(gold);
            return true;
        }

        /// <summary>
        /// Try to sell an item. 
        /// </summary>
        /// <param name="config">Config of the item. </param>
        public void Sell(ItemConfig config) {
            var item = configToItem[config];
            if (item.NumStacked == 0) {
                Debug.LogWarning($"Trying to sell an item {config.ItemName} with 0 numStacked. ");
                return;
            }

            item.Remove();
            gold += config.Price;

            OnGoldChanged?.Invoke(gold);
        }

        /// <summary>
        /// Try to buy a tower. 
        /// </summary>
        /// <param name="config">Config of the tower. </param>
        public bool Buy(TowerConfig config) {
            if (config.Price > gold) return false;

            gold -= config.Price;

            OnGoldChanged?.Invoke(gold);
            return true;
        }

        /// <summary>
        /// Try to sell a tower. 
        /// </summary>
        /// <param name="config">Config of the tower. </param>
        public void Sell(TowerConfig config) {
            gold += config.Price;
            OnGoldChanged?.Invoke(gold);
        }

        /// <summary>
        /// Utility function that adds up gold dropped by killing enemy.
        /// </summary>
        /// <param name="gold">Amount of gold dropped.</param>
        public void addReward(int goldAmount) {
            gold += goldAmount;
            OnGoldChanged?.Invoke(gold);
        }

        /// <summary>
        /// (Debug) Add all items, ignoring numAvailable and price. 
        /// </summary>
        /// <param name="num">Num of each item to add. </param>
        public void Debug_addItems(int num = 1) {
            foreach (var config in itemConfigs) {
                for (int i = 0; i < num; i++) {
                    var item = configToItem[config];
                    item.Add();
                    OnItemAdded?.Invoke(item);
                }
            }
        }    
        #endregion

        #region Internals
        #endregion

        #region Unity Methods
        protected override void Awake() {
            base.Awake();

            // Init items by its config and rank. 
            foreach (var config in itemConfigs) {
                var item = new Item(config);
                configToItem.Add(config, item);

                var rank = config.Rank;
                if (!rankToItem.ContainsKey(rank)) {
                    rankToItem[rank] = new();
                }
                rankToItem[rank].Add(item);
            }
        }
        #endregion
    }
}
