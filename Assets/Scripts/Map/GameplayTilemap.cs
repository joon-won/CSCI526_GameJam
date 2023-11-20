using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;
using System.Linq;

namespace CSCI526GameJam {
    public class GameplayTilemap : MonoBehaviour {

        #region Fields
        [MandatoryFields]
        [SerializeField] private Tilemap tilemap;
        [SerializeField] private TileInfo[] baseTileInfos;
        #endregion

        #region Publics
        public void Draw(Spot spot) {
            var tile = GetRandomTile();
            var index = spot.Index;
            //var spread = GetSpread(index.x, index.y);
            //if (spread) {
            //    tile = spread.Tile;
            //    CreateDeposit(spread, spot);
            //}
            tilemap.SetTile(new Vector3Int(index.x, index.y, 0), tile);
        }

        //public void SetTile(ProductConfig data, int x, int y) {
        //    Vector3Int position = new Vector3Int(x, y, 0);
        //    var spread = GetSpread(data);
        //    Tile tile = baseTile;
        //    if (spread)
        //        tile = spread.Tile;

        //    tilemap.SetTile(position, tile);
        //}

        //public void CreateDeposit(DepositSpread spread, Spot spot) {
        //    var deposit = new Deposit(spread.Config, 1000, spread.Color, spread.ExtractedBy); // TODO: change amount later
        //    spot.SetDeposit(deposit);
        //}

        //public DepositSpread GetSpread(ProductConfig data) {
        //    if (typeToSpread.ContainsKey(data)) {
        //        return typeToSpread[data];
        //    }
        //    return null;
        //}

        //public DepositSpread GetSpread(int x, int y) {
        //    foreach (var kvp in typeToSpread) {
        //        if (kvp.Value.HasResource(x, y)) {
        //            return kvp.Value;
        //        }
        //    }
        //    return null;
        //}

        public void Init(int seed) {
            //var width = MapManager.Instance.MapSize;
            //var height = width;

            //typeToSpread.Clear();
            //foreach (var spread in spreadData.Spreads) {
            //    spread.GenerateSpread(seed, width, height);
            //    typeToSpread.Add(spread.Config, spread);
            //}
        }

        public Tuple<Vector3, Vector3> ComputeSizeAndCenter() {
            var bounds = tilemap.cellBounds;
            var minTilePosition = bounds.min;
            var maxTilePosition = bounds.max;
            var minWorldPosition = tilemap.CellToWorld(minTilePosition);
            var maxWorldPosition = tilemap.CellToWorld(maxTilePosition);
            var size = maxWorldPosition - minWorldPosition;
            var center = (minWorldPosition + maxWorldPosition) / 2f;
            return new Tuple<Vector3, Vector3>(size, center);
        }
        #endregion

        #region Internals
        private Tile GetRandomTile() {
            var totalChance = baseTileInfos.Sum(x => x.Chance);
            var randomValue = Random.Range(0f, totalChance);

            foreach (var tileInfo in baseTileInfos) {
                if (randomValue <= tileInfo.Chance) return tileInfo.Tile;
                randomValue -= tileInfo.Chance;
            }
            return baseTileInfos[0].Tile;
        }
        #endregion

        #region Unity Methods
        #endregion

        [Serializable]
        private struct TileInfo {
            public Tile Tile;
            [Range(0f, 1f)]
            public float Chance;
        }
    }
}
