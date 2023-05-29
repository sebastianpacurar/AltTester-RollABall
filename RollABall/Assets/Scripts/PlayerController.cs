using Input;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour {
    // Create public variables for player speed, and for the Text UI game objects
    public float speed = 0;
    public TextMeshProUGUI countText;
    public GameObject winTextObject;
    public Vector3 previousAcceleration = Vector3.zero;

    private Rigidbody _rb;
    private int _count;

    private InputManager _inputManager;
    private Vector2 _moveVal;

    private void Awake() {
        _rb = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    private void Start() {
        _inputManager = InputManager.Instance;
        SetCountText();
        winTextObject.SetActive(false);
    }

    private void SetCountText() {
        countText.text = $"Count: {_count}";

        if (_count >= 12) {
            // Set the text value of your 'winText'
            winTextObject.SetActive(true);
        }
    }

    private void Update() => _moveVal = !_inputManager.IsPaused ? _inputManager.LookInput : Vector2.zero;

    private void FixedUpdate() => ApplyPhysics();

    private void ApplyPhysics() {
        _rb.AddForce(new Vector3(_moveVal.x, 0.0f, _moveVal.y) * speed);
        _rb.drag = _inputManager.IsPaused ? 2.5f : 0f;
        _rb.angularDrag = _inputManager.IsPaused ? 2.5f : 0.05f;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("PickUp")) {
            Destroy(other.gameObject);
            _count++;
            SetCountText();
        }
    }
}