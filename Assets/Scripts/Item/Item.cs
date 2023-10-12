//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//namespace CSCI526GameJam {

//    [Serializable]
//    public class Item {

//        #region Fields
//        [SerializeField] private Card config;
//        [SerializeField] private int numAvailable;
//        [SerializeField] private int numStacked;
//        #endregion

//        #region Publics
//        public Action OnChange;

//        public Card Config => config;
//        public int NumAvailable => numAvailable;
//        public int NumStacked => numStacked;

//        public Item(Card config) {
//            this.config = config;
//            numAvailable = config.NumAvailable;
//        }

//        public void Add(int value = 1) {
//            if (value == 0) return;
//            if (numAvailable == 0) return;

//            if (value < 0) {
//                Debug.LogWarning($"Value ({value}) cannot be negative. ");
//                return;
//            }

//            value = Mathf.Min(value, numAvailable);
//            numStacked += value;
//            numAvailable -= value;

//            for (int i = 0; i < value; i++) {
//                config.OnAdd();
//            }
//            OnChange?.Invoke();
//        }

//        public void Remove(int value = 1) {
//            if (value == 0) return;
//            if (numStacked == 0) return;

//            if (value < 0) {
//                Debug.LogWarning($"Value ({value}) cannot be negative. ");
//                return;
//            }

//            value = Mathf.Max(value, numStacked);
//            numStacked -= value;
//            numAvailable += value;

//            if (numStacked < 0) {
//                Debug.LogWarning($"Num of item {config.name} is negative. ");
//            }

//            for (int i = 0; i < value; i++) {
//                config.OnRemove();
//            }
//            OnChange?.Invoke();
//        }
//        #endregion

//        #region Internals
//        #endregion

//        #region Unity Methods
//        #endregion
//    }
//}
