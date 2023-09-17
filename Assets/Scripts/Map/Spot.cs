using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace CSCI526GameJam {

    //[Serializable]
    //public class SpotData {
    //    public DepositData DepositData;
    //    public bool IsExplored;
    //}

    [Serializable]
    public class Spot {

        #region Fields
        [ComputedFields]
        [SerializeField] private Vector2Int index;
        [SerializeField] private Vector3 position;
        //[SerializeField] private Building building;
        #endregion

        #region Publics
        public delegate void OnChanged();
        public OnChanged OnChange;

        public Vector2Int Index => index;
        public Vector3 Position => position;
        //public Building Building => building;

        public Spot Top => GetAdjacent(Direction.Top);
        public Spot Bottom => GetAdjacent(Direction.Bottom);
        public Spot Left => GetAdjacent(Direction.Left);
        public Spot Right => GetAdjacent(Direction.Right);


        public static implicit operator bool(Spot obj) {
            return obj != null;
        }

        public Spot(int x, int y, Vector3 position) {
            index = new Vector2Int(x, y);
            this.position = position;
        }

        public Spot GetAdjacent(Direction direction) {
            int x = index.x;
            int y = index.y;
            switch (direction) {
                case (Direction.Top):
                    y++;
                    break;
                case (Direction.Bottom):
                    y--;
                    break;
                case (Direction.Left):
                    x--;
                    break;
                case (Direction.Right):
                    x++;
                    break;
            }
            return MapManager.Instance.Get(x, y);
        }

        //public SpotData SaveData() {
        //    var data = new SpotData() {
        //        IsExplored = isExplored
        //    };
        //    if (deposit) {
        //        data.DepositData = deposit.SaveData();
        //    }
        //    return data;
        //}

        //public void ReadData(SpotData data) {
        //    if (data.DepositData != null) {
        //        deposit = new(data.DepositData);
        //    }
        //    isExplored = data.IsExplored;
        //}
        #endregion

        #region Internals
        #endregion
    }
}
