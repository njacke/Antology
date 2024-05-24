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
        _antEater.TongueAttack.SetIsActive(true);
    }

    public override void Exit()
    {
        base.Exit();
        _antEater.TongueAttack.SetIsActive(false);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();        
        Vector3 dir = _target.position - _entity.transform.position;
        float distance = dir.magnitude;
        
        if (distance <= _chaseStateInfo.MinChaseRange) {
            _antEater.StateMachine.ChangeState(_antEater.MeleeState);
        }
        
        //Debug.Log("I AM IN CHASE STATE");
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
