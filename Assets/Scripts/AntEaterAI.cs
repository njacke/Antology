using UnityEditor.Callbacks;
using UnityEngine;

public class AntEaterAI : MonoBehaviour
{
    [SerializeField] float _rotationSpeed = 3f;
    [SerializeField] float _moveSpeed = 2f;
    private Rigidbody2D _rb;
    private Transform _player;

    private void Awake() {
        _rb = GetComponent<Rigidbody2D>();        
    }

    private void Start() {
        _player = FindObjectOfType<PlayerController>().transform;
    }

    private void Update() {
        RotateTowardsPlayer();
    }

    private void FixedUpdate() {
        MoveTowardsPlayer();
        //RotateTowardsPlayerRb();
    }

    private void MoveTowardsPlayer() {

        Vector2 direction = (_player.position - this.transform.position).normalized;

        _rb.MovePosition((Vector2)this.transform.position + _moveSpeed * Time.fixedDeltaTime * direction);
    }

    private void RotateTowardsPlayer() {
        Vector3 relativeTarget = (_player.position - transform.position).normalized;

        Quaternion targetRotation = Quaternion.FromToRotation(Vector3.right, relativeTarget);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
    }

    private void RotateTowardsPlayerRb() {
        Vector2 playerDir = _player.position - this.transform.position;
        float angle = Mathf.Atan2(playerDir.y, playerDir.x) * Mathf.Rad2Deg;

        _rb.rotation = Mathf.Lerp(_rb.rotation, angle, _rotationSpeed * Time.fixedDeltaTime);
    }
}
