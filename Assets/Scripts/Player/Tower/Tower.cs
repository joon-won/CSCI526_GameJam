using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CSCI526GameJam {

    /// <summary>
    /// Abstract <see cref="Tower"/> class. 
    /// </summary>

    public abstract class Tower : MonoBehaviour {

        #region Fields
        [ClassHeader(typeof(Tower))]

        [MandatoryFields]
        [SerializeField] protected TowerConfig config;

        [ComputedFields]
        [SerializeField] protected Sprite previewImage;
        [SerializeField] protected Sprite regularImage;
        [SerializeField] protected TowerEntity entity;

        [SerializeReference] protected Damage attackDamage;
        [SerializeReference] protected Numeric attackSpeed;
        [SerializeReference] protected Numeric attackRange;
        [SerializeReference] protected Numeric critChance;
        [SerializeReference] protected Numeric critDamage;

        [SerializeField] protected float attackCooldown = 0f;
        [SerializeField] protected LayerMask targetLayerMask;
        #endregion

        #region Public
        public Action OnBuild;
        public Action OnDemolish;

        public TowerConfig Config => config;
        public string TowerName => config.name;
        public Sprite Preview => previewImage;
        public Sprite Image => regularImage;
        public Spot Spot => entity.Spot;
        public Collider2D Collider2D => entity.Collider2D;
        public Quaternion Rotation => entity.transform.rotation;
        public float AttackCooldown => attackCooldown;

        /// <summary>
        /// Check buildability of a Spot. 
        /// </summary>
        /// <param name="spot">Spot to be checked. </param>
        public bool CanBuild(Spot spot) {
            return !spot.Tower;
        }

        /// <summary>
        /// Build on a spot. Does not check for buildability.
        /// </summary>
        /// <param name="spot">Spot to be built on. </param>
        public virtual void Build(Spot spot) {
            if (!spot) {
                Debug.LogError($"{GetType()}: spot cannot be null. ");
                return;
            }
            if (Spot) {
                Debug.LogWarning($"{name} is already built. ");
                return;
            }

            gameObject.SetActive(true);

            entity.Settle(spot);
            transform.position = spot.Position;

            OnBuild?.Invoke();
        }

        /// <summary>
        /// Demolish the tower.
        /// </summary>
        public virtual void Demolish() {
            entity.Unsettle();
            Destroy(gameObject);

            OnDemolish?.Invoke();
        }
        #endregion

        #region Internal
        protected abstract bool CanAttack();
        protected abstract void PerformAttack();
        protected abstract void PerformUpdate();

        protected virtual void InitNumerics() {
            var stats = Stats.Instance.Tower;

            critChance = new(config.CritChance);
            critChance.AddNumericSet(stats.CritChance);

            critDamage = new(config.CritDamage);
            critDamage.AddNumericSet(stats.CritDamage);

            attackDamage = new(config.AttackDamage, critChance, critDamage);
            attackDamage.AddNumericSet(stats.AttackDamage);

            attackSpeed = new(config.AttackSpeed);
            attackSpeed.AddNumericSet(stats.AttackSpeed);

            attackRange = new(config.AttackRange);
            attackRange.AddNumericSet(stats.AttackRange);
        }

        protected virtual void TryAttack() {
            if (attackCooldown > 0f) return;
            if (!CanAttack()) return;

            attackCooldown = 1f / attackSpeed;
            PerformAttack();
        }

        private void UpdateAttackCooldown() {
            if (attackCooldown <= 0f) return;
            attackCooldown -= Time.deltaTime;
        }
        #endregion

        #region Unity Methods
        private void Awake() {
            previewImage = config.Preview;
            regularImage = config.Image;

            entity = GetComponentInChildren<TowerEntity>();
            entity.Init(this);
            targetLayerMask = LayerMask.GetMask("Enemy");

            InitNumerics();
        }

        private void Update() {
            PerformUpdate();
            UpdateAttackCooldown();
            TryAttack();
        }
        #endregion

        protected class Damage : Numeric {

            private Numeric critChance;
            private Numeric critDamage;

            public override float Value {
                get {
                    var value = base.Value;
                    if (Random.Range(0f, 1f) < critChance) {
                        value *= 1f + critDamage;
                    }
                    return value;
                }
            }

            public Damage(float baseDamage, Numeric critChance, Numeric critDamage) : base(baseDamage) {
                this.critChance = critChance;
                this.critDamage = critDamage;
            }
        }
    }
}
