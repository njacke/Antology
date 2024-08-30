using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntEater : Entity
{
    [SerializeField] private ChaseStateInfo _chaseStateInfo;
    [SerializeField] private MeleeStateInfo _meleeStateInfo;
    [SerializeField] private DeathStateInfo _deathStateInfo;
    [SerializeField] private GameObject _takeDamageVFX;
    [SerializeField] private float _deathAnimDelay = 1f;
    [SerializeField] private SpriteRenderer _damageSpriteRenderer;
    [SerializeField] private Sprite[] _damageSprites;

    public AntEaterMeleeState MeleeState { get; private set; }
    public AntEaterChaseState ChaseState { get; private set; }
    public AntEaterDeathState DeathState { get; private set; }

    public AntEaterTongue TongueAttack { get; private set; }
    public AntEaterMeleeAttack MeleeAttack { get; private set;}
    public AntEaterTailAttack TailAttack { get; private set; }
    public AntEaterReposition Reposition { get; private set; }
    public float GetDeathAnimDelay { get { return _deathAnimDelay; } }

    public bool IsBusy { get; private set; } = false;

    private Transform _player;
    private float _healthThresholdIncrement;
    private List<float> _healthThresholds = new();
    private int _currentDamageSpriteIndex = -1;
    private bool _isDead = false;

    private readonly int ANIM_BOOL_MELEE_HASH = Animator.StringToHash("Melee");
    private readonly int ANIM_BOOL_CHASE_HASH = Animator.StringToHash("Chase");
    private readonly int ANIM_BOOL_DEATH_HASH = Animator.StringToHash("Death");

    public override void Awake()
    {
        base.Awake();
        TongueAttack = GetComponentInChildren<AntEaterTongue>();
        MeleeAttack = GetComponent<AntEaterMeleeAttack>();
        TailAttack = GetComponent<AntEaterTailAttack>();
        Reposition = GetComponent<AntEaterReposition>();

        _damageSpriteRenderer.sprite = _damageSprites[0];
        _currentDamageSpriteIndex = 0;

        SetHealthThresholds();
    }

    private void SetHealthThresholds() {       
        if (_damageSprites.Length > 1) {
            _healthThresholdIncrement = 1f / (_damageSprites.Length - 1) * _entityInfo.BaseHealth;
            Debug.Log("Threshold increment: " + _healthThresholdIncrement);
            for (int i = 0; i < _damageSprites.Length - 1; i++) {
                var threshold = _entityInfo.BaseHealth - i * _healthThresholdIncrement;
                _healthThresholds.Add(threshold);
                Debug.Log("Threshold Added: " + threshold);
            }
        }
    }

    public override void Start() {
        base.Start();
        _player = FindObjectOfType<PlayerController>().transform;


        MeleeState = new AntEaterMeleeState(this, StateMachine, ANIM_BOOL_MELEE_HASH, _meleeStateInfo, _player, this);
        ChaseState = new AntEaterChaseState(this, StateMachine, ANIM_BOOL_CHASE_HASH, _chaseStateInfo, _player, this);
        DeathState = new AntEaterDeathState(this, StateMachine, ANIM_BOOL_CHASE_HASH, _deathStateInfo, this);

        StateMachine.Initialize(ChaseState);
    }

    public override void OnEnable() {
        base.OnEnable();
        AntEaterMeleeAttack.OnMeleeAttackStateChange += SetIsBusy;
        AntEaterTailAttack.OnTailAttackStateChange += SetIsBusy;
        AntEaterReposition.OnRepositionStateChange += SetIsBusy;
    }

    public override void OnDisable() {
        base.OnDisable();
        AntEaterMeleeAttack.OnMeleeAttackStateChange -= SetIsBusy;
        AntEaterTailAttack.OnTailAttackStateChange -= SetIsBusy;
        AntEaterReposition.OnRepositionStateChange -= SetIsBusy;        
    }

    private void SetIsBusy(bool busyState) {
        IsBusy = busyState;
    }

    public override void TakeDamage(int damageAmount, Vector3 damagePos) {
        base.TakeDamage(damageAmount, damagePos);
        Instantiate(_takeDamageVFX, damagePos, Quaternion.identity);

        Debug.Log("Current AntEater HP: " + _currentHealth);

        // update damage visuals if needed
        UpdateDamageVisuals();

        if (_currentHealth <= 0 && !_isDead) {
            _isDead = true;
            StateMachine.ChangeState(DeathState);
            OnDeath?.Invoke();
            Debug.Log("AntEater is Dead");
        }
    }

    public override void GameManager_OnGameStarted() {
        base.GameManager_OnGameStarted();
        _isDead = false;
        _damageSpriteRenderer.sprite = _damageSprites[0];
        _currentDamageSpriteIndex = 0;
    }

    private void UpdateDamageVisuals() {
        for (int i = 1; i < _healthThresholds.Count; i++) { // start at one, never changing to no damage sprite
            if (_currentHealth <= _healthThresholds[i] && _currentDamageSpriteIndex < i) {
                _damageSpriteRenderer.sprite = _damageSprites[i];
                _currentDamageSpriteIndex = i;
                break;
            }
        }
    }

    public void Death() {
        StartCoroutine(DeathRoutine());
    }

    private IEnumerator DeathRoutine() {
        yield return new WaitForSeconds(_deathAnimDelay);
        _damageSpriteRenderer.sprite = _damageSprites[_damageSprites.Length - 1];
        _currentDamageSpriteIndex = _damageSprites.Length - 1;
        Anim.Play(ANIM_BOOL_DEATH_HASH);
    }
}
