using System.Collections;
using System;
using UnityEngine;

public class AntEaterTailAttack : MonoBehaviour
{
    public static Action<bool> OnTailAttackStateChange;

    [SerializeField] private float _minAttackAngle = 0f;
    [SerializeField] private float _maxAttackAngle = 50f;
    [SerializeField] private float _maxAttackRange = 3f;
    [SerializeField] private float _attackRotationAngle = 20f;
    [SerializeField] private float _attackDuration = .2f;
    [SerializeField] private float _knockbackForce = 5f;
    [SerializeField] private float _knockbackTime = .2f;
    [SerializeField] private float _cooldown = 5f;
    [SerializeField] private CapsuleCollider2D _tailCollider;
    private float _cooldownRemaining;
    private float _linearT;
    private Animator _myAnim;
    private AntEater _antEater;
    private Transform _player;
    readonly int TAIL_ATTACK_HASH = Animator.StringToHash("TailAttack");

    private void Awake() {
        _myAnim = GetComponent<Animator>();
        _antEater = GetComponentInParent<AntEater>();
    }

    private void Start() {
        _player = FindObjectOfType<PlayerController>().transform;
    }

    private void Update() {
        _cooldownRemaining -= Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (_tailCollider.enabled && other.gameObject == _player.gameObject) {
            var playerKnockback = other.GetComponent<Knockback>();
            if (playerKnockback != null) {            
                Vector2 collisonPoint = other.ClosestPoint(_tailCollider.gameObject.transform.position);
                Vector2 dir = (collisonPoint - (Vector2)_tailCollider.gameObject.transform.position).normalized;
                //Vector2 dir = ((Vector2)_player.position - collisonPoint).normalized;
                playerKnockback.GetKnockedBack(dir, _knockbackForce, _knockbackTime);
            }
        }
    }

    private void UseTailAttack() {
        _myAnim.SetBool(TAIL_ATTACK_HASH, true);
        _cooldownRemaining = _cooldown;
        OnTailAttackStateChange?.Invoke(true);
    }

    private void RotateTailAnimEvent() {
        StartCoroutine(TailAttackRoutine());
    }

    private IEnumerator TailAttackRoutine() {
        float startRotationAngleZ = this.transform.localEulerAngles.z;
        //Debug.Log(startRotationAngleZ);
        Quaternion startRotation = this.transform.rotation;

        // check player's position relative to boss tail
        Vector3 dir = this.transform.position - _player.position;
        Vector3 crossProduct = Vector3.Cross(transform.right, dir);
        Quaternion targetRotationMin;
        Quaternion targetRotationMax;

        // set target rotations based on player's position
        if (crossProduct.z >= 0) {
            targetRotationMin = Quaternion.Euler(0, 0, startRotationAngleZ - _attackRotationAngle);
            targetRotationMax = Quaternion.Euler(0, 0, startRotationAngleZ + _attackRotationAngle);
        } else {
            targetRotationMin = Quaternion.Euler(0, 0, startRotationAngleZ + _attackRotationAngle);
            targetRotationMax = Quaternion.Euler(0, 0, startRotationAngleZ - _attackRotationAngle);
        }

        float timePassed = 0f;

        // rotation towards targetRotationMin
        while (timePassed < _attackDuration) {
            timePassed += Time.deltaTime;
            _linearT = timePassed / _attackDuration;

            this.transform.rotation = Quaternion.Slerp(startRotation, targetRotationMin, _linearT);
            yield return null;
        }

        Debug.Log("Exited 1st rotation");
        _tailCollider.enabled = true;
        timePassed = 0f;

        // rotation towards targetRotationMax
        while (timePassed < _attackDuration) {
            timePassed += Time.deltaTime;
            _linearT = timePassed / _attackDuration;

            this.transform.rotation = Quaternion.Slerp(targetRotationMin, targetRotationMax, _linearT);
            yield return null;
        }

        Debug.Log("Exited 2nd rotation");
        timePassed = 0f;

        // rotation towards startRotation
        while (timePassed < _attackDuration) {
            timePassed += Time.deltaTime;
            _linearT = timePassed / (_attackDuration * 2);

            this.transform.rotation = Quaternion.Slerp(targetRotationMax, startRotation, _linearT);
            yield return null;
        }

        Debug.Log("Exited 3rd rotation");

        _tailCollider.enabled = false;
        _myAnim.SetBool(TAIL_ATTACK_HASH, false);
        OnTailAttackStateChange?.Invoke(false);
    }

    private bool IsPlayerInRange() {
        Vector3 dir = this.transform.position - _player.position;
        float angle = Vector3.Angle(transform.right, dir);
        //Debug.Log(Mathf.Abs(angle));

        if (Mathf.Abs(angle) > _minAttackAngle && Mathf.Abs(angle) < _maxAttackAngle && dir.magnitude <= _maxAttackRange) {
            return true;
        }

        return false;
    }

    public void TryUseTailAttack() {
        if (!_antEater.IsBusy && _cooldownRemaining <= 0f && IsPlayerInRange()) {
            UseTailAttack();
        }
    }
}
