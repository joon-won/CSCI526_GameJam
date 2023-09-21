using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;

namespace CSCI526GameJam {

    /// <summary>
    /// Manage game state. 
    /// </summary>

    public class GameManager : MonoBehaviourSingleton<GameManager> {

        public static bool IsApplicationQuitting { get; private set; } = false;

        #region Fields
        [ComputedFields]
        [SerializeField] private float gameTime;
        [SerializeField] private int frameRate = 144;
        [SerializeField] private bool vsync = true;
        #endregion

        #region Publics
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
        }

        private void OnDisable() {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }

        private void Start() {
            //InputManager.Instance.ToggleInput(InputMode.General, true);
            //InputManager.Instance.ToggleInput(InputMode.Gameplay, false);
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