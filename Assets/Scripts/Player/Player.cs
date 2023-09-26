using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSCI526GameJam {
    public class Player : MonoBehaviourSingleton<Player> {

        #region Fields
        [ComputedFields]
        [SerializeField] private int gold;
        #endregion

        #region Publics
        public event Action<int> OnGoldChanged;
        public int Gold => gold;

        public bool CanPay(int value) {
            return gold >= value;
        }

        public bool TryPay(int value) {
            if (!CanPay(value)) return false;

            gold -= value;
            OnGoldChanged?.Invoke(gold);
            return true;
        }

        public void AddGold(int value) {
            gold += value;
            OnGoldChanged?.Invoke(gold);
        }
        #endregion

        #region Internals
        #endregion

        #region Unity Methods
        protected override void Awake() {
            base.Awake();

            // Debug
            gold = 1000000;
            Debug.Log($"<Debug> Gold is set to {gold}. ");
        }
        #endregion
    }
}
