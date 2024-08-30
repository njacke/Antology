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
        //RotateTowardsTargetAngled();
        RotateTowardsTarget();
        MoveForward();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public virtual void MoveForward() {
        _entity.transform.position = Vector3.MoveTowards(_entity.transform.position, _entity.transform.position + _entity.transform.right, _chaseStateInfo.MovementSpeed * Time.deltaTime);
    }

    public virtual void RotateTowardsTarget() {
        Vector3 dir = _target.position - _entity.transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
        _entity.transform.rotation = Quaternion.Slerp(_entity.transform.rotation, targetRotation, _chaseStateInfo.RotationSpeed * Time.deltaTime);        
    }

    public virtual void RotateTowardsTargetAngled() {

        //Debug.Log("Rotating towards target");
        //_entity.transform.right = Vector3.MoveTowards(_entity.transform.right, dir, _chaseStateInfo.RotationSpeed * Time.deltaTime);

        Vector3 dir = _target.position - _entity.transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        //Debug.Log("Angle is " + angle);

        Quaternion targetRotation;


        if (angle >= -22.5f && angle < 22.5f) {
            targetRotation = Quaternion.Euler(0, 0, 0); // RIGHT          
        }
        else if (angle >= 22.5f && angle < 67.5f) {
            targetRotation = Quaternion.Euler(0, 0, 45); //TOP-RIGHT
        }
        else if (angle >= 67.5f && angle < 112.5f) {
            targetRotation = Quaternion.Euler(0, 0, 90); //TOP
        }
        else if (angle >= 112.5f && angle < 157.5f) {
            targetRotation = Quaternion.Euler(0, 0, 135); //TOP-LEFT
        }
        else if (angle >= -157.5f && angle < -112.5f) {
            targetRotation = Quaternion.Euler(0, 0, -135); //BOTTOM-LEFT
        }
        else if (angle >= -112.5f && angle < -67.5f) {
            targetRotation = Quaternion.Euler(0, 0, -90); //BOTTOM
        }
        else if (angle >= -67.5f && angle < -22.5f) {
            targetRotation = Quaternion.Euler(0, 0, -45); //BOTTOM-RIGHT
        }        
        else {
            targetRotation = Quaternion.Euler(0, 0, 180); //LEFT
        }

        _entity.transform.rotation = Quaternion.Slerp(_entity.transform.rotation, targetRotation, _chaseStateInfo.RotationSpeed * Time.deltaTime);        
    }
}
