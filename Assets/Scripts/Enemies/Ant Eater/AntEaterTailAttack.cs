using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AntEaterTailAttack : MonoBehaviour
{
    [SerializeField] private float _minAttackAngle = 0f;
    [SerializeField] private float _maxAttackAngle = 50f;
    [SerializeField] private float _maxAttackRange = 3f;
    [SerializeField] private float _attackRotationAngle = 20f;
    [SerializeField] private float _attackSpeed = 1f;
    [SerializeField] private float _cooldown = 5f;
    [SerializeField] private CapsuleCollider2D _tailCollider;
    private float _cooldownRemaining;
    private float _linearT;
    private Animator _myAnim;
    private Transform _player;
    readonly int TAIL_ATTACK_HASH = Animator.StringToHash("TailAttack");

    public bool IsActive { get; private set; } = false;

    private void Awake() {
        _myAnim = GetComponent<Animator>();
    }

    private void Start() {
        _player = FindObjectOfType<PlayerController>().transform;
    }

    private void Update() {
        _cooldownRemaining -= Time.deltaTime;
        if (IsActive && _cooldownRemaining <= 0f && IsPlayerInRange()) {
            UseTailAttack();
        }
    }

    private void UseTailAttack() {
        _myAnim.SetBool(TAIL_ATTACK_HASH, true);
        _cooldownRemaining = _cooldown;
    }

    private void RotateTailAnimEvent() {
        StartCoroutine(TailAttackRoutine());
    }

    private IEnumerator TailAttackRoutine() {
        _tailCollider.enabled = true;

        float startRotationAngleZ = this.transform.localEulerAngles.z;
        Debug.Log(startRotationAngleZ);
        Quaternion startRotation = this.transform.rotation;
        Quaternion targetRotationMin = Quaternion.Euler(0, 0, startRotationAngleZ - _attackRotationAngle);
        Quaternion targetRotationMax = Quaternion.Euler(0, 0, startRotationAngleZ + _attackRotationAngle);

        float timePassed = 0f;

        while (this.transform.localEulerAngles.z > startRotationAngleZ - _attackRotationAngle) {
            timePassed += Time.deltaTime;
            _linearT = timePassed / _attackSpeed;

            this.transform.rotation = Quaternion.Slerp(startRotation, targetRotationMin, _linearT);
            yield return null;
        }

        Debug.Log("Exited 1st rotation");
        timePassed = 0f;

        while (this.transform.localEulerAngles.z < startRotationAngleZ + _attackRotationAngle) {
            timePassed += Time.deltaTime;
            _linearT = timePassed / _attackSpeed;

            this.transform.rotation = Quaternion.Slerp(targetRotationMin, targetRotationMax, _linearT);
            yield return null;
        }

        Debug.Log("Exited 2nd rotation");
        timePassed = 0f;

        while (this.transform.localEulerAngles.z > startRotationAngleZ) {
            timePassed += Time.deltaTime;
            _linearT = timePassed / (_attackSpeed * 2);

            this.transform.rotation = Quaternion.Slerp(targetRotationMax, startRotation, _linearT);
            yield return null;
        }

        Debug.Log("Exited 3rd rotation");

        _tailCollider.enabled = false;
        _myAnim.SetBool(TAIL_ATTACK_HASH, false);
    }

    private bool IsPlayerInRange() {
        Vector3 dir = this.transform.position - _player.position;
        float angle = Vector3.Angle(transform.right, dir);
        //Debug.Log(Mathf.Abs(angle));

        if (Mathf.Abs(angle) > _minAttackAngle && Mathf.Abs(angle) < _maxAttackAngle && dir.magnitude < _maxAttackRange) {
            return true;
        }

        return false;
    }
    public void SetIsActive(bool value) {
        //Debug.Log("TailAttack is active called.");
        IsActive = value;
    }
}
