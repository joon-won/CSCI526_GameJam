using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CSCI526GameJam {
    public class MainMenuEnemySpawner : MonoBehaviour {

        #region Fields
        [MandatoryFields]
        [SerializeField] private Enemy[] enemyPrefabs;
        [SerializeField] private Vector3 spawnRectSize;
        [SerializeField] private Vector2 spawnIntervalRange;
        [SerializeField] private Vector2Int numSpawnsRange;
        [SerializeField] private float enemyDuration;

        private GameObject enemyHolder;
        private float spawnInterval;
        private float spawnElapsed;
        #endregion

        #region Publics
        #endregion

        #region Internals
        private IEnumerator MoveEnemyRoutine(Enemy enemy, Vector3 direction) {
            var elpased = 0f;
            enemy.GetComponent<SpriteRenderer>().flipX = direction.x > 0f;
            while (elpased < enemyDuration) {
                elpased += Time.deltaTime;
                enemy.transform.position += direction * enemy.Config.MoveSpeed * Time.deltaTime;
                yield return null;
            }

            Destroy(enemy.gameObject);
        }

        private int RandomSign() {
            if (Random.value > 0.5f) return 1;
            else return -1;
        }

        private Vector3 GetRandomPositionOutsideRect(Vector3 size, Vector3 center, float ratio = 1.5f) {
            var outerSize = size * ratio;
            var position = new Vector3(
                Random.Range(-outerSize.x, outerSize.x),
                Random.Range(-outerSize.y, outerSize.y),
                0f
            );

            if (RandomSign() > 0) {
                position.x = Random.Range(size.x, outerSize.x) * RandomSign();
            }
            else {
                position.y = Random.Range(size.y, outerSize.y) * RandomSign();
            }

            position *= 0.5f;
            position += center;
            return position;
        }
        #endregion

        #region Unity Methods
        private void Awake() {
            spawnInterval = Random.Range(spawnIntervalRange.x, spawnIntervalRange.y);
            enemyHolder = new GameObject("Enemy Instances");
            enemyHolder.transform.SetParent(transform);
        }
        private void Update() {
            if (spawnElapsed < spawnInterval) {
                spawnElapsed += Time.deltaTime;
                return;
            }

            spawnElapsed = 0f;
            spawnInterval = Random.Range(spawnIntervalRange.x, spawnIntervalRange.y);

            var numSpawns = Random.Range(numSpawnsRange.x, numSpawnsRange.y);
            for (int i = 0; i < numSpawns; i++) {
                var point = GetRandomPositionOutsideRect(spawnRectSize, transform.position, 1.2f);

                var enemy = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], enemyHolder.transform);
                enemy.transform.position = point;

                var direction = transform.position - point;
                StartCoroutine(MoveEnemyRoutine(enemy, direction.normalized));
            }
        }

        private void OnDrawGizmos() {
            Gizmos.DrawWireCube(transform.position, spawnRectSize);
        }
        #endregion
    }
}
