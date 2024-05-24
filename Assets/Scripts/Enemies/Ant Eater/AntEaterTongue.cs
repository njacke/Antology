using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntEaterTongue : MonoBehaviour
{
    [SerializeField] private float _tongueGrowTime = 1f;
    [SerializeField] private float _minTongueRange = 1f;
    [SerializeField] private float _maxTongueRange = 4f;
    [SerializeField] private float _tongueCooldown = 2f;
    [SerializeField] private float _tongueTelegraphTime = .3f;
    [SerializeField] private float _tongueTelegraphRange = .5f;
    private float _tongueCooldownRemaining;
    private float _linearT = 0f;
    private SpriteRenderer _spriteRenderer;
    private BoxCollider2D _boxCollider2D;
    private Transform _player;
    private PlayerController _playerController;
    private PlayerCombat _playerCombat;
    private Rigidbody2D _playerRb;
    public bool IsActive { get; private set; } = false;
    private bool _playerHit = false;

    private void Awake() {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _boxCollider2D = GetComponent<BoxCollider2D>();

        _tongueCooldownRemaining = _tongueCooldown;
    }

    private void Start () {
        _player = FindAnyObjectByType<PlayerController>().transform;
        _playerController = _player.GetComponent<PlayerController>();
        _playerRb = _player.GetComponent<Rigidbody2D>();
        _playerCombat = _player.GetComponent<PlayerCombat>();

        //StartCoroutine(TongueTestRoutine());
    }

    private void Update() {
        //Debug.Log("Tongue CD" + _tongueCooldownRemaining);
        //Debug.Log(IsActive);
        _tongueCooldownRemaining -= Time.deltaTime;
        if (IsActive && _tongueCooldownRemaining <= 0f && IsPlayerInRange()) {
            StartCoroutine(UseTongueRoutine());
        }
    }

    private void FixedUpdate() {
        if (_playerHit) {
            //Debug.Log("Tongue player rb being used");
            _playerRb.MovePosition(new Vector2(Mathf.Lerp(_playerRb.transform.position.x, this.transform.position.x, _linearT),
                                               Mathf.Lerp(_playerRb.transform.position.y, this.transform.position.y, _linearT)));
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.GetComponent<PlayerController>()) {
            //Debug.Log("Player was hit.");
            _playerHit = true;
            _playerController.MovementLocked = true;
            _playerCombat.CanAttack = false;
        }
    }

    private IEnumerator TongueTestRoutine() {
        for (int i = 0; i < 10; i++) {
            yield return new WaitForSeconds(3f);
            StartCoroutine(UseTongueRoutine());
        }
    }

    private IEnumerator UseTongueRoutine() {
        _tongueCooldownRemaining = _tongueCooldown;
        
        float startSpriteRendererX = _spriteRenderer.size.x;
        float startBoxCollider2DSizeX = _boxCollider2D.size.x;
        float startBoxCollider2DOffsetX = _boxCollider2D.offset.x;        

        float timePassed = 0f;

        while (_spriteRenderer.size.x < _tongueTelegraphRange && !_playerHit) {
            timePassed += Time.deltaTime;
            _linearT = timePassed / _tongueGrowTime;

            _spriteRenderer.size = new Vector2(Mathf.Lerp(startSpriteRendererX, _tongueTelegraphRange, _linearT), _spriteRenderer.size.y);
            
            _boxCollider2D.size = new Vector2(Mathf.Lerp(startBoxCollider2DSizeX, _tongueTelegraphRange, _linearT), _boxCollider2D.size.y);

            _boxCollider2D.offset = new Vector2(Mathf.Lerp(startBoxCollider2DOffsetX, _tongueTelegraphRange, _linearT) / 2, _boxCollider2D.offset.y);  

            yield return null;

        }

        yield return new WaitForSeconds(_tongueTelegraphTime);

        _boxCollider2D.enabled = true;

        timePassed = 0f;

        while (_spriteRenderer.size.x < _maxTongueRange && !_playerHit) {
            timePassed += Time.deltaTime;
            _linearT = timePassed / _tongueGrowTime;

            _spriteRenderer.size = new Vector2(Mathf.Lerp(startSpriteRendererX, _maxTongueRange, _linearT), _spriteRenderer.size.y);
            
            _boxCollider2D.size = new Vector2(Mathf.Lerp(startBoxCollider2DSizeX, _maxTongueRange, _linearT), _boxCollider2D.size.y);

            _boxCollider2D.offset = new Vector2(Mathf.Lerp(startBoxCollider2DOffsetX, _maxTongueRange, _linearT) / 2, _boxCollider2D.offset.y);  

            yield return null;

        }

        timePassed = 0f;

        while (_spriteRenderer.size.x > startSpriteRendererX) {
            timePassed += Time.deltaTime;
            _linearT = timePassed / _tongueGrowTime;

            _spriteRenderer.size = new Vector2(Mathf.Lerp(_maxTongueRange, startSpriteRendererX, _linearT), _spriteRenderer.size.y);
            
            _boxCollider2D.size = new Vector2(Mathf.Lerp(_maxTongueRange, startBoxCollider2DSizeX, _linearT), _boxCollider2D.size.y);

            _boxCollider2D.offset = new Vector2(Mathf.Lerp(_maxTongueRange, startBoxCollider2DOffsetX, _linearT) / 2, _boxCollider2D.offset.y);  

            yield return null;
        }

        _boxCollider2D.enabled = false;
        _playerHit = false;
        _playerController.MovementLocked = false;
        _playerCombat.CanAttack = true;
    }

    private bool IsPlayerInRange() {
        var distance = Vector3.Distance(this.transform.position, _player.position);
        return distance <= _maxTongueRange && distance >= _minTongueRange;
    }

    public void SetIsActive(bool value) {
        //Debug.Log("Tongue is active called.");
        IsActive = value;
    }
}
