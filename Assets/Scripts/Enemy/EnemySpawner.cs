using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace CSCI526GameJam
{
    public class EnemySpawner : MonoBehaviour
    {

        #region Fields
        [MandatoryFields]
        [SerializeField] private Tilemap tilemap;
        [SerializeField] private string[] enemyNames = {"BugEnemy", "BeastEnemy", "ShroomEnemy"};

        #endregion
        //spawning areas representing North, East, West, South current values are placeholders
        public Vector2[] spawnZones = {Vector2.zero, 
                                        new Vector2Int(10, 10), 
                                        new Vector2Int(10, 10), 
                                        new Vector2Int(10, 10)};

        int enemyCount = 20;

        //Enemy gen counter 
        

        //Temporary locations for mobgen
        public Vector2 spawnAreaCenter = new Vector2(10000, 10000);
        public Vector2 spawnAreaSize = new Vector2(10000, 10000);

        //Util function to get random enemy from enemyNames array
        private string GetRandomEnemy()
        {
            return enemyNames[Random.Range(0, enemyNames.Length)];
        }

        void Start()
        {
            spawnAreaCenter = MapManager.Instance.MapCenter;

            for (int i = 0; i < enemyCount; i++)
        {
            // Randomly choose one of the prefab names
            string selectedEnemyName = GetRandomEnemy();

            // Load the chosen prefab from the "Resources" folder
            GameObject selectedEnemy = Resources.Load<GameObject>(selectedEnemyName);


            Vector3Int randomPosition = new Vector3Int(
                Mathf.FloorToInt(Random.Range(spawnAreaCenter.x - spawnAreaSize.x / 2, spawnAreaCenter.x + spawnAreaSize.x / 2) * 100 + 500),
                Mathf.FloorToInt(Random.Range(spawnAreaCenter.x - spawnAreaSize.x / 2, spawnAreaCenter.x + spawnAreaSize.x / 2) * 100 + 250),
                0
            );

            // Instantiate(selectedEnemy, randomPosition, Quaternion.identity);

            // Get the world position of the grid cell
            Vector3 cellWorldPosition = tilemap.GetCellCenterWorld(randomPosition);

            // Instantiate the prefab and set its position
            Instantiate(selectedEnemy, cellWorldPosition, Quaternion.identity);            
            
        }
        }

    }
}
