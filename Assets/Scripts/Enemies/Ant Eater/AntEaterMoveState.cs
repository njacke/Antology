using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntEaterMoveState : MoveState
{
    private AntEater _antEater;
    public AntEaterMoveState(Entity entity, FiniteStateMachine stateMachine, int animBoolNameHash, MoveStateInfo moveStateInfo, AntEater antEater) : base(entity, stateMachine, animBoolNameHash, moveStateInfo)
    {
        _antEater = antEater;
    }

    public override void Enter()
    {
        base.Enter();
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
