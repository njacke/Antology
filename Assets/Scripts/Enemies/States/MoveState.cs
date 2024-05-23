using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : State
{
    protected MoveStateInfo _moveStateInfo;

    public MoveState(Entity entity, FiniteStateMachine stateMachine, int animBoolNameHash, MoveStateInfo moveStateInfo) : base(entity, stateMachine, animBoolNameHash)
    {        
        _moveStateInfo = moveStateInfo;
    }

    public override void Enter()
    {
        base.Enter();
        _entity.SetVelocity(_moveStateInfo.MovementSpeed);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
