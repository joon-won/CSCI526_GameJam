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
        public Action OnStop;

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

            OnStop?.Invoke();
        }

        /// <summary>
        /// Place currently previewed tower. 
        /// </summary>
        public bool TryPlace() {
            if (!canBuild) return false;
            if (!Player.Instance.TryPay(cachedTower.Config.Price)) return false;

            cachedTower.Build(cachedSpot);
            CancelPreview();
            return true;
        }
        #endregion

        #region Internals
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

        private void Refresh(Spot spot = null) {
            if (!isPreviewing) return;

            cachedSpot = spot ? spot : MapManager.Instance.MouseSpot;

            var isBlocked = Physics2D.OverlapBox(cachedSpot.Position, new(Configs.CellSize, Configs.CellSize), 0f, blockerLayer);
            canBuild = isBlocked ? false : cachedTower.CanBuild(cachedSpot);
            spriteRenderer.color = canBuild ? Color.white : Color.red;

            transform.position = cachedSpot.Position;
        }
        #endregion
    }
}
