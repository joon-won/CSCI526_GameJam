using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CSCI526GameJam {
    
    public class Enemy : Buffable {

        #region Fields
        [ClassHeader(typeof(Enemy))]
        
        [MandatoryFields]
        [SerializeField] private EnemyConfig config;

        [ComputedFields]
        [SerializeField] private float currentHitPoint;
        [SerializeField] private Numeric maxHitPoint;
        
        [SerializeField] private Numeric attackDamage;
        [SerializeField] private Numeric armor;
        [SerializeField] private Numeric moveSpeed;
        
        [SerializeField] private Numeric gold;

        [SerializeField] private bool isAlive = true;

        private Coroutine pathRoutine;
        private Coroutine freezeRoutine;

        private SpriteRenderer spriteRenderer;
        private DamageFlasher damageFlasher;
        private Animator animator;
        #endregion

        #region Public
        public event Action OnDied;
        public event Action<float> OnHitPointChanged;

        public EnemyConfig Config => config;
        public bool IsAlive => isAlive;
        public bool IsFlying => config.IsFlying;

        public float CurrentHitPoint => currentHitPoint;
        public float MaxHitPoint => maxHitPoint;

        public Numeric AttackDamage => attackDamage;
        public Numeric MoveSpeed => moveSpeed;
        public Numeric Armor => armor;
        public Numeric Gold => gold;


        public void InitNumerics() {
            var stats = StatSystem.Instance.Enemy;
            
            maxHitPoint = new(config.MaxHitPoint);
            maxHitPoint.AddModifierSet(stats.MaxHealth);
            
            attackDamage = new(config.AttackDamage);
            attackDamage.AddModifierSet(stats.Damage);
            
            moveSpeed = new(config.MoveSpeed);
            moveSpeed.AddModifierSet(stats.MoveSpeed);
            moveSpeed.OnChanged += () => {
                var speed = moveSpeed / config.MoveSpeed;
                animator.speed = Mathf.Max(0f, speed);
            };
            
            armor = new(config.Armor);
            armor.AddModifierSet(stats.Armor);
            
            gold = new(config.GoldDrop);
            gold.AddModifierSet(stats.Gold);
        }

        public void TakeDamage(float damage) {
            var finalRatio = 100f / (100f + armor);
            var finalDamage = Mathf.Max(0f, damage * finalRatio);
            currentHitPoint -= damage;
            damageFlasher.Flash();

            if (isAlive && currentHitPoint <= 0f) {
                isAlive = false;
                Die();
            }

            OnHitPointChanged?.Invoke(-finalDamage);
        }

        public void Follow(Path path) {
            if (pathRoutine != null) return;

            var pathSpots = IsFlying ? path.AirSpots : path.GroundSpots;
            if (pathSpots.Count == 0) {
                Debug.LogWarning($"The path assigned to enemy {name} is empty. ");
                return;
            }
            pathRoutine = StartCoroutine(FollowPathRoutine(pathSpots));
        }
        #endregion

        #region Internals
        private IEnumerator FollowPathRoutine(List<Spot> pathSpots) {
            var index = 0;
            transform.position = pathSpots[index].Position;

            var distance = 0f;
            while (index < pathSpots.Count) {
                yield return null;

                var targetPos = pathSpots[index].Position;

                var xDiff = (targetPos - transform.position).x;
                if (xDiff > 0f) spriteRenderer.flipX = true;
                else if (xDiff < 0f) spriteRenderer.flipX = false;

                var step = Mathf.Max(0f, moveSpeed) * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, targetPos, step);
                distance -= step;
                if (distance <= 0f) {
                    index++;
                    if (index == pathSpots.Count) break;

                    // Handle overflowed step. 
                    transform.position = Vector3.MoveTowards(transform.position, pathSpots[index].Position, -distance);
                    distance += Vector3.Distance(pathSpots[index].Position, pathSpots[index - 1].Position);
                }
            }

            var end = pathSpots[^1];
            if (end.Tower == TowerManager.Instance.PlayerBase) {
                TowerManager.Instance.PlayerBase.TakeDamage(attackDamage);
                Die(false);
            }

            pathRoutine = null;
        }

        private void Die(bool dropGold = true) {
            Destroy(gameObject);
            if (dropGold) {
                Player.Instance.AddGold((int)gold);
            }
            OnDied?.Invoke();
        }
        #endregion

        #region Unity Methods
        private void Awake() {
            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = config.RegularSprite;
            damageFlasher = GetComponent<DamageFlasher>();
            animator = GetComponent<Animator>();

            InitNumerics();
            currentHitPoint = maxHitPoint;
        }
        #endregion
    }
}
