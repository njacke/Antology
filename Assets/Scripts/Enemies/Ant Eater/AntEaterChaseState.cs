using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntEaterChaseState : ChaseState
{
    private AntEater _antEater;

    public AntEaterChaseState(Entity entity, FiniteStateMachine stateMachine, int animBoolNameHash, ChaseStateInfo chaseStateInfo, Transform target, AntEater antEater) : base(entity, stateMachine, animBoolNameHash, chaseStateInfo, target)
    {
        _antEater = antEater;
    }

    public override void Enter()
    {
        base.Enter();
        _antEater.Tongue.SetIsActive(true);
    }

    public override void Exit()
    {
        base.Exit();
        _antEater.Tongue.SetIsActive(false);
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
