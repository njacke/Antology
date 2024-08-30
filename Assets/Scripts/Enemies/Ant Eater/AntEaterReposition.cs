using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntEaterReposition : MonoBehaviour
{
    public static Action<bool> OnRepositionStateChange;

    [SerializeField] private float _minRepositionAngle = 130f;
    [SerializeField] private float _maxRepositionAngle = 180f;
    [SerializeField] private float _repositionDuration = 1f;
    [SerializeField] private float _repositionDistance = 1f;
    [SerializeField] private float _cooldown = 3f;

    private float _cooldownRemaining;
    private Animator _myAnim;
    private AntEater _antEater;
    private Transform _player;
    readonly int REPOSITION_HASH = Animator.StringToHash("Reposition");

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

    private void UseReposition() {
        _myAnim.SetBool(REPOSITION_HASH, true);
        _cooldownRemaining = _cooldown;
        OnRepositionStateChange?.Invoke(true);
    }

    private void RepositionAnimEvent() {
        StartCoroutine(RepositionRoutine());
    }

    private IEnumerator RepositionRoutine() {
        Vector3 targetPos = this.transform.position + -transform.right * _repositionDistance;

        Vector3 dir = _player.position - targetPos;
        float targetAngleZ = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngleZ);

        this.transform.GetPositionAndRotation(out Vector3 startPos, out Quaternion startRotation);
        float timePassed = 0f;

        while (timePassed < _repositionDuration) {
            timePassed += Time.deltaTime;
            float linearT = timePassed / _repositionDuration;
            
            this.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, linearT);
            this.transform.position = Vector3.Lerp(startPos, targetPos, linearT);

            yield return null;
        }

        this.transform.SetPositionAndRotation(targetPos, targetRotation);
        _myAnim.SetBool(REPOSITION_HASH, false);
        OnRepositionStateChange?.Invoke(false);
    }

    private bool IsPlayerInRange() {
        Vector3 dir = this.transform.position - _player.position;
        float angle = Vector3.Angle(transform.right, dir);

        if (Mathf.Abs(angle) > _minRepositionAngle && Mathf.Abs(angle) <= _maxRepositionAngle) {
            return true;
        }

        return false;
    }

    public void TryUseReposition() {
        if (!_antEater.IsBusy && _cooldownRemaining <= 0f && IsPlayerInRange()) {
            UseReposition();
        }
    }
}
