using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSCI526GameJam {
    public class Enemy : MonoBehaviour {

        #region Fields
        [SerializeField] protected EnemyConfig config;
        [SerializeField] protected Numeric attackDamage;
        [SerializeField] protected Numeric attackSpeed;
        [SerializeField] private Numeric attackRange;
        [SerializeField] private Numeric moveSpeed;
        [SerializeField] private Numeric hitPoint;
        [SerializeField] private Numeric hpMod;
        [SerializeField] private Numeric barrier;
        [SerializeField] private float attackCooldown = 0f;
        #endregion

        #region Public
        public Action onDeath;

        #endregion
        public bool IsDead() {
            return hitPoint.Value <= 0f;
        }

        public void RemoveDeadEnemy() {
            if (IsDead()) {
                Destroy(gameObject);
                onDeath?.Invoke();
            }
        }

        public void CalculateRealHP() {
            hitPoint = new(100f);
            hpMod = new(1f);
            barrier = new(0f);

            hitPoint.IncreaseScalar(hpMod);
            hitPoint.IncreaseValue(barrier);
        }

        public void TakeDamage(float damage) {
            hitPoint.IncreaseValue(-1 * barrier);
        }

        [SerializeField] private Path path;
        private Coroutine pathRoutine;
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

        // Start is called before the first frame update
        void Start() {
            CalculateRealHP();
        }

        // Update is called once per frame
        void Update() {
            RemoveDeadEnemy();
        }

        private void OnDrawGizmos() {
            var color = Color.red;
            color.a = 0.5f;
            Gizmos.color = color;
            foreach (var spot in path.Spots) {
                Gizmos.DrawCube(spot.Position, Vector3.one);
            }
        }
    }
}
