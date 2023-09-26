using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CSCI526GameJam {
    public class ItemManager : MonoBehaviourSingleton<ItemManager> {

        #region Fields
        [ComputedFields]
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
        public event Action<Item> OnItemRemoved;

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
        public void Buy(ItemConfig config) {
            var item = configToItem[config];
            if (item.NumAvailable == 0) {
                Debug.LogWarning($"Trying to buy an item {config.ItemName} with 0 numAvailable. ");
                return;
            }
            if (!Player.Instance.TryPay(config.Price)) return;

            item.Add();
            OnItemAdded?.Invoke(item);
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
            Player.Instance.AddGold(config.Price);
            OnItemRemoved?.Invoke(item);
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
