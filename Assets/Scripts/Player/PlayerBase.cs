using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSCI526GameJam {
    public class PlayerBase : Tower {

        #region Fields
        [ClassHeader(typeof(PlayerBase))]

        [MandatoryFields]
        [SerializeField] private float maxHealth;

        [ComputedFields]
        [SerializeField] private float health;
        #endregion

        #region Publics
        public event Action OnDamaged;

        public float Health => health;
        public float MaxHealth => maxHealth;

        public void TakeDamage(float damage) {
            health -= damage;
            health = Mathf.Max(0f, health);

            OnDamaged?.Invoke();

            if (health <= 0f) {
                Debug.Log("Game over");
            }
        }
        #endregion

        #region Internals
        protected override bool CanAttack() {
            return false;
        }

        protected override void PerformAttack() {
            return;
        }

        protected override void PerformUpdate() {
            return;
        }
        #endregion

        #region Unity Methods
        protected override void Awake() {
            base.Awake();
            health = maxHealth;
        }

        private void OnTriggerEnter2D(Collider2D collision) {
            Debug.Log($"TODO: Enemy hits base. Health: ");
        }
        #endregion
    }
}
