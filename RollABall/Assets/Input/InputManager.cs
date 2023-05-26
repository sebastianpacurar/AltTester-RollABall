using UnityEngine;
using UnityEngine.InputSystem;

namespace Input {
    public class InputManager : MonoBehaviour {
        public static InputManager Instance { get; private set; }
        public Vector2 LookInput { get; private set; }
        public bool IsPaused { get; private set; }

        private InputActions _controls;


        private void Awake() {
            if (Instance != null && Instance != this) {
                Destroy(gameObject);
            } else {
                Instance = this;
            }

            _controls = new InputActions();
        }

        private void OnLook(InputAction.CallbackContext ctx) => LookInput = _controls.Player.Look.ReadValue<Vector2>();
        private void OnPause(InputAction.CallbackContext ctx) => IsPaused = !IsPaused;

        private void OnEnable() {
            _controls.Player.Look.Enable();
            _controls.UI.Pause.Enable();

            _controls.Player.Look.performed += OnLook;
            _controls.UI.Pause.performed += OnPause;
        }

        private void OnDisable() {
            _controls.Player.Look.performed -= OnLook;
            _controls.UI.Pause.performed -= OnPause;

            _controls.Player.Look.Disable();
            _controls.UI.Pause.Disable();
        }
    }
}