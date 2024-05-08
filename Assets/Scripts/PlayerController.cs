using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashDuration = .1f;

    private PlayerControls playerControls;
    private Vector2 movement;
    private Rigidbody2D rb;
    private PlayerCombat playerCombat;

    private float startingMoveSpeed;
    private bool isDashing = false;

    private void Awake() {
        playerControls = new PlayerControls();

        rb = GetComponent<Rigidbody2D>();

        playerCombat = FindObjectOfType<PlayerCombat>();
    }

    private void Start() {
        playerControls.Movement.Dash.performed += _ => Dash();

        startingMoveSpeed = moveSpeed;
    }

    private void OnEnable() {
        playerControls.Enable();
    }

    private void OnDisable() {
        playerControls.Disable();
    }

    private void Update() {
        PlayerInput();

        if (!playerCombat.ControlsLocked) {
            RotatePlayerToMoveDirection();

            //RotatePlayerToMousePos();
        }
    }

    private void FixedUpdate() {
        Move();
    }

    private void PlayerInput() {
        movement = playerControls.Movement.Move.ReadValue<Vector2>();
    }

    private void Move() {
        if (!playerCombat.ControlsLocked){
            rb.MovePosition((Vector2)this.transform.position + moveSpeed * Time.fixedDeltaTime * movement);
        }
    }

    private void Dash() {
        if (!isDashing && !playerCombat.ControlsLocked) {
            isDashing = true;
            moveSpeed = dashSpeed;
            StartCoroutine(EndDashRoutine());
        }
    }

    private IEnumerator EndDashRoutine() {
        yield return new WaitForSeconds(dashDuration);
        moveSpeed = startingMoveSpeed;
        isDashing = false;
    }

    private void RotatePlayerToMoveDirection() {        
        if (movement == Vector2.zero) {
            return;
        }
        else {
            float angle = Mathf.Atan2(movement.x, movement.y) * Mathf.Rad2Deg;

            //Debug.Log ("Angle is " + angle);

            this.transform.rotation = Quaternion.Euler(0, 0, -angle);
        }
    }

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
}
