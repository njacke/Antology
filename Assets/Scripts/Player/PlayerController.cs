using System;
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

    [SerializeField] private ParticleSystem _dashVfxLeft;
    [SerializeField] private ParticleSystem _dashVfxRight;

    public float DashCooldown { set { _dashCooldown = value; } }
    public float DashCooldownRemaining { set { _dashCooldownRemaining = value; }}
    public bool MovementLocked { set { _movementLocked = value; } }
    public float MovementGradePeriod { get { return _movementGracePeriod; } }

    private PlayerControls _playerControls;
    private Rigidbody2D _rb;
    private EquipmentManager _equipmentManager;
    private Knockback _knockback;
    private Vector3 _startPos;
    private Quaternion _startRot;

    private Vector2 _movement = Vector2.zero;

    private Vector2 _previousMovement;
    private Vector2 _delayedMovement;
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
        _knockback = GetComponent<Knockback>();

        _startPos = this.transform.position;
        _startRot = this.transform.rotation;
    }

    private void Start() {
        _playerControls.Movement.Dash.performed += _ => Dash();
        _startingDashCooldown = _dashCooldown;

        CalculateAndSetMoveSpeed();
        SetDefaultRotationTop();
    }

    private void OnEnable() {
        _playerControls.Enable();
        EquipmentManager.OnEquipmentChange += CalculateAndSetMoveSpeed;
        PlayerHealth.OnPlayerDeath += PlayerHealth_OnPlayerDeath;
        GameManager.OnGameStarted += GameManager_OnGameStarted;
    }

    private void OnDisable() {
        _playerControls.Disable();
        EquipmentManager.OnEquipmentChange -= CalculateAndSetMoveSpeed;
        PlayerHealth.OnPlayerDeath -= PlayerHealth_OnPlayerDeath;
        GameManager.OnGameStarted -= GameManager_OnGameStarted;
    }


    private void Update() {
        if (GameManager.Instance.IsGameActive) {
            PlayerInput();
            TrackPlayerMovementChange();
            TrackDashCooldown();
        }
    }

    private void FixedUpdate() {
        if (GameManager.Instance.IsGameActive) {
            Move();
        }
    }

    private void GameManager_OnGameStarted() {
        PlayerReset();
    }

    private void PlayerHealth_OnPlayerDeath() {
        _dashVfxLeft.Stop();
        _dashVfxRight.Stop();
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
        if (!_movementLocked && !_knockback.GettingKnockedBack) {
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
        if (!_isDashing && !_dashIsOnCooldown && !_movementLocked && !_knockback.GettingKnockedBack) {
            _isDashing = true;
            _dashCooldownRemaining = _dashCooldown;
            _dashIsOnCooldown = true;
            _dashDirection = _movement;            
            if (_dashDirection == Vector2.zero) {
                _dashDirection = _delayedMovement;
            }

            if (_dashCooldownRemaining > 0f) {
                _dashVfxLeft.Stop();
                _dashVfxRight.Stop();
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
            _dashVfxLeft.Play();
            _dashVfxRight.Play();
            Debug.Log("Dash is ready to use.");
        }
    }

    private void RotatePlayerToMoveDirection() {
        if (_movement == Vector2.zero) {
            // use delayedMovement to prevent rotation within grace period
            float angle = Mathf.Atan2(_delayedMovement.y, _delayedMovement.x) * Mathf.Rad2Deg;
            _rb.rotation = angle;
        }
        else if (_timeSinceMovementChange > _rotationGracePeriod) {
            float angle = Mathf.Atan2(_movement.y, _movement.x) * Mathf.Rad2Deg;
            _rb.rotation = angle;
        }
    }

    private void SetDefaultRotationTop() {
        _previousMovement = new Vector2(0, 1);
        _delayedMovement = new Vector2(0, 1);
    }

    // RESET
    private void PlayerReset() {
        SetDefaultDashCooldown();
        _dashCooldownRemaining = 0f;
        _dashVfxLeft.Play();
        _dashVfxRight.Play();

        this.transform.position = _startPos;
        this.transform.rotation = _startRot;
        CalculateAndSetMoveSpeed();
        SetDefaultRotationTop();
        _movementLocked = false;
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
