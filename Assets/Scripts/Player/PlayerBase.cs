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
        [SerializeField] private DamageFlasher damageFlasher;

        [ComputedFields]
        [SerializeField] private float health;
        #endregion

        #region Publics
        public event Action OnDamaged;
        public event Action OnHealed;
        public event Action OnDied;

        public float Health => health;
        public float MaxHealth => maxHealth;

        public void TakeDamage(float damage) {
            if (health <= 0f) return;
            
            health -= damage;
            health = Mathf.Max(0f, health);
            damageFlasher.Flash();

            OnDamaged?.Invoke();

            if (health <= 0f) {
                OnDied?.Invoke();
            }
        }

        public void Heal(float amount) {
            if (amount <= 0f) return;

            health = Mathf.Min(maxHealth, health + amount);
            OnHealed?.Invoke();
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
