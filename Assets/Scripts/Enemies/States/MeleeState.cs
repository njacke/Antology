using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeState : State
{
    protected MeleeStateInfo _meleeStateInfo;
    protected Transform _target;

    public MeleeState(Entity entity, FiniteStateMachine stateMachine, int animBoolNameHash, MeleeStateInfo meleeStateInfo, Transform target) : base(entity, stateMachine, animBoolNameHash)
    {        
        _meleeStateInfo = meleeStateInfo;
        _target = target;
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
