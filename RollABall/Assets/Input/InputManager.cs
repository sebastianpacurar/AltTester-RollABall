using UnityEngine;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;

namespace Input {
    public class InputManager : MonoBehaviour {
        public static InputManager Instance { get; private set; }
        public Vector2 MoveInput { get; private set; }
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

        private void OnMove(InputAction.CallbackContext ctx) {
            MoveInput = ctx.phase switch {
                InputActionPhase.Started or InputActionPhase.Performed => ctx.ReadValue<Vector2>(),
                InputActionPhase.Canceled => Vector2.zero,
                _ => Vector2.zero,
            };
        }

        private void OnLook(InputAction.CallbackContext ctx) => MoveInput = _controls.Player.Look.ReadValue<Vector2>();
        private void OnPause(InputAction.CallbackContext ctx) => IsPaused = !IsPaused;

        private void OnEnable() {
            _controls.Player.Look.Enable();
            _controls.Player.Move.Enable();
            _controls.UI.Pause.Enable();

            _controls.Player.Look.performed += OnLook;
            _controls.Player.Move.performed += OnMove;
            _controls.Player.Move.canceled += OnMove;
            _controls.UI.Pause.performed += OnPause;
        }

        private void OnDisable() {
            _controls.Player.Look.performed -= OnLook;
            _controls.Player.Move.performed -= OnMove;
            _controls.Player.Move.canceled -= OnMove;
            _controls.UI.Pause.performed -= OnPause;

            _controls.Player.Look.Disable();
            _controls.Player.Move.Disable();
            _controls.UI.Pause.Disable();
        }
    }
}