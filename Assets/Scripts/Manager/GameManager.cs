using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;
using System;

namespace CSCI526GameJam {

    /// <summary>
    /// Manage game state. 
    /// </summary>

    public class GameManager : MonoBehaviourSingleton<GameManager> {

        public static bool IsApplicationQuitting { get; private set; } = false;

        public enum State {
            Buying,
            Building,
            Fighting,
        }

        #region Fields
        [ComputedFields]
        [SerializeField] private State state;
        [SerializeField] private int level;

        [SerializeField] private float gameTime;
        [SerializeField] private int frameRate = 144;
        [SerializeField] private bool vsync = true;
        #endregion

        #region Publics
        public event Action OnPreparationStarted;
        public event Action OnCombatStarted;

        public State GameState => state;
        public int Level => level;
        public float GameTime => gameTime;

        /// <summary>
        /// Load into the gameplay scene. 
        /// </summary>
        /// <param name="profileIndex">Profile data index. </param>
        public void LoadGameplayScene(int profileIndex) {
            //SaveManager.Instance.SelectProfile(profileIndex);
            SceneManager.LoadScene(Configs.GameplaySceneIndex);
        }
        #endregion

        #region Internals
        private void StartBuying() {
            state = State.Buying;
            level++;
            OnPreparationStarted?.Invoke();
        }

        private void StartCombat() {
            state = State.Fighting;
            OnCombatStarted?.Invoke();
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
            switch (scene.buildIndex) {
                case Configs.MainMenuSceneIndex:
                    InputManager.Instance.Toggle(InputMode.General, false);
                    break;

                case Configs.GameplaySceneIndex:
                    SetupGameplay();
                    break;
            }

            Debug.Log($"Loaded scene (Mode: {mode}): {scene.name}. ");
        }

        private void SetupGameplay() {
            InputManager.Instance.Toggle(true);
            MapManager.Instance.GenerateMap();
            CameraManager.Instance.Init();
            TowerManager.Instance.GenerateBase();
        }

        private void OnSceneUnloaded(Scene scene) {
            switch (scene.buildIndex) {
                case Configs.GameplaySceneIndex:
                    CleanupGameplay();
                    break;
            }

            Debug.Log($"Unloaded scene: {scene.name}. ");
        }

        private void CleanupGameplay() {
            //Destroy(Player.Instance.gameObject);
            //Destroy(InputManager.Instance.gameObject);
        }
        #endregion

        #region Unity Methods
        protected override void Awake() {
            base.Awake();

            //DontDestroyOnLoad(gameObject);
            //Cursor.lockState = CursorLockMode.Confined;

            Application.targetFrameRate = Mathf.Max(0, frameRate);
            QualitySettings.vSyncCount = vsync ? 1 : 0;
        }

        private void OnEnable() {
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
            EnemyManager.Instance.OnEnemiesClear += StartBuying;
        }

        private void OnDisable() {
            if (!IsApplicationQuitting) {
                SceneManager.sceneLoaded -= OnSceneLoaded;
                SceneManager.sceneUnloaded -= OnSceneUnloaded;
                EnemyManager.Instance.OnEnemiesClear -= StartBuying;
            }
        }

        private void Start() {
            StartBuying();
        }

        private void Update() {
            gameTime = Time.time;
        }

        private void OnApplicationQuit() {
            IsApplicationQuitting = true;
        }
        #endregion
    }
}