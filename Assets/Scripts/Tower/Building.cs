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
        [SerializeReference] private Numeric maxHealth;
        [SerializeReference] private Numeric armor;
        [SerializeReference] private Numeric tenacity;

        [SerializeField] private float shield;
        [SerializeReference] private Numeric maxShield;

        [SerializeField] protected bool isConstructed = false;
        [SerializeField] protected bool isRunning = false;

        [SerializeField] protected Sprite previewImage;
        [SerializeField] protected Sprite regularImage;

        [SerializeField] protected BuildingEntity entity;

        private Coroutine constructionRoutine;
        private Coroutine repairRoutine;
        #endregion

        #region Public
        public Action OnConstruct;
        public Action OnBuild;
        public Action OnDemolish;

        public BuildingConfig Config => config;
        public string BuildingName => config.name;
        public Sprite Preview => previewImage;
        public Sprite Image => regularImage;
        public bool InstantBuild => config.InstantBuild;

        public float ConstructionRange => config.ConstructionRange;
        public float EnergyCost => config.EnergyCost;

        public Vector2Int Shape => entity.Shape;
        public Vector2Int Pivot => entity.Pivot;
        public Direction Direction => entity.Direction;
        public Area Area => entity.Area;

        public bool IsSettled => entity.IsSettled;
        public bool IsConstructing => constructionRoutine != null;
        public bool IsRepairing => repairRoutine != null;

        public override Numeric MaxHealth => maxHealth;
        public override Numeric Armor => armor;
        public override Numeric Tenacity => tenacity;

        public float Shield => shield;
        public Numeric MaxShield => maxShield;

        public BuildingRepairer Repairer => repairer;
        public override Collider2D Collider2D => entity.Collider2D;

        /// <summary>
        /// Check if can build on the area.
        /// </summary>
        public bool CanBuild(Area area) {
            if (area.Width != Shape.x || area.Height != Shape.y) {
                Debug.LogWarning($"Target area shape does not match building shape. ");
                return false;
            }

            foreach (var index in area.Matrix) {
                var spot = index.GetSpot();
                if (!spot) return false;
                if (!spot.IsVitalized || !CheckCanBuild(spot)) {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Run the building.
        /// </summary>
        public virtual void Run() {
            isRunning = true;
        }

        /// <summary>
        /// Pause the building.
        /// </summary>
        public virtual void Pause() {
            isRunning = false;
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

        public virtual void Repair() {
            if (repairRoutine != null) return; // Check if already repairing
            repairRoutine = StartCoroutine(RepairRoutine());
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

        public override void HitBack(Vector3 from, float impact) {

        }

        /// <summary>
        /// Rotate to a specific direction.
        /// </summary>
        /// <param name="direction"></param>
        public virtual void RotateTo(Direction direction) {
            if (direction == Direction.None) {
                direction = Direction.Top;
            }

            while (Direction != direction) {
                Rotate90(true);
            }
        }

        /// <summary>
        /// Rotate to next direction.
        /// </summary>
        /// <param name="isClockwise"></param>
        public virtual void Rotate90(bool isClockwise) {
            entity.Rotate90(isClockwise);
        }

        public virtual void OnSelect() {

        }

        public virtual void OnUnselect() {

        }

        public virtual void OnHover() {
            frameIndicator.Show();
        }

        public virtual void OnUnhover() {
            frameIndicator.Hide();
        }


        public virtual BuildingData SaveData() {
            return new(this);
        }

        public void ReadData(BuildingData save) {
            OnDataRead(save);
            OnInfoChange?.Invoke();
        }
        #endregion

        #region Internal
        /// <summary>
        /// Perform building's functionalities.
        /// </summary>
        protected abstract void PerformUpdate();

        protected virtual void OnDataRead(BuildingData data) {
            RotateTo(data.Direction);

            var area = new Area(data.Config, data.Index.GetSpot());
            Construct(area, !data.IsConstructing);
            health = data.Health;

            if (data.IsRepairing) {
                repairer.ReadData(data.RepairerData);
                Repair();
            }
        }

        protected override void InitNumerics() {
            var stats = Stats.Instance.Building;

            maxHealth = new(config.MaxHealth);
            maxHealth.AddNumericSet(stats.MaxHealth);

            armor = new(config.Armor);
            armor.AddNumericSet(stats.Armor);

            tenacity = new(config.Tenacity);
            tenacity.AddNumericSet(stats.Tenacity);

            maxShield = new(0f);
            maxShield.AddNumericSet(stats.MaxShield);
        }

        protected virtual bool CheckCanBuild(Spot spot) {
            if (spot.Building) return false;
            return true;
        }

        protected void SetSpotsVitalityInRange(bool vitalize) {
            var radius = ConstructionRange * Configs.CellSize;
            var center = Area.Matrix[Pivot.x, Pivot.y];
            var indices = Utility.GetFilledCircleIndices(center, radius);
            MapManager.Instance.SetVitalized(indices, vitalize);

            // TODO: To be changed.
            MapManager.Instance.SetRevealed(indices, vitalize);
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

        private IEnumerator ConstructionRoutine() {
            var speed = Stats.Instance.Player.ConstructSpeed;
            if (speed > 0f) {
                health = 0f;
                while (health < MaxHealth) {
                    Heal(speed * Time.deltaTime);
                    yield return null;
                }
            }
            constructionRoutine = null;
            Build();
        }

        /// <summary>
        /// Consume repair material and keep repairing until health is full. 
        /// </summary>
        private IEnumerator RepairRoutine() {

            // Repair until health is full. 
            while (!IsFullHealth) {
                var distance = Player.Instance.transform.position.DistanceTo(Collider2D);

                // If out of range, pause until player is close enough. 
                if (distance > Stats.Instance.Player.RepairRange) {

                }
                // Perform repairing. 
                else {
                    // Stop if repairer is complete. 
                    if (!repairer.TryPerform()) break;
                }
                yield return null;
            }
            repairRoutine = null;
        }

        protected override void Die() {
            Demolish();
        }

        protected override float ResolveTakenDamage(float damage, float penetration) {
            var finalDamage = base.ResolveTakenDamage(damage, penetration);
            var shieldTaken = Mathf.Min(shield, finalDamage);
            shield -= shieldTaken;

            return finalDamage - shieldTaken;
        }
        #endregion

        #region Unity Methods
        protected override void Awake() {
            base.Awake();

            isRunning = false;

            previewImage = config.Preview;
            regularImage = config.Image;

            entity = GetComponentInChildren<BuildingEntity>();
            entity.Init(this);

            repairer = new(this);

            frameIndicator = GetComponentInChildren<BuildingFrameIndicator>();
            if (!frameIndicator) {
                Debug.LogError($"Cannot find {typeof(BuildingFrameIndicator)} in {name}. ");
            }

            minimapUnit = GetComponentInChildren<MinimapUnit>();
            if (!minimapUnit) {
                Debug.LogError($"Cannot find {typeof(MinimapUnit)} in {name}. ");
            }
            minimapUnit.SetSize(Shape);
            minimapUnit.SetColor(config.MinimapUnitColor);

            var componentsHolder = new GameObject("Components");
            componentsHolder.transform.SetParent(transform);

            var armorPerMissingHealth = componentsHolder.AddComponent<ArmorPerMissingHealth>();
        }

        protected override void Start() {
            base.Start();
        }

        protected override void Update() {
            base.Update();

            if (isRunning) {
                PerformUpdate();
            }
        }
        #endregion
    }
}
