using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CSCI526GameJam {

    public abstract class Building : MonoBehaviour {

        #region Fields
        [ClassHeader(typeof(Building))]

        [MandatoryFields]
        [SerializeField] protected BuildingConfig config;

        [ComputedFields]
        [SerializeField] protected Sprite previewImage;
        [SerializeField] protected Sprite regularImage;

        [SerializeField] protected BuildingEntity entity;
        #endregion

        #region Public
        public Action OnBuild;
        public Action OnDemolish;

        public BuildingConfig Config => config;
        public string BuildingName => config.name;
        public Sprite Preview => previewImage;
        public Sprite Image => regularImage;

        public Spot Spot => entity.Spot;

        public Collider2D Collider2D => entity.Collider2D;

        /// <summary>
        /// Check if can build on the area.
        /// </summary>
        public bool CanBuild(Spot spot) {
            return !spot.Building;
        }

        public virtual void Construct(Area area, bool forceInstant = false) {
            if (!area) {
                Debug.LogError($"{GetType()}: area cannot be null. ");
                return;
            }
            if (isConstructed) {
                Debug.LogWarning($"{name} is already constructed. ");
                return;
            }

            gameObject.SetActive(true);
            isConstructed = false;
            isRunning = false;

            entity.Settle(area);
            transform.position = area.CenterPosition;

            // If instant build, skip construction.
            if (InstantBuild || forceInstant) {
                Build();
            }
            // Start construction. 
            else {
                if (constructionRoutine != null) {
                    Debug.LogWarning("Construction routine should not be called twice. ");
                    return;
                }
                constructionRoutine = StartCoroutine(ConstructionRoutine());
            }

            OnConstruct?.Invoke();
        }

        /// <summary>
        /// Demolish the building. If refund is needed, call ConstructionManager.Refund(). 
        /// </summary>
        public virtual void Demolish() {
            if (isConstructed) {
                SetSpotsVitalityInRange(false);
            }

            isConstructed = false;
            entity.Unsettle();

            Destroy(gameObject);

            OnDemolish?.Invoke();
        }
        #endregion

        #region Internal
        /// <summary>
        /// Perform building's functionalities.
        /// </summary>
        protected abstract void PerformUpdate();

        protected override void InitNumerics() {

        }

        /// <summary>
        /// Build the building on given Area.
        /// </summary>
        protected virtual void Build() {
            isConstructed = true;
            isRunning = true;
            health = MaxHealth;

            // extend construction range
            SetSpotsVitalityInRange(true);
            OnBuild?.Invoke();
        }
        #endregion

        #region Unity Methods
        protected override void Awake() {
            previewImage = config.Preview;
            regularImage = config.Image;

            entity = GetComponentInChildren<BuildingEntity>();
            entity.Init(this);
        }

        protected override void Update() {
            PerformUpdate();
        }
        #endregion
    }
}
