using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CSCI526GameJam {
    public class CameraDragger : MonoBehaviour {
        [SerializeField] private float dragSpeed = 30f;
        [SerializeField] private bool isDragging = false;
        [SerializeField] private float dragSmoothTime = 2f;
        [SerializeField] private float inertiaMagnitude = 1f;

        private Vector3 origin;
        private Vector3 delta;
        private Vector3 prev;

        private Vector3 velocity;
        private bool underInertia;
        private float time = 0.0f;

        private void LateUpdate() {
            UpdateCameraPosition();
        }

        public void ToggleDragToMoveCamera(bool startDragging) {
            isDragging = startDragging;

            if (startDragging) {
                origin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                prev = origin;
                underInertia = false;
            }
            else {
                underInertia = true;
            }
        }

        private void UpdateCameraPosition() {
            if (isDragging) {
                delta = origin - Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Camera.main.transform.position += delta * dragSpeed * Time.deltaTime;
                velocity = (Camera.main.transform.position - prev) / inertiaMagnitude;
                prev = Camera.main.transform.position;
            }

            if (underInertia && time <= dragSmoothTime) {
                Camera.main.transform.position += velocity;
                velocity = Vector3.Lerp(velocity, Vector3.zero, time / dragSmoothTime);
                time += Time.smoothDeltaTime;
            }
            else {
                underInertia = false;
                time = 0f;
            }
        }

    }
}