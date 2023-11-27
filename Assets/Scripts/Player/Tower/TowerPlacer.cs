using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace CSCI526GameJam {

    public class TowerPlacer : MonoBehaviour {

        #region Fields
        [MandatoryFields]
        [SerializeField] private float moveAnimDuration;
        [SerializeField] private LayerMask blockerLayer;
        [SerializeField] private Transform rangeIndicator;

        [ComputedFields]
        [SerializeField] private bool isPreviewing = false;
        //[SerializeField] private TowerConfig config;

        [SerializeReference] private Spot cachedSpot;
        [SerializeField] private Tower cachedTower;
        [SerializeField] private bool canBuild;

        private SpriteRenderer spriteRenderer;
        private Tween moveTween;
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

            rangeIndicator.gameObject.SetActive(true);
            rangeIndicator.localScale = tower.Config.AttackRange * 2f * Vector3.one;

            transform.position = MapManager.Instance.MouseSpot.Position;
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
            rangeIndicator.gameObject.SetActive(false);

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

            var placedConfig = cachedTower.Config;
            cachedTower.Build(cachedSpot);
            CancelPreview();

            OnPlaced?.Invoke(placedConfig);
            return true;
        }
        #endregion

        #region Internals
        private void Refresh(Spot spot = null) {
            if (!isPreviewing) return;

            cachedSpot = spot ? spot : MapManager.Instance.MouseSpot;

            var isBlocked = Physics2D.OverlapBox(cachedSpot.Position, new(Configs.CellSize, Configs.CellSize), 0f, blockerLayer);
            canBuild = isBlocked || !EnemyManager.Instance.CanEnemiesReachBase(spot)
                ? false : cachedTower.CanBuild(cachedSpot);

            var color = canBuild ? Color.white : Color.red;
            color.a = spriteRenderer.color.a;
            spriteRenderer.color = color;

            moveTween.Kill();
            moveTween = transform.DOMove(cachedSpot.Position, moveAnimDuration).SetEase(Ease.OutQuad);
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
            CancelPreview();
        }

        private void OnDrawGizmos() {
            if (cachedTower) {
                Gizmos.DrawWireSphere(transform.position, cachedTower.Config.AttackRange);
            }
        }
        #endregion
    }
}
