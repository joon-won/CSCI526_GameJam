using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

namespace CSCI526GameJam {

    /// <summary>
    /// Manage the map system. 
    /// </summary>

    public class MapManager : MonoBehaviourSingleton<MapManager> {

        #region Fields
        [Header("---Spot---")]
        [ComputedFields]
        [SerializeField] private Spot mouseSpot;

        [Header("---Tilemap---")]
        [MandatoryFields]
        [SerializeField] private int mapSize;

        [ComputedFields]
        [SerializeField] private int seed;
        [SerializeField] private Vector3 mapDimension;
        [SerializeField] private Vector3 mapCenter;

        private Grid grid;
        private GameplayTilemap gameplayTilemap;
        #endregion

        #region Publics
        public Action<Spot> OnMouseSpotChanged;
        public Action<Spot> OnPlayerSpotChanged;

        public Spot MouseSpot => mouseSpot;
        public Vector3 MapDimension => mapDimension;
        public int MapSize => mapSize;
        public Vector3 MapCenter => mapCenter;
        public GameplayTilemap GameplayTilemap => gameplayTilemap;

        // From left bottom to right top
        public Matrix<Spot> Spots { get; private set; } = new();

        /// <summary>
        /// Get a spot. 
        /// </summary>
        /// <param name="x">The x index. </param>
        /// <param name="y">The y index. </param>
        /// <returns>Spot at given index. Null if the index is out of bounds. </returns>
        public Spot Get(int x, int y) {
            if (x < 0 || y < 0 || x >= Spots.Width || y >= Spots.Height)
                return null;
            return Spots[x, y];
        }

        /// <summary>
        /// Generate the map. 
        /// </summary>
        public void GenerateMap() {
            seed = UnityEngine.Random.Range(-10000, 10000);
            gameplayTilemap.Init(seed);
            GenerateSpots();

            var result = gameplayTilemap.ComputeSizeAndCenter();
            mapDimension = result.Item1;
            mapCenter = result.Item2;
        }

        /// <summary>
        /// Check if a world position is on the map. 
        /// </summary>
        /// <param name="position">World position. </param>
        /// <returns>True if the position is on map. Else false. </returns>
        public bool IsOnMap(Vector3 position) {
            var rect = new Rect(MapCenter, Vector2.one * mapSize);
            return rect.Contains(position);
        }

        /// <summary>
        /// Find a spot by the given world position. 
        /// </summary>
        /// <param name="worldPosition">World position. </param>
        /// <returns>The Spot that corresponds to the world position. </returns>
        public Spot RoundToSpot(Vector3 worldPosition) {
            int x = Mathf.FloorToInt(worldPosition.x);
            int y = Mathf.FloorToInt(worldPosition.y);
            return Get(x, y);
        }
        #endregion

        #region Internals
        private void GenerateSpots() {
            var cellNum = mapSize;
            var cellSize = Configs.CellSize;
            grid.cellSize = new Vector3(cellSize, cellSize, 0f);

            Spots = new(mapSize, mapSize);
            for (int x = 0; x < cellNum; x++) {
                for (int y = 0; y < cellNum; y++) {
                    var tilePosition = new Vector3Int(x, y, 0);
                    var worldPosition = tilePosition + new Vector3(1, 1) * (cellSize / 2);
                    var spot = new Spot(x, y, worldPosition);
                    Spots[x, y] = spot;

                    gameplayTilemap.Draw(spot);
                }
            }
        }

        private void UpdateMouseSpot() {
            var mousePosition = InputManager.Instance.GetMousePositionInWorld();
            var spot = RoundToSpot(mousePosition);
            if (!spot) return;

            if (spot.Index != mouseSpot.Index) {
                mouseSpot = spot;
                OnMouseSpotChanged?.Invoke(mouseSpot);
            }
        }
        #endregion

        #region Unity Methods
        protected override void Awake() {
            base.Awake();

            grid = GetComponentInChildren<Grid>(true);
            gameplayTilemap = GetComponentInChildren<GameplayTilemap>(true);
        }

        private void Update() {
            UpdateMouseSpot();
        }
        #endregion
    }
}
