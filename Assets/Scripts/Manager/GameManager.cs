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
            None,
            Preparation,
            Combat,
            GameOver,
            Win,
        }

        #region Fields
        [MandatoryFields]
        [SerializeField] private TutorialConfig tutorialConfig;
        [SerializeField] private LevelConfig levelConfig;

        [ComputedFields]
        [SerializeField] private State state;
        [SerializeField] private int level;
        [SerializeField] private int tutorialLevel;

        [SerializeField] private bool doTutorial;
        [SerializeField] private bool isInTutorial;

        [SerializeField] private float gameTime;
        [SerializeField] private int frameRate = 144;
        [SerializeField] private bool vsync = true;
        #endregion

        #region Publics
        public event Action OnPreparationStarted;
        public event Action OnCombatStarted;
        public event Action OnGameWon;
        public event Action OnGameOver;

        public event Action<int> OnTutorialStarted;
        public event Action OnTutorialEnded;

        public event Action OnCurrentSceneExiting;

        public State GameState => state;
        public int Level => level;
        public float GameTime => gameTime;
        public bool IsInTutorial => isInTutorial;
        public bool DoTutorial => doTutorial;
        public int TutorialLevel => tutorialLevel;

        /// <summary>
        /// Load into a scene. 
        /// </summary>
        /// <param name="profileIndex">Profile data index. </param>
        public void LoadScene(int sceneIndex) {
            OnCurrentSceneExiting?.Invoke();
            SceneManager.LoadScene(sceneIndex);
        }

        public void SetTutorial(bool doTutorial, int tutorialLevel = 0) {
            this.doTutorial = doTutorial;
            this.tutorialLevel = tutorialLevel;
        }

        public void StartCombat() {
            state = State.Combat;
            OnCombatStarted?.Invoke();
        }

        public LevelInfo GetCurrentLevelInfo() {
            if (isInTutorial) {
                return tutorialConfig.TutorialInfos[tutorialLevel].LevelInfo;
            }
            else {
                return levelConfig.LevelInfos[level];
            }
        }
        #endregion

        #region Internals
        private void StartPreparation() {
            if (level >= levelConfig.NumLevels) {
                state = State.Win;
                OnGameWon?.Invoke();
                return;
            }

            if (isInTutorial) {
                if (tutorialLevel >= tutorialConfig.TutorialInfos.Length) {
                    isInTutorial = false;
                    Player.Instance.EndTutorial();
                    CardManager.Instance.EndTutorial();
                    OnTutorialEnded?.Invoke();
                }
                else {
                    OnTutorialStarted?.Invoke(tutorialLevel);
                }
            }

            state = State.Preparation;
            OnPreparationStarted?.Invoke();

            if (isInTutorial) {
                Player.Instance.LoadTutorial(tutorialConfig.TutorialInfos[tutorialLevel]);
                CardManager.Instance.LoadTutorial(tutorialConfig.TutorialInfos[tutorialLevel]);
                tutorialLevel++;
            }
            else {
                level++;
            }
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
            if (doTutorial) {
                doTutorial = false;
                isInTutorial = true;
            }
            StartPreparation();
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
            switch (scene.buildIndex) {
                case Configs.MainMenuSceneIndex:
                    InputManager.Instance.Toggle(InputMode.General, false);
                    state = State.None;
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
            doTutorial = false;
            isInTutorial = false;
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
