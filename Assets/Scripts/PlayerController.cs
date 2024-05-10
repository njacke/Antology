using System.Collections;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashDuration = .1f;
    [SerializeField] private float dashCooldown = 3f;
    [SerializeField] private float rotationGracePeriod = .03f;
    [SerializeField] private float movementGracePeriod = .08f;

    public float MoveSpeed { set { moveSpeed = value; } }
    public float DashCooldown { set { dashCooldown = value; } }
    public float DashCooldownRemaining { set { dashCooldownRemaining = value; }}
    public bool MovementLocked { set { movementLocked = value; } }
    public float MovementGradePeriod { get { return movementGracePeriod; } }

    private PlayerControls playerControls;
    private Rigidbody2D rb;

    private Vector2 movement = Vector2.zero;
    private Vector2 previousMovement = Vector2.zero;
    private Vector2 delayedMovement = Vector2.zero;
    private float timeSinceMovementChange = 0f; 
    private float startingMoveSpeed;
    private bool movementLocked = false;

    private Vector2 dashDirection = Vector2.zero;
    private bool isDashing = false;
    private bool dashIsOnCooldown = false;
    private float dashCooldownRemaining = 0f;
    private float startingDashCooldown;

    private void Awake() {
        playerControls = new PlayerControls();

        rb = GetComponent<Rigidbody2D>();
    }

    private void Start() {
        playerControls.Movement.Dash.performed += _ => Dash();

        startingMoveSpeed = moveSpeed;
        startingDashCooldown = dashCooldown;
    }

    private void OnEnable() {
        playerControls.Enable();
    }

    private void OnDisable() {
        playerControls.Disable();
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
        movement = playerControls.Movement.Move.ReadValue<Vector2>();
    }

    private void TrackPlayerMovementChange() {
        if (previousMovement == movement) {
            timeSinceMovementChange += Time.deltaTime;
            //Debug.Log(timeSinceMovementChange);
        }
        else if (timeSinceMovementChange > rotationGracePeriod) {
            timeSinceMovementChange = 0f;
            delayedMovement = previousMovement;
        }

        if (movement != Vector2.zero) { // don't want to rotate to zero when we stop moving
            previousMovement = movement;
        }            
    }

    private void Move() {
        if (!movementLocked) {
            if (isDashing) {
                rb.MovePosition((Vector2)this.transform.position + dashSpeed * Time.fixedDeltaTime * dashDirection);
            }

            else {
                rb.MovePosition((Vector2)this.transform.position + moveSpeed * Time.fixedDeltaTime * movement);
                //Debug.Log("Magnitude " + movement.magnitude);
            }

            RotatePlayerToMoveDirection();
        }
    }

    private void Dash() {
        if (!isDashing && !dashIsOnCooldown && !movementLocked) {
            isDashing = true;
            dashCooldownRemaining = dashCooldown;
            dashIsOnCooldown = true;
            dashDirection = movement;
            if (dashDirection == Vector2.zero) {
                dashDirection = delayedMovement;
            }

            StartCoroutine(EndDashRoutine());
        }
    }

    private IEnumerator EndDashRoutine() {
        yield return new WaitForSeconds(dashDuration);
        isDashing = false;
    }

    private void TrackDashCooldown() {
        if (dashCooldownRemaining > 0f) {
            dashCooldownRemaining -= Time.deltaTime;
        }
        else if (dashIsOnCooldown) {
            dashIsOnCooldown = false;
            Debug.Log("Dash is ready to use.");
        }
    }

    private void RotatePlayerToMoveDirection() {
        if (movement == Vector2.zero) {
            // use delayedMovement to prevent rotation within grace period
            float angle = Mathf.Atan2(delayedMovement.x, delayedMovement.y) * Mathf.Rad2Deg;
            rb.rotation = -angle;
        }
        else if (timeSinceMovementChange > rotationGracePeriod) {
            float angle = Mathf.Atan2(movement.x, movement.y) * Mathf.Rad2Deg;
            rb.rotation = -angle;
        }
    }

    // ROTATION TOWARDS MOUSE (ALTERNATIVE CONTROL SCHEME)

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

    public void SetDefaultMoveSpeed() {
        this.moveSpeed = startingMoveSpeed;
    }

    public void SetDefaultDashCooldown() {
        this.dashCooldown = startingDashCooldown;
    }
}
