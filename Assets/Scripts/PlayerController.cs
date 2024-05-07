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
    private PrimaryAttack primaryAttack;

    private float startingMoveSpeed;
    private bool isDashing = false;

    private void Awake() {
        playerControls = new PlayerControls();

        rb = GetComponent<Rigidbody2D>();

        primaryAttack = FindObjectOfType<PrimaryAttack>();
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
        RotatePlayer();
    }

    private void FixedUpdate() {
        Move();
    }

    private void PlayerInput() {
        movement = playerControls.Movement.Move.ReadValue<Vector2>();
    }

    private void Move() {
        if (!primaryAttack.IsAttacking){
            rb.MovePosition((Vector2)this.transform.position + moveSpeed * Time.fixedDeltaTime * movement);
        }
    }

    private void Dash() {
        if (!isDashing && !primaryAttack.IsAttacking) {
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

    private Vector3 GetMouseWorldPos() {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void RotatePlayer() {

        Vector3 mouseWorldPos = GetMouseWorldPos();
        mouseWorldPos.z = this.transform.position.z;


        // get cursor direction compared to player
        Vector3 direction = mouseWorldPos - this.transform.position;


        // calculate direction's angle
        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;

        Debug.Log("Angle is " + angle);

        Quaternion targetRotation;

        if (angle >= -22.5 && angle < 22.5) {
            targetRotation = Quaternion.Euler(0, 0, 0); // TOP          
        }
        else if (angle >= 22.5 && angle < 67.5) {
            targetRotation = Quaternion.Euler(0, 0, -45); //TOP-RIGHT
        }
        else if (angle >= 67.5 && angle < 112.5) {
            targetRotation = Quaternion.Euler(0, 0, -90); //RIGHT
        }
        else if (angle >= 112.5 && angle < 157.5) {
            targetRotation = Quaternion.Euler(0, 0, -135); //BOTTOM-RIGHT
        }
        else if (angle >= 157.5 && angle < -157.5) {
            targetRotation = Quaternion.Euler(0, 0, 180); //BOTTOM
        }
        else if (angle >= -157.5 && angle < -112.5) {
            targetRotation = Quaternion.Euler(0, 0, 135); //BOTTOM-LEFT
        }
        else if (angle >= -112.5 && angle < -67.5) {
            targetRotation = Quaternion.Euler(0, 0, 90); //LEFT
        }
        else {
            targetRotation = Quaternion.Euler(0, 0, 45); //TOP-LEFT
        }        

        this.transform.rotation = targetRotation;        
    }
}
