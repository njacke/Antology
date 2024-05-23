using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : State
{
    protected ChaseStateInfo _chaseStateInfo;
    protected Transform _target;

    public ChaseState(Entity entity, FiniteStateMachine stateMachine, int animBoolNameHash, ChaseStateInfo chaseStateInfo, Transform target) : base(entity, stateMachine, animBoolNameHash)
    {        
        _chaseStateInfo = chaseStateInfo;
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
        Vector3 dir = _target.position - _entity.transform.position;
        float distance = dir.magnitude;
        _entity.transform.right = Vector3.MoveTowards(_entity.transform.right, dir, _chaseStateInfo.RotationSpeed * Time.deltaTime);
        if (distance <= _chaseStateInfo.MaxChaseRange && distance >= _chaseStateInfo.MinChaseRange) {
            _entity.transform.position = Vector3.MoveTowards(_entity.transform.position, _entity.transform.position + _entity.transform.right, _chaseStateInfo.MovementSpeed * Time.deltaTime);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
