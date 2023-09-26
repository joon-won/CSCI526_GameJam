using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSCI526GameJam {
    public abstract class Enemy : MonoBehaviour {

        #region Fields
        [SerializeField] protected EnemyConfig config;

        [SerializeField] protected Numeric attackDamage;
        [SerializeField] private Numeric moveSpeed;
        [SerializeField] private float currentHitPoint;
        [SerializeField] private Numeric maxHitPoint;
        [SerializeField] private Numeric armor;
        [SerializeField] protected bool isAlive = true;

        [SerializeField] private Path path;
        private Coroutine pathRoutine;
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
                onDeath?.Invoke();
            }
        }
        public void FreezeEntity(float duration)
        {
            if (!isAlive)
                return;

            Numeric originalMoveSpeed = moveSpeed;
            moveSpeed = 0;
            StartCoroutine(RestoreMoveSpeedAfterDelay(duration, originalMoveSpeed));
        }

        private IEnumerator RestoreMoveSpeedAfterDelay(float delay, Numeric originalMoveSpeed)
        {
            yield return new WaitForSeconds(delay);

            moveSpeed = originalMoveSpeed;
        }

        private void Awake() {
            InitNumerics();
            currentHitPoint = maxHitPoint;
        }

        // Start is called before the first frame update
        void Start() {

        }

        // Update is called once per frame
        void Update() {

        }

        public void Follow(Path path) {
            if (pathRoutine != null) return;
            if (path.Spots.Count == 0) {
                Debug.LogWarning($"The path assigned to enemy {name} is empty. ");
                return;
            }

            moveSpeed = new(2f);
            this.path = path;
            pathRoutine = StartCoroutine(FollowPathRoutine());
        }

        private IEnumerator FollowPathRoutine() {
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