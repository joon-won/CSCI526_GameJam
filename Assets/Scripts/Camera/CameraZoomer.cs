using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CSCI526GameJam {
    public class CameraZoomer : MonoBehaviour {
        [SerializeField] private float minSize = 5f;
        [SerializeField] private float maxSize = 15f;
        [SerializeField] private float defaultSize;
        [SerializeField] private float zoomMagnitude = 1f;
        [SerializeField] private float zoomSmoothTime = 0.5f;

        private float currentSize;
        private float startSize;
        private float targetSize;
        private float elapsed = 0f;

        private void Awake() {
            elapsed = zoomSmoothTime;
            Camera.main.orthographicSize = defaultSize;
            targetSize = Camera.main.orthographicSize;
        }

        private void Update() {
            if (elapsed >= zoomSmoothTime)
                return;

            elapsed += Time.deltaTime;
            Camera.main.orthographicSize = Mathf.Lerp(currentSize, targetSize, elapsed / zoomSmoothTime);
        }

        public void ZoomCamera(bool isZoomIn) {
            elapsed = 0f;

            currentSize = Camera.main.orthographicSize;
            startSize = targetSize;

            if (isZoomIn) {
                targetSize = startSize - zoomMagnitude;
            }
            else {
                targetSize = startSize + zoomMagnitude;
            }
            targetSize = Mathf.Clamp(targetSize, minSize, maxSize);
        }
    }
}