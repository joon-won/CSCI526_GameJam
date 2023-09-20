using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CSCI526GameJam {

    public abstract class Defense : Building {
        #region Fields
        [ClassHeader(typeof(Defense))]

        [MandatoryFields]
        [SerializeReference] protected Numeric attackDamage;
        [SerializeReference] protected Numeric attackSpeed;
        [SerializeReference] protected Numeric attackRange;
        [SerializeReference] protected Numeric critChance;
        [SerializeReference] protected Numeric critDamage;

        [ComputedFields]
        [SerializeField] protected LayerMask targetLayerMask;
        [SerializeField] protected float attackCooldown = 0f;
        #endregion

        #region Publics
        public Quaternion Rotation => entity.transform.rotation;
        public float AttackCooldown => attackCooldown;

        public virtual float ComputeFinalDamage() {
            float finalDamage = attackDamage;
            if (Random.Range(0f, 1f) < critChance) {
                finalDamage *= 1f + critDamage;
            }
            return finalDamage;
        }
        #endregion

        #region Internals
        protected abstract bool CanAttack();
        protected abstract void PerformAttack();

        protected override void InitNumerics() {
            base.InitNumerics();

            var data = config as DefenseConfig;
            if (!data) {
                Debug.LogError($"{config.name} is not a DefenseData. ");
                return;
            }

            var stats = Stats.Instance.Defense;

            attackDamage = new(data.AttackDamage);
            attackDamage.AddNumericSet(stats.AttackDamage);

            attackSpeed = new(data.AttackSpeed);
            attackSpeed.AddNumericSet(stats.AttackSpeed);

            attackRange = new(data.AttackRange);
            attackRange.AddNumericSet(stats.AttackRange);

            critChance = new(data.CritChance);
            critChance.AddNumericSet(stats.CritChance);

            critDamage = new(data.CritDamage);
            critDamage.AddNumericSet(stats.CritDamage);
        }

        protected virtual void TryAttack() {
            if (attackCooldown > 0f) return;
            if (!CanAttack()) return;

            attackCooldown = 1f / attackSpeed;
            PerformAttack();
        }

        protected T GetAmmo<T>(ProductConfig data) where T : class {
            if (!data) {
                Debug.LogWarning("Ammo data is null. ");
                return null;
            }
            if (!ResourceManager.Instance.TryConsume(data, 1)) return null;

            var go = ProjectilePooler.Instance.Get(data);
            if (!go) {
                Debug.LogError($"{data.name} is not pooled. ");
            }
            return go.GetComponent<T>();
        }

        private void UpdateAttackCooldown() {
            if (attackCooldown <= 0f) return;
            attackCooldown -= Time.deltaTime;
        }
        #endregion

        #region Unity Methods
        protected override void Awake() {
            base.Awake();
            targetLayerMask = LayerMask.GetMask("Enemy");
        }

        protected override void Update() {
            base.Update();

            UpdateAttackCooldown();
            TryAttack();
        }

        protected virtual void OnDrawGizmosSelected() {
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
        #endregion
    }
}
