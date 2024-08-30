using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathState : State
{
    protected DeathStateInfo _deathStateInfo;

    public DeathState(Entity entity, FiniteStateMachine stateMachine, int animBoolNameHash, DeathStateInfo deathStateInfo) : base(entity, stateMachine, animBoolNameHash)
    {        
        _deathStateInfo = deathStateInfo;
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
