using System;
using System.Linq;
using Input;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour {
    // Create public variables for player speed, and for the Text UI game objects
    public float speed = 0;
    public TextMeshProUGUI countText;
    public GameObject winTextObject;
    // public Vector3 previousAcceleration = Vector3.zero;

    private Rigidbody _rb;
    private int _count;

    private InputManager _inputManager;
    [SerializeField] private Vector2 moveVal;
    [SerializeField] private Vector2 direction;
    [SerializeField] private Vector2 velocity;
    [SerializeField] private Vector2 angVelocity;
    [SerializeField] private float dirDotVel;
    [SerializeField] private float dirDotAngVel;

    private GameObject[] _debugItems;
    private Vector3 _activeItem;

    private void Awake() {
        _rb = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    private void Start() {
        _inputManager = InputManager.Instance;
        _debugItems = GameObject.FindGameObjectsWithTag("PickUp");
        SetActiveTarget();

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

    private void Update() {
        DebugData();
        SetDirectionToActive();
        moveVal = !_inputManager.IsPaused ? _inputManager.MoveInput : Vector2.zero;
    }

    private void FixedUpdate() => ApplyPhysics();

    private void ApplyPhysics() {
        _rb.AddForce(new Vector3(moveVal.x, 0.0f, moveVal.y) * speed);
        _rb.drag = _inputManager.IsPaused ? 2.5f : 0f;
        _rb.angularDrag = _inputManager.IsPaused ? 2.5f : 0.15f;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("PickUp")) {
            other.gameObject.SetActive(false);
            _count++;
            SetCountText();
            SetActiveTarget();
        }
    }

    private void SetActiveTarget() {
        try {
            _activeItem = _debugItems.Where(i => i.activeInHierarchy).ToArray()[0].transform.position;
        } catch (Exception) {
            _activeItem = transform.position;
        }
    }

    private void SetDirectionToActive() {
        var v = (_activeItem - transform.position).normalized;
        direction = new Vector2(v.x, v.z);
    }

    private void OnDrawGizmos() {
        var pos = transform.position;

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(pos, pos + new Vector3(direction.x, 0f, direction.y) * 3f);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(pos, pos + _rb.velocity.normalized * 3f);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(pos, pos + _rb.angularVelocity.normalized * 3f);
    }

    private void DebugData() {
        velocity = new Vector2(_rb.velocity.x, _rb.velocity.z).normalized;
        angVelocity = new Vector2(_rb.angularVelocity.x, _rb.angularVelocity.z).normalized;

        dirDotVel = Vector2.Dot(velocity, direction);
        dirDotAngVel = Vector2.Dot(angVelocity, direction);
    }
}