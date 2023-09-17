using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

namespace CSCI526GameJam {

    //[Serializable]
    //public class MapManagerData {
    //    public int Seed;
    //    [ShowInInspector]
    //    public Dictionary<Vector2Int, SpotData> IndexToSpotData = new();

    //    public MinimapData MinimapData;
    //}

    //public class MapManager : MonoBehaviourSingleton<MapManager>, ISaveable<MapManagerData> {
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

        private HashSet<Vector2Int> aroundPlayer = new();

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


        public Spot Get(int x, int y) {
            if (x < 0 || y < 0 || x >= Spots.Width || y >= Spots.Height)
                return null;
            return Spots[x, y];
        }

        public Action OnSpotRevealed;


        public void GenerateMap() {
            seed = UnityEngine.Random.Range(-10000, 10000);
            gameplayTilemap.Init(seed);
            GenerateSpots();

            var result = gameplayTilemap.ComputeSizeAndCenter();
            mapDimension = result.Item1;
            mapCenter = result.Item2;
        }

        public int RoundToIndex(float value) {
            return Mathf.FloorToInt(value);
        }

        public bool IsOnMap(Vector3 position) {
            var rect = new Rect(MapCenter, Vector2.one * mapSize);
            return rect.Contains(position);
        }

        public Spot RoundToSpot(Vector3 worldPosition) {
            int x = RoundToIndex(worldPosition.x);
            int y = RoundToIndex(worldPosition.y);
            return Get(x, y);
        }

        //public MapManagerData SaveData() {
        //    var data = new MapManagerData {
        //        Seed = seed,
        //        MinimapData = minimap.SaveData()
        //    };

        //    foreach (var spot in Spots) {
        //        var spotData = spot.SaveData();
        //        data.IndexToSpotData[spot.Index] = spotData;
        //    }

        //    return data;
        //}

        //public void ReadData(MapManagerData data) {
        //    seed = data.Seed;

        //    gameplayTilemap.Init(seed);
        //    GenerateSpots();

        //    var result = gameplayTilemap.ComputeSizeAndCenter();
        //    mapDimension = result.Item1;
        //    mapCenter = result.Item2;

        //    foreach (var kvp in data.IndexToSpotData) {
        //        var index = kvp.Key;
        //        var spotData = kvp.Value;
        //        index.GetSpot().ReadData(spotData);
        //    }
        //    minimap.ReadData(data.MinimapData);
        //}
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
