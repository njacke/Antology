using UnityEngine;

public class Entity : MonoBehaviour
{
    public FiniteStateMachine StateMachine;
    public EntityInfo EntityInfo;
    public int FacingDirection { get; private set; }
    public Rigidbody2D Rb { get; private set; }
    public Animator Anim { get; private set; }
    private Vector2 _velocityWorkspace;

    public virtual void Awake() {
        Rb = GetComponent<Rigidbody2D>();
        Anim = GetComponent<Animator>();
    }

    public virtual void Start() {
        StateMachine = new FiniteStateMachine();        
    }

    public virtual void Update() {
        StateMachine.CurrentState.LogicUpdate();
    }

    public virtual void FixedUpdate() {
        StateMachine.CurrentState.PhysicsUpdate();
    }

    public virtual void SetVelocity(float velocity) {
        _velocityWorkspace.Set(FacingDirection * velocity, Rb.velocity.y);
        Rb.velocity = _velocityWorkspace;
    }
}
