using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntEaterDeathState : DeathState
{
    private AntEater _antEater;

    public AntEaterDeathState(Entity entity, FiniteStateMachine stateMachine, int animBoolNameHash, DeathStateInfo deathStateInfo, AntEater antEater) : base(entity, stateMachine, animBoolNameHash, deathStateInfo)
    {
        _antEater = antEater;
    }

    public override void Enter()
    {
        base.Enter();
        _antEater.TongueAttack.SetIsActive(false);
        _antEater.Death();
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
