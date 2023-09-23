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

        // NOTE: Test
        private void Debug_buy(string itemName) {
            foreach (var config in itemConfigs) {
                if (config.ItemName == itemName) Buy(config);
            }
        }

        [Button("Buy Apple", ButtonSizes.Medium)]
        private void BuyApple() {
            Debug_buy("Apple");
        }
        [Button("Buy Banana", ButtonSizes.Medium)]
        private void BuyBanana() {
            Debug_buy("Banana");
        }
        [Button("Buy Cat", ButtonSizes.Medium)]
        private void BuyCat() {
            Debug_buy("Cat");
        }
        #endregion

        #region Publics
        public event Action<Item> OnItemAdded;

        public ItemConfig GetRandom(ItemRank rank) {
            var items = rankToItem[rank];
            var index = Random.Range(0, items.Count);
            return items[index].Config;
        }

        public void Buy(ItemConfig config) {
            if (config.Price > gold) return;

            var item = configToItem[config];
            if (item.NumAvailable == 0) {
                Debug.LogWarning($"Trying to buy an item {config.ItemName} with 0 numAvailable. ");
                return;
            }

            item.Add();
            OnItemAdded?.Invoke(item);
        }

        // TODO: To be modified. 
        public void Sell(ItemConfig config) {
            var item = configToItem[config];
            if (item.NumStacked == 0) {
                Debug.LogWarning($"Trying to sell an item {config.ItemName} with 0 numStacked. ");
                return;
            }

            item.Remove();
            gold += config.Price;
        }

        public void Debug_addItems(int num) {
            foreach (var data in itemConfigs) {
                for (int i = 0; i < num; i++) {
                    Buy(data);
                }
            }
        }
        #endregion

        #region Internals
        #endregion

        #region Unity Methods
        protected override void Awake() {
            base.Awake();

            foreach (var data in itemConfigs) {
                var item = new Item(data);
                configToItem.Add(data, item);

                var rank = data.Rank;
                if (!rankToItem.ContainsKey(rank)) {
                    rankToItem[rank] = new();
                }
                rankToItem[rank].Add(item);
            }
        }
        #endregion
    }
}
