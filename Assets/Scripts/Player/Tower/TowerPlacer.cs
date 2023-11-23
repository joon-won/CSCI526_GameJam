using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSCI526GameJam {

    public class TowerPlacer : MonoBehaviour {

        #region Fields
        [MandatoryFields]
        [SerializeField] private LayerMask blockerLayer;

        [ComputedFields]
        [SerializeField] private bool isPreviewing = false;
        //[SerializeField] private TowerConfig config;

        [SerializeReference] private Spot cachedSpot;
        [SerializeField] private Tower cachedTower;
        [SerializeField] private bool canBuild;

        private SpriteRenderer spriteRenderer;
        #endregion

        #region Publics
        public event Action<TowerConfig> OnPlaced;
        public event Action OnCanceled;

        public bool IsPreviewing => isPreviewing;

        /// <summary>
        /// Start previewing a tower. 
        /// </summary>
        /// <param name="config">Config of the tower to be previewed. </param>
        public void StartPreview(Tower tower) {
            if (isPreviewing) {
                CancelPreview();
            }
            isPreviewing = true;
            spriteRenderer.sprite = tower.Config.Preview;
            cachedTower = tower;

            Refresh();
        }

        /// <summary>
        /// Cancel current preview. 
        /// </summary>
        public void CancelPreview() {
            if (!isPreviewing) return;

            isPreviewing = false;
            cachedTower = null;
            cachedSpot = null;
            spriteRenderer.sprite = null;

            OnCanceled?.Invoke();
        }

        /// <summary>
        /// Place currently previewed tower. 
        /// </summary>
        public bool TryPlace() {
            if (!canBuild) {
                // TODO: Invoke OnBuildBlocked
                return false;
            }
            if (!IsBaseReachable(cachedSpot)) {
                // TODO: Invoke OnPathBlocked
                return false;
            }

            cachedTower.Build(cachedSpot);
            OnPlaced?.Invoke(cachedTower.Config);

            CancelPreview();
            return true;
        }
        #endregion

        #region Internals
        private bool IsBaseReachable(Spot spot) {
            var path = new Path(MapManager.Instance.Spots[0, 0], TowerManager.Instance.PlayerBase.Spot, new() { spot });
            return path.GroundSpots.Count > 0;
        }

        private void Refresh(Spot spot = null) {
            if (!isPreviewing) return;

            cachedSpot = spot ? spot : MapManager.Instance.MouseSpot;

            var isBlocked = Physics2D.OverlapBox(cachedSpot.Position, new(Configs.CellSize, Configs.CellSize), 0f, blockerLayer);
            canBuild = isBlocked || !IsBaseReachable(spot)
                ? false : cachedTower.CanBuild(cachedSpot);
            spriteRenderer.color = canBuild ? Color.white : Color.red;

            transform.position = cachedSpot.Position;
        }
        #endregion

        #region Unity Methods
        private void Awake() {
            spriteRenderer = GetComponent<SpriteRenderer>();
            isPreviewing = false;
        }

        private void Start() {
            MapManager.Instance.OnMouseSpotChanged += Refresh;
            Refresh();
        }
        #endregion
    }
}
