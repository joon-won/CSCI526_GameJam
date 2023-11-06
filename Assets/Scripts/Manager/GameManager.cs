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

        public enum State {
            Preparation,
            Combat,
            GameOver,
            Win,
        }

        #region Fields
        [MandatoryFields]
        [SerializeField] private int numLevels = 3;
        [SerializeField] private TutorialConfig tutorialConfig;

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
        public event Action OnGameWon;
        public event Action OnGameOver;
        public event Action OnTutorialStarted;

        public event Action OnCurrentSceneExiting;

        public State GameState => state;
        public int Level => level;
        public float GameTime => gameTime;

        /// <summary>
        /// Load into the gameplay scene. 
        /// </summary>
        /// <param name="profileIndex">Profile data index. </param>
        public void LoadGameplayScene() {
            OnCurrentSceneExiting?.Invoke();
            SceneManager.LoadScene(Configs.GameplaySceneIndex);
        }

        public void StartCombat() {
            state = State.Combat;
            OnCombatStarted?.Invoke();
        }

        [ContextMenu("Start Tutorial")]
        public void StartTutorial() {
            Player.Instance.LoadTutorial(tutorialConfig);
            CardManager.Instance.LoadTutorial(tutorialConfig);
            OnTutorialStarted?.Invoke();
        }
        #endregion

        #region Internals
        private void StartPreparation() {
            if (level >= numLevels) {
                state = State.Win;
                OnGameWon?.Invoke();
                return;
            }
            state = State.Preparation;
            level++;
            OnPreparationStarted?.Invoke();
        }

        private void SetGameOver() {
            state = State.GameOver;
            OnGameOver?.Invoke();
        }

        private void SetupGameplay() {
            InputManager.Instance.Toggle(true);
            MapManager.Instance.GenerateMap();
            CameraManager.Instance.Init();
            TowerManager.Instance.GenerateBase();

            TowerManager.Instance.PlayerBase.OnDied += SetGameOver;
            EnemyManager.Instance.OnEnemiesClear += StartPreparation;

            level = 0;
            StartPreparation();
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

        private void OnSceneUnloaded(Scene scene) {
            switch (scene.buildIndex) {
                case Configs.GameplaySceneIndex:
                    CleanupGameplay();
                    break;
            }
            OnCurrentSceneExiting = null;

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

            DontDestroyOnLoad(gameObject);
            //Cursor.lockState = CursorLockMode.Confined;

            Application.targetFrameRate = Mathf.Max(0, frameRate);
            QualitySettings.vSyncCount = vsync ? 1 : 0;
        }

        private void OnEnable() {
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        private void OnDisable() {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }

        private void Update() {
            gameTime = Time.time;
        }
        #endregion
    }
}