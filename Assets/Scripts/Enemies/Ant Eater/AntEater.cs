using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntEater : Entity
{
    public AntEaterMeleeState MeleeState { get; private set; }
    public AntEaterChaseState ChaseState { get; private set; }

    [SerializeField] private ChaseStateInfo _chaseStateInfo;
    [SerializeField] private MeleeStateInfo _meleeStateInfo;

    public AntEaterTongue TongueAttack { get; private set; }
    public AntEaterMeleeAttack MeleeAttack { get; private set;}
    public AntEaterTailAttack TailAttack { get; private set; }
    private Transform _player;

    private readonly int ANIM_BOOL_MOVE_HASH = Animator.StringToHash("Move");
    private readonly int ANIM_BOOL_CHASE_HASH = Animator.StringToHash("Chase");

    public override void Awake()
    {
        base.Awake();
        TongueAttack = GetComponentInChildren<AntEaterTongue>();
        MeleeAttack = GetComponent<AntEaterMeleeAttack>();
        TailAttack = GetComponent<AntEaterTailAttack>();
    }

    public override void Start()
    {
        base.Start();
        _player = FindObjectOfType<PlayerController>().transform;

        MeleeState = new AntEaterMeleeState(this, StateMachine, ANIM_BOOL_MOVE_HASH, _meleeStateInfo, _player, this);
        ChaseState = new AntEaterChaseState(this, StateMachine, ANIM_BOOL_CHASE_HASH, _chaseStateInfo, _player, this);

        StateMachine.Initialize(ChaseState);
    }
}
