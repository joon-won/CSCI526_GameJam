using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using UnityEngine.EventSystems;

namespace CSCI526GameJam {

    public enum InputMode {
        General,
        Gameplay,
    }

    public class InputManager : MonoBehaviourSingleton<InputManager> {

        public static bool IsMouseOverUI { get; private set; }

        #region Fields
        [ComputedFields]
        [SerializeField] private bool isMouseLeftPressed = false;
        [SerializeField] private bool isShiftPressed = false;
        [SerializeField] private bool isControlPressed = false;
        [SerializeField] private bool isAltPressed = false;
        private InputActions inputActions;
        #endregion

        #region Publics
        public event Action OnMouseLeftDown;
        public event Action OnMouseLeftUp;
        public event Action OnMouseRightDown;

        public event Action<int> OnNumberDown;
        public event Action<int> OnFunctionKeyDown;

        public event Action OnEscapeDown;

        public event Action<Vector2> OnMoveInputChanged;
        public event Action OnDemolishKeyDown;

        public event Action<bool> OnCameraMoveInputChanged;
        public event Action<float> OnCameraZoomInputChanged;

        public bool IsMouseLeftPressed => isMouseLeftPressed;
        public bool IsShiftPressed => isShiftPressed;
        public bool IsControlPressed => isControlPressed;
        public bool IsAltPressed => isAltPressed;

        /// <summary>
        /// Toggle input. 
        /// </summary>
        /// <param name="isEnabled">If to enable. </param>
        public void Toggle(bool isEnabled) {
            if (inputActions == null) return;

            if (isEnabled) {
                inputActions.Enable();
            }
            else  {
                inputActions.Disable();
            }
        }

        /// <summary>
        /// Toggle a input mode. 
        /// </summary>
        /// <param name="mode">Mode to toggle. </param>
        /// <param name="isEnabled">If to enable. </param>
        public void Toggle(InputMode mode, bool isEnabled) {
            InputActionMap targetActions = null;

            switch (mode) {
                case InputMode.General:
                    targetActions = inputActions.General;
                    break;

                case InputMode.Gameplay:
                    targetActions = inputActions.Gameplay;
                    break;
            }

            if (targetActions != null) {
                if (isEnabled) targetActions.Enable();
                else targetActions.Disable();
            }
        }

        /// <summary>
        /// Get mouse's world position. 
        /// </summary>
        /// <returns>Mouse's world position. </returns>
        public Vector3 GetMousePositionInWorld() {
            return Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        }
        #endregion

        #region Internals
        private void OnMouseLeftDown_performed() {
            isMouseLeftPressed = true;
            //if (UIManager.IsMouseOverUI) return;

            OnMouseLeftDown?.Invoke();
        }

        private void OnMouseLeftUp_performed() {
            isMouseLeftPressed = false;
            OnMouseLeftUp?.Invoke();
        }

        private void OnMouseRightDown_performed() {
            //if (UIManager.IsMouseOverUI) return;

            OnMouseRightDown?.Invoke();
        }
        #endregion

        #region Unity Methods
        protected override void Awake() {
            base.Awake();
            if (inputActions == null) {
                inputActions = new InputActions();
            }
        }

        private void Start() {

            // General inputs. 
            inputActions.General.MouseLeft.performed += ctx => OnMouseLeftDown_performed();
            inputActions.General.MouseLeft.canceled += ctx => OnMouseLeftUp_performed();

            inputActions.General.MouseRight.performed += ctx => OnMouseRightDown_performed();

            inputActions.General.Move.performed += ctx => OnMoveInputChanged?.Invoke(ctx.ReadValue<Vector2>());
            inputActions.General.Move.canceled += ctx => OnMoveInputChanged?.Invoke(ctx.ReadValue<Vector2>());

            inputActions.General.Escape.performed += ctx => OnEscapeDown?.Invoke();

            inputActions.General.MoveCamera.performed += ctx => OnCameraMoveInputChanged?.Invoke(true);
            inputActions.General.MoveCamera.canceled += ctx => OnCameraMoveInputChanged?.Invoke(false);

            inputActions.General.ZoomCamera.performed += ctx => OnCameraZoomInputChanged?.Invoke(ctx.ReadValue<float>());


            // Gameplay inputs. 
            inputActions.Gameplay.Shift.performed += ctx => isShiftPressed = true;
            inputActions.Gameplay.Shift.canceled += ctx => isShiftPressed = false;
            inputActions.Gameplay.Control.performed += ctx => isControlPressed = true;
            inputActions.Gameplay.Control.canceled += ctx => isControlPressed = false;
            inputActions.Gameplay.Alt.performed += ctx => isAltPressed = true;
            inputActions.Gameplay.Alt.canceled += ctx => isAltPressed = false;

            //inputActions.Gameplay.Rotate.performed += ctx => OnRotateKeyDown?.Invoke();
            //inputActions.Gameplay.Repair.performed += ctx => OnRepairKeyDown?.Invoke();
            inputActions.Gameplay.Demolish.performed += ctx => OnDemolishKeyDown?.Invoke();

            //inputActions.Gameplay.Inventory.performed += ctx => OnInventoryKeyDown?.Invoke();
            //inputActions.Gameplay.Map.performed += ctx => OnMapKeyDown?.Invoke();

            inputActions.Gameplay.Number_1.performed += ctx => OnNumberDown?.Invoke(1);
            inputActions.Gameplay.Number_2.performed += ctx => OnNumberDown?.Invoke(2);
            inputActions.Gameplay.Number_3.performed += ctx => OnNumberDown?.Invoke(3);
            inputActions.Gameplay.Number_4.performed += ctx => OnNumberDown?.Invoke(4);
            inputActions.Gameplay.Number_5.performed += ctx => OnNumberDown?.Invoke(5);
            inputActions.Gameplay.Number_6.performed += ctx => OnNumberDown?.Invoke(6);
            inputActions.Gameplay.F1.performed += ctx => OnFunctionKeyDown?.Invoke(1);
            inputActions.Gameplay.F2.performed += ctx => OnFunctionKeyDown?.Invoke(2);
            inputActions.Gameplay.F3.performed += ctx => OnFunctionKeyDown?.Invoke(3);
            inputActions.Gameplay.F4.performed += ctx => OnFunctionKeyDown?.Invoke(4);
            inputActions.Gameplay.F5.performed += ctx => OnFunctionKeyDown?.Invoke(5);
            inputActions.Gameplay.F6.performed += ctx => OnFunctionKeyDown?.Invoke(6);
            inputActions.Gameplay.F7.performed += ctx => OnFunctionKeyDown?.Invoke(7);
            inputActions.Gameplay.F8.performed += ctx => OnFunctionKeyDown?.Invoke(8);
            inputActions.Gameplay.F9.performed += ctx => OnFunctionKeyDown?.Invoke(9);
        }

        private void OnEnable() {
            Toggle(true);
        }

        private void OnDisable() {
            Toggle(false);
        }

        private void Update() {
            IsMouseOverUI = EventSystem.current.IsPointerOverGameObject();
        }
        #endregion
    }
}