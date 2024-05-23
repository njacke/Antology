using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntEaterMelee : MonoBehaviour
{
    [SerializeField] private float _minAttackAngle = 90f;
    [SerializeField] private float _maxAttackAngle = 270f;
    [SerializeField] private float _maxAttackRange = 3f;
    [SerializeField] private float _cooldown = 3f;
    [SerializeField] private Collider2D _rightHandCollider; 
    [SerializeField] private Collider2D _leftHandCollider;
    private float _cooldownRemaining;
    private Transform _player;

    private Animator _myAnim;

    private void Awake() {
        _myAnim = GetComponent<Animator>();
    }
    private void Start() {
        _player = FindObjectOfType<PlayerController>().transform;
    }

    private void Update() {
        _cooldownRemaining -= Time.deltaTime;
        if (_cooldownRemaining < 0 && IsPlayerInRange()) {
            UseMeleeAttack();
        }
    }

    private void UseMeleeAttack() {
        _myAnim.SetBool("MeleeAttack", true);
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
        _myAnim.SetBool("MeleeAttack", false);
    }

    private bool IsPlayerInRange() {
        Vector3 dir = this.transform.position - _player.position;
        float angle = Vector3.Angle(transform.right, dir);

        if (Mathf.Abs(angle) > _minAttackAngle && Mathf.Abs(angle) < _maxAttackAngle && dir.magnitude < _maxAttackRange) {
            return true;
        }

        return false;
    }
}
