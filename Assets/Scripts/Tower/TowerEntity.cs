using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSCI526GameJam {

    public class TowerEntity : MonoBehaviour {

        #region Fields
        [ClassHeader(typeof(TowerEntity))]

        [ComputedFields]
        [SerializeField] private Tower building;

        [SerializeField] private bool isSettled;
        [SerializeReference] private Spot spot;

        private SpriteRenderer spriteRenderer;
        private Collider2D collider2d;
        #endregion

        #region Public
        public Tower Building => building;
        public Spot Spot => spot;
        public bool IsSettled => isSettled;
        public SpriteRenderer SpriteRenderer {
            get {
                if (!spriteRenderer) spriteRenderer = GetComponent<SpriteRenderer>();
                return spriteRenderer;
            }
        }
        public Collider2D Collider2D {
            get {
                if (!collider2d) collider2d = GetComponent<Collider2D>();
                return collider2d;
            }
        }

        public void Init(Tower building) {
            this.building = building;
        }

        public void Settle(Spot spot) {
            isSettled = true;
            this.spot = spot;
            spot.SetBuilding(building);
            SpriteRenderer.sprite = building.Image;
        }

        public void Unsettle() {
            isSettled = false;
            spot.SetBuilding(null);
            spot = null;
        }
        #endregion

        #region Internals
        #endregion

        #region Unity Methods
        #endregion
    }
}
