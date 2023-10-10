using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace CSCI526GameJam
{

    /// <summary>
    /// Basic unit of the grid-based map. 
    /// </summary>

    [Serializable]
    public class Spot
    {

        #region Fields
        [ComputedFields]
        [SerializeField] private Vector2Int index;
        [SerializeField] private Vector3 position;
        [SerializeField] private Tower tower;
        [SerializeField] private bool constructable = true;
        #endregion

        #region Publics
        public delegate void OnChanged();
        public OnChanged OnChange;

        public Vector2Int Index => index;
        public Vector3 Position => position;
        public Tower Tower => tower;
        public bool Constructable => constructable;

        public Spot Top => GetAdjacent(Direction.Top);
        public Spot Bottom => GetAdjacent(Direction.Bottom);
        public Spot Left => GetAdjacent(Direction.Left);
        public Spot Right => GetAdjacent(Direction.Right);


        public static implicit operator bool(Spot obj)
        {
            return obj != null;
        }

        /// <summary>
        /// Constructor for <see cref="Spot"/>. 
        /// </summary>
        /// <param name="x">The x index. </param>
        /// <param name="y">The y index. </param>
        /// <param name="position">World position. </param>
        public Spot(int x, int y, Vector3 position)
        {
            index = new Vector2Int(x, y);
            this.position = position;
        }

        /// <summary>
        /// Get an adjacent spot. 
        /// </summary>
        /// <param name="direction">Adjacent direction. </param>
        /// <returns></returns>
        public Spot GetAdjacent(Direction direction)
        {
            int x = index.x;
            int y = index.y;
            switch (direction)
            {
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

        public void SetBuilding(Tower building)
        {
            this.tower = building;
            OnChange?.Invoke();
        }


        // Set the spot to be constructable or not
        public void SetConstructable(bool cons)
        {
            this.constructable = cons;
            OnChange?.Invoke();
        }
        #endregion

        #region Internals
        #endregion
    }
}
