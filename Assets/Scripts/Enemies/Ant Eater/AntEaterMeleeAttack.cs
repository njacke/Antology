using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntEaterMeleeAttack : MonoBehaviour
{
    [SerializeField] private float _minAttackAngle = 130f;
    [SerializeField] private float _maxAttackAngle = 180f;
    [SerializeField] private float _maxAttackRange = 3f;
    [SerializeField] private float _cooldown = 3f;
    [SerializeField] private BoxCollider2D _rightHandCollider; 
    [SerializeField] private BoxCollider2D _leftHandCollider;
    private float _cooldownRemaining;
    private Animator _myAnim;
    private Transform _player;
    readonly int MELEE_ATTACK_HASH = Animator.StringToHash("MeleeAttack");

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
            UseMeleeAttack();
        }
    }

    private void UseMeleeAttack() {
        _myAnim.SetBool(MELEE_ATTACK_HASH, true);
        _cooldownRemaining = _cooldown;
    }

    private void EnableColliderAnimEvent() {
        _rightHandCollider.enabled = true;
        _leftHandCollider.enabled = true;
    }

    private void DisableColliderAnimEvent() {
        _rightHandCollider.enabled = true;
        _leftHandCollider.enabled = true;
    }

    private void EndMeleeAttackAnimEvent() {
        _myAnim.SetBool(MELEE_ATTACK_HASH, false);
    }

    private bool IsPlayerInRange() {
        Vector3 dir = this.transform.position - _player.position;
        float angle = Vector3.Angle(transform.right, dir);

        if (Mathf.Abs(angle) > _minAttackAngle && Mathf.Abs(angle) < _maxAttackAngle && dir.magnitude < _maxAttackRange) {
            return true;
        }

        return false;
    }
    public void SetIsActive(bool value) {
        //Debug.Log("MeleeAttack is active called.");
        IsActive = value;
    }
}
