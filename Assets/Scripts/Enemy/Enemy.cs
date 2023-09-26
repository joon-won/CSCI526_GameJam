using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSCI526GameJam {
    public abstract class Enemy : MonoBehaviour {

        #region Fields
        [SerializeField] protected EnemyConfig config;

        [SerializeField] private Sprite regularSprite;
        [SerializeField] private Sprite frozenSprite;

        [SerializeField] protected Numeric attackDamage;
        [SerializeField] private Numeric moveSpeed;
        [SerializeField] private float currentHitPoint;
        [SerializeField] private Numeric maxHitPoint;
        [SerializeField] private Numeric armor;
        [SerializeField] protected bool isAlive = true;

        private Coroutine pathRoutine;
        private Coroutine freezeRoutine;

        private SpriteRenderer spriteRenderer;
        #endregion

        #region Public
        public Action onDeath;

        #endregion
        public bool IsDead() {
            return currentHitPoint <= 0f;
        }

        public void RemoveDeadEnemy() {
            if (IsDead()) {
                Destroy(gameObject);
                onDeath?.Invoke();
            }
        }

        public void InitNumerics() {
            maxHitPoint = new(100f);
            maxHitPoint.IncreaseScalar(1f);
        }

        public void TakeDamage(float damage) {
            currentHitPoint -= damage;

            if (IsDead()) {
                isAlive = false;
                Destroy(gameObject);
                DropGold(1);
                onDeath?.Invoke();
            }
        }

        public void DropGold(int gold) {
            Player.Instance.AddGold(gold);
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
            spriteRenderer.sprite = frozenSprite;
            yield return new WaitForSeconds(duration);

            moveSpeed = prevSpeed;
            spriteRenderer.sprite = regularSprite;
            yield return new WaitForSeconds(cooldown);

            freezeRoutine = null;
        }

        private void Awake() {
            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = regularSprite;

            InitNumerics();
            currentHitPoint = maxHitPoint;
        }

        public void Follow(Path path) {
            if (pathRoutine != null) return;
            if (path.Spots.Count == 0) {
                Debug.LogWarning($"The path assigned to enemy {name} is empty. ");
                return;
            }
            moveSpeed = new(2f);
            pathRoutine = StartCoroutine(FollowPathRoutine(path));
        }

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
            pathRoutine = null;
        }
        
    }
}