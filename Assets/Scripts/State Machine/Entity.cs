using System;
using UnityEngine;

public class Entity : MonoBehaviour, IDamagable
{
    public static Action<int, int> OnHealthChanged;
    public static Action OnDeath;
    [SerializeField] protected EntityInfo _entityInfo;
    public FiniteStateMachine StateMachine;
    public int FacingDirection { get; private set; }
    public Rigidbody2D Rb { get; private set; }
    public Animator Anim { get; private set; }
    private Vector2 _velocityWorkspace;
    private Vector3 _startPos;
    private Quaternion _startRot;
    protected int _currentHealth;

    public virtual void Awake() {
        Rb = GetComponent<Rigidbody2D>();
        Anim = GetComponent<Animator>();
        
        _startPos = this.transform.position;
        _startRot = this.transform.rotation;

        _currentHealth = _entityInfo.BaseHealth;
        OnHealthChanged?.Invoke(_currentHealth, _entityInfo.BaseHealth);
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

    public virtual void OnEnable() {
        GameManager.OnGameStarted += GameManager_OnGameStarted;
    }

    public virtual void OnDisable() {
        GameManager.OnGameStarted -= GameManager_OnGameStarted;        
    }

    public virtual void GameManager_OnGameStarted() {
        this.transform.position = _startPos;
        this.transform.rotation = _startRot;
        _currentHealth = _entityInfo.BaseHealth;
        OnHealthChanged?.Invoke(_currentHealth, _entityInfo.BaseHealth);
    }

    public virtual void SetVelocity(float velocity) {
        _velocityWorkspace.Set(FacingDirection * velocity, Rb.velocity.y);
        Rb.velocity = _velocityWorkspace;
    }

    public virtual void TakeDamage(int damageAmount, Vector3 damagePos) {
        _currentHealth -= damageAmount;
        OnHealthChanged?.Invoke(_currentHealth, _entityInfo.BaseHealth);
    }
}
