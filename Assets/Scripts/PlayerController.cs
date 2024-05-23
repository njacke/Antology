using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _baseMoveSpeed = 6f;
    [SerializeField] private float _dashSpeed = 20f;
    [SerializeField] private float _armorSpeedReduction = .5f;
    [SerializeField] private float _dashDuration = .1f;
    [SerializeField] private float _dashCooldown = 3f;
    [SerializeField] private float _rotationGracePeriod = .05f;
    [SerializeField] private float _movementGracePeriod = .08f;

    public float DashCooldown { set { _dashCooldown = value; } }
    public float DashCooldownRemaining { set { _dashCooldownRemaining = value; }}
    public bool MovementLocked { set { _movementLocked = value; } }
    public float MovementGradePeriod { get { return _movementGracePeriod; } }

    private PlayerControls _playerControls;
    private Rigidbody2D _rb;
    private EquipmentManager _equipmentManager;

    private Vector2 _movement = Vector2.zero;
    private Vector2 _previousMovement = Vector2.zero;
    private Vector2 _delayedMovement = Vector2.zero;
    private float _timeSinceMovementChange = 0f; 
    [SerializeField] private bool _movementLocked = false;

    private Vector2 _dashDirection = Vector2.zero;
    private bool _isDashing = false;
    private bool _dashIsOnCooldown = false;
    private float _dashCooldownRemaining = 0f;
    private float _startingDashCooldown;

    private void Awake() {
        _playerControls = new PlayerControls();

        _rb = GetComponent<Rigidbody2D>();

        _equipmentManager = GetComponent<EquipmentManager>();
    }

    private void Start() {
        _playerControls.Movement.Dash.performed += _ => Dash();

        CalculateAndSetMoveSpeed();

        _startingDashCooldown = _dashCooldown;
    }

    private void OnEnable() {
        _playerControls.Enable();
        EquipmentManager.OnEquipmentChange += CalculateAndSetMoveSpeed;
    }

    private void OnDisable() {
        _playerControls.Disable();
        EquipmentManager.OnEquipmentChange -= CalculateAndSetMoveSpeed;
    }

    private void Update() {
        PlayerInput();
        TrackPlayerMovementChange();
        TrackDashCooldown();
    }

    private void FixedUpdate() {
        Move();
    }

    private void PlayerInput() {
        _movement = _playerControls.Movement.Move.ReadValue<Vector2>();
    }

    private void TrackPlayerMovementChange() {
        if (_previousMovement == _movement) {
            _timeSinceMovementChange += Time.deltaTime;
        }
        else if (_timeSinceMovementChange > _rotationGracePeriod) {
            _timeSinceMovementChange = 0f;
            _delayedMovement = _previousMovement;
        }

        if (_movement != Vector2.zero) { // don't want to rotate to zero when we stop moving
            _previousMovement = _movement;
        }            
    }

    private void Move() {
        if (!_movementLocked) {
            //_rb.velocity = Vector2.zero;
            if (_isDashing) {
                _rb.MovePosition((Vector2)this.transform.position + _dashSpeed * Time.fixedDeltaTime * _dashDirection);
            }
            else {
                _rb.MovePosition((Vector2)this.transform.position + _moveSpeed * Time.fixedDeltaTime * _movement);
            }

            RotatePlayerToMoveDirection();
        }
    }

    private void Dash() {
        if (!_isDashing && !_dashIsOnCooldown && !_movementLocked) {
            _isDashing = true;
            _dashCooldownRemaining = _dashCooldown;
            _dashIsOnCooldown = true;
            _dashDirection = _movement;
            if (_dashDirection == Vector2.zero) {
                _dashDirection = _delayedMovement;
            }

            StartCoroutine(EndDashRoutine());
        }
    }

    private IEnumerator EndDashRoutine() {
        yield return new WaitForSeconds(_dashDuration);
        _isDashing = false;
    }

    private void TrackDashCooldown() {
        if (_dashCooldownRemaining > 0f) {
            _dashCooldownRemaining -= Time.deltaTime;
        }
        else if (_dashIsOnCooldown) {
            _dashIsOnCooldown = false;
            Debug.Log("Dash is ready to use.");
        }
    }

    // TODO remove -90f when I change sprites to right orientation 
    private void RotatePlayerToMoveDirection() {
        if (_movement == Vector2.zero) {
            // use delayedMovement to prevent rotation within grace period
            float angle = Mathf.Atan2(_delayedMovement.y, _delayedMovement.x) * Mathf.Rad2Deg;
            _rb.rotation = angle - 90f;
        }
        else if (_timeSinceMovementChange > _rotationGracePeriod) {
            float angle = Mathf.Atan2(_movement.y, _movement.x) * Mathf.Rad2Deg;
            _rb.rotation = angle - 90f;
        }
    }

    // ROTATION TOWARDS MOUSE (ALTERNATIVE CONTROL SCHEME) -> NOT USED ATM

    private Vector3 GetMouseWorldPos() {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void RotatePlayerToMousePos() {
        Vector3 mouseWorldPos = GetMouseWorldPos();
        mouseWorldPos.z = this.transform.position.z;

        // get cursor direction compared to player
        Vector3 direction = mouseWorldPos - this.transform.position;


        // calculate direction's angle
        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;

        //Debug.Log("Angle is " + angle);

        Quaternion targetRotation;


        if (angle >= -22.5f && angle < 22.5f && this.transform.position.y < mouseWorldPos.y) { // check y axis since angle == 0 in top and bot cases
            targetRotation = Quaternion.Euler(0, 0, 0); // TOP          
        }
        else if (angle >= 22.5f && angle < 67.5f) {
            targetRotation = Quaternion.Euler(0, 0, -45); //TOP-RIGHT
        }
        else if (angle >= 67.5f && angle < 112.5f) {
            targetRotation = Quaternion.Euler(0, 0, -90); //RIGHT
        }
        else if (angle >= 112.5f && angle < 157.5f) {
            targetRotation = Quaternion.Euler(0, 0, -135); //BOTTOM-RIGHT
        }
        else if (angle >= -157.5f && angle < -112.5f) {
            targetRotation = Quaternion.Euler(0, 0, 135); //BOTTOM-LEFT
        }
        else if (angle >= -112.5f && angle < -67.5f) {
            targetRotation = Quaternion.Euler(0, 0, 90); //LEFT
        }
        else if (angle >= -67.5f && angle < -22.5f) {
            targetRotation = Quaternion.Euler(0, 0, 45); //TOP-LEFT
        }        
        else {
            targetRotation = Quaternion.Euler(0, 0, 180); //BOTTOM
        }

        this.transform.rotation = targetRotation;        
    }

    // PUBLIC

    public void CalculateAndSetMoveSpeed() {

        int armorsAmount = 0;

        foreach (var armor in _equipmentManager.ArmorDict) {
            if (armor.Value != null) {
                armorsAmount++;
            }
        }

        _moveSpeed = _baseMoveSpeed - armorsAmount * _armorSpeedReduction;
    }

    public void SetDefaultDashCooldown() {
        this._dashCooldown = _startingDashCooldown;
    }
}
