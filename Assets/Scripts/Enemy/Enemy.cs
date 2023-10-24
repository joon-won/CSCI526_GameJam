using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSCI526GameJam {
    public abstract class Enemy : Buffable {

        #region Fields
        [SerializeField] protected EnemyConfig config;

        [SerializeField] protected Numeric attackDamage;
        [SerializeField] private Numeric moveSpeed;
        [SerializeField] private float currentHitPoint;
        [SerializeField] private Numeric maxHitPoint;
        [SerializeField] private Numeric armor;
        [SerializeField] private Numeric gold;

        [SerializeField] protected bool isAlive = true;

        private Coroutine pathRoutine;
        private Coroutine freezeRoutine;

        private SpriteRenderer spriteRenderer;
        #endregion

        #region Public
        public Action onDeath;

        public EnemyConfig Config => config;
        public bool IsAlive => isAlive;

        public float CurrentHitPoint => currentHitPoint;
        public float MaxHitPoint => maxHitPoint;
        public event Action<float> OnHitPointChanged;

        public Numeric AttackDamage => attackDamage;
        public Numeric MoveSpeed => moveSpeed;
        public Numeric Armor => armor;
        public Numeric Gold => gold;
        #endregion

        public void InitNumerics() {
            maxHitPoint = new(config.MaxHitPoint);
            attackDamage = new(config.AttackDamage);
            moveSpeed = new(config.MoveSpeed);
            armor = new(config.Armor);
            gold = new(config.GoldDrop);
        }

        public void TakeDamage(float damage) {
            currentHitPoint -= damage;

            if (isAlive && currentHitPoint <= 0f) {
                isAlive = false;
                Die();
            }

            OnHitPointChanged?.Invoke(-damage);
        }

        private void Die(bool dropGold = true) {
            Destroy(gameObject);
            if (dropGold) {
                Player.Instance.AddGold((int)gold);
            }
            onDeath?.Invoke();
        }

        public void FreezeEntity(float duration) {
            if (!isAlive)
                return;

            if (freezeRoutine != null) return;

            freezeRoutine = StartCoroutine(FreezeRoutine(duration));
        }

        private IEnumerator FreezeRoutine(float duration) 
        {
            var cooldown = 3f; // Get from config later

            var prevSpeed = moveSpeed;
            moveSpeed = new(0f);
            spriteRenderer.sprite = config.FrozenSprite;
            yield return new WaitForSeconds(duration);

            moveSpeed = prevSpeed;
            spriteRenderer.sprite = config.RegularSprite;
            yield return new WaitForSeconds(cooldown);

            freezeRoutine = null;
        }

        private void Awake() {
            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = config.RegularSprite;

            InitNumerics();
            currentHitPoint = maxHitPoint;
        }

        public void Follow(Path path) {
            if (pathRoutine != null) return;
            if (path.Spots.Count == 0) {
                Debug.LogWarning($"The path assigned to enemy {name} is empty. ");
                return;
            }            
            pathRoutine = StartCoroutine(FollowPathRoutine(path));
        }
        // Set the json output to get the in game values.
        private IEnumerator FollowPathRoutine(Path path) {
            var index = 0;
            transform.position = path.Spots[index].Position;

            var distance = 0f;
            while (index < path.Spots.Count) {
                yield return null;

                var step = moveSpeed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, path.Spots[index].Position, step);
                distance -= step;
                if (distance <= 0f) {
                    index++;
                    if (index == path.Spots.Count) break;

                    // Handle overflowed step. 
                    transform.position = Vector3.MoveTowards(transform.position, path.Spots[index].Position, -distance);
                    distance += Vector3.Distance(path.Spots[index].Position, path.Spots[index - 1].Position);
                }
            }

            var end = path.Spots[path.Spots.Count - 1];
            if (end.Tower == TowerManager.Instance.PlayerBase) {
                TowerManager.Instance.PlayerBase.TakeDamage(attackDamage);
                Die(false);
            }

            pathRoutine = null;
        }
        
    }
}