using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntEater : Entity
{
    public AntEaterMoveState MoveState { get; private set; }
    public AntEaterChaseState ChaseState { get; private set; }

    [SerializeField] private MoveStateInfo _moveStateInfo;
    [SerializeField] private ChaseStateInfo _chaseStateInfo;

    public  AntEaterTongue Tongue { get; private set; }
    private Transform _player;

    private readonly int ANIM_BOOL_MOVE_HASH = Animator.StringToHash("Move");
    private readonly int ANIM_BOOL_CHASE_HASH = Animator.StringToHash("Chase");

    public override void Awake()
    {
        base.Awake();
        Tongue = GetComponentInChildren<AntEaterTongue>();
    }

    public override void Start()
    {
        base.Start();
        _player = FindObjectOfType<PlayerController>().transform;

        MoveState = new AntEaterMoveState(this, StateMachine, ANIM_BOOL_MOVE_HASH, _moveStateInfo, this);
        ChaseState = new AntEaterChaseState(this, StateMachine, ANIM_BOOL_CHASE_HASH, _chaseStateInfo, _player, this);

        StateMachine.Initialize(ChaseState);
    }
}
