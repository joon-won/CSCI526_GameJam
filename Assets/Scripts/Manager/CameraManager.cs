using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CSCI526GameJam {

    /// <summary>
    /// Manage cameras. 
    /// </summary>

    public class CameraManager : MonoBehaviourSingleton<CameraManager> {

        #region Fields
        [MandatoryFields]
        [SerializeField] private Camera uiCamera;

        [SerializeField] private Transform followedTarget;
        [SerializeField] private Vector3 defaultPosition;

        [ComputedFields]
        [SerializeField] private bool isFree = false;

        private CameraZoomer cameraZoomer;
        private CameraDragger cameraDragger;
        #endregion

        #region Publics
        public Camera UICamera => uiCamera;

        /// <summary>
        /// Init states of cameras. 
        /// </summary>
        public void Init() {
            var camera = Camera.main;

            var mapCenter = MapManager.Instance.MapCenter;
            var z = Camera.main.transform.position.z;
            camera.transform.position = new(mapCenter.x, mapCenter.y, z);

            //var rectHeight = MapManager.Instance.MapSize * Configs.CellSize;
            //var aspectRatio = camera.aspect;
            //camera.orthographicSize = (rectHeight / 2) / aspectRatio;
        }

        /// <summary>
        /// Toggle camera to free or not. 
        /// </summary>
        /// <param name="isFree">If to free. </param>
        public void ToggleFreeCamera(bool isFree) {
            this.isFree = isFree;
        }
        #endregion

        #region Internals
        private void FollowTarget() {
            if (!followedTarget) return;

            Camera.main.transform.position = new Vector3(followedTarget.position.x, followedTarget.position.y, Camera.main.transform.position.z);
        }

        private void DragCamera(bool enabled) {
            if (!isFree) return;
            cameraDragger.ToggleDragToMoveCamera(enabled);
        }

        private void ZoomCamera(float value) {
            var isZoomIn = true;
            if (value < 0)
                isZoomIn = false;

            cameraZoomer.ZoomCamera(isZoomIn);
        }

        #endregion

        #region Unity Methods
        protected override void Awake() {
            base.Awake();
            cameraZoomer = GetComponent<CameraZoomer>();
            cameraDragger = GetComponent<CameraDragger>();

            Camera.main.transform.position = defaultPosition;
            ToggleFreeCamera(true);
        }

        private void OnEnable() {
            InputManager.Instance.OnCameraMoveInputChanged += DragCamera;
            InputManager.Instance.OnCameraZoomInputChanged += ZoomCamera;
        }

        private void OnDisable() {
            if (!GameManager.IsApplicationQuitting) {
                InputManager.Instance.OnCameraMoveInputChanged -= DragCamera;
                InputManager.Instance.OnCameraZoomInputChanged -= ZoomCamera;
            }
        }

        private void Update() {
            if (isFree) {

            }
            else {
                FollowTarget();
            }
        }
        #endregion
    }
}