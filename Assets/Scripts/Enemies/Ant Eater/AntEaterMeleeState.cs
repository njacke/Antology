using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntEaterMeleeState : MeleeState
{
    private AntEater _antEater;

    public AntEaterMeleeState(Entity entity, FiniteStateMachine stateMachine, int animBoolNameHash, MeleeStateInfo meleeStateInfo, Transform target, AntEater antEater) : base(entity, stateMachine, animBoolNameHash, meleeStateInfo, target)
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
        Vector3 dir = _target.position - _entity.transform.position;
        float distance = dir.magnitude;
        
        if (distance > _meleeStateInfo.MaxMeleeRange && !_antEater.IsBusy) {
            _antEater.StateMachine.ChangeState(_antEater.ChaseState);
        } else {
            // attacks in order of priority
            _antEater.MeleeAttack.TryUseMeleeAttack();
            _antEater.TailAttack.TryUseTailAttack();
            _antEater.Reposition.TryUseReposition();
        }

        //Debug.Log("I AM IN MELEE STATE");
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
