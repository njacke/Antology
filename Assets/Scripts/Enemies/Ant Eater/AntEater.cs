using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class AntEater : Entity
{
    [SerializeField] private ChaseStateInfo _chaseStateInfo;
    [SerializeField] private MeleeStateInfo _meleeStateInfo;
    [SerializeField] private GameObject _takeDamageVFX;

    public AntEaterMeleeState MeleeState { get; private set; }
    public AntEaterChaseState ChaseState { get; private set; }

    public AntEaterTongue TongueAttack { get; private set; }
    public AntEaterMeleeAttack MeleeAttack { get; private set;}
    public AntEaterTailAttack TailAttack { get; private set; }
    public AntEaterReposition Reposition { get; private set; }

    public bool IsBusy { get; private set; } = false;

    private Transform _player;

    private readonly int ANIM_BOOL_MOVE_HASH = Animator.StringToHash("Move");
    private readonly int ANIM_BOOL_CHASE_HASH = Animator.StringToHash("Chase");

    public override void Awake() {
        base.Awake();
        TongueAttack = GetComponentInChildren<AntEaterTongue>();
        MeleeAttack = GetComponent<AntEaterMeleeAttack>();
        TailAttack = GetComponent<AntEaterTailAttack>();
        Reposition = GetComponent<AntEaterReposition>();
    }

    public override void Start() {
        base.Start();
        _player = FindObjectOfType<PlayerController>().transform;


        MeleeState = new AntEaterMeleeState(this, StateMachine, ANIM_BOOL_MOVE_HASH, _meleeStateInfo, _player, this);
        ChaseState = new AntEaterChaseState(this, StateMachine, ANIM_BOOL_CHASE_HASH, _chaseStateInfo, _player, this);

        StateMachine.Initialize(ChaseState);
    }

    public override void OnEnable() {
        base.OnEnable();
        AntEaterMeleeAttack.OnMeleeAttackStateChange += SetIsBusy;
        AntEaterTailAttack.OnTailAttackStateChange += SetIsBusy;
        AntEaterReposition.OnRepositionStateChange += SetIsBusy;
    }

    public override void OnDisable() {
        base.OnDisable();
        AntEaterMeleeAttack.OnMeleeAttackStateChange -= SetIsBusy;
        AntEaterTailAttack.OnTailAttackStateChange -= SetIsBusy;
        AntEaterReposition.OnRepositionStateChange -= SetIsBusy;        
    }

    private void SetIsBusy(bool busyState) {
        IsBusy = busyState;
    }

    public override void TakeDamage(int damageAmount, Vector3 damagePos) {
        base.TakeDamage(damageAmount, damagePos);
        Instantiate(_takeDamageVFX, damagePos, Quaternion.identity);
        
        OnHealthChanged?.Invoke(_currentHealth, _entityInfo.BaseHealth);
        Debug.Log("Current AntEater HP: " + _currentHealth);

        if (_currentHealth <= 0) {
            // trigger death
            Debug.Log("AntEater is Dead");
        }
    }
}
