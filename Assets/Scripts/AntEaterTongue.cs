using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntEaterTongue : MonoBehaviour
{
    [SerializeField] private float _tongueGrowTime = 1f;
    [SerializeField] private float _tongueRange = 3f;
    private float _linearT = 0f;
    private SpriteRenderer _spriteRenderer;
    private BoxCollider2D _boxCollider2D;
    private PlayerController _playerController;
    private Rigidbody2D _playerRb;

    private bool _playerHit = false;

    private void Awake() {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void Start () {
        _playerController = FindObjectOfType<PlayerController>();
        _playerRb = _playerController.GetComponent<Rigidbody2D>();

        StartCoroutine(TongueTestRoutine());
    }

    private void FixedUpdate() {
        if (_playerHit) {
            _playerRb.MovePosition(new Vector2(Mathf.Lerp(_playerRb.transform.position.x, this.transform.position.x, _linearT),
                                               Mathf.Lerp(_playerRb.transform.position.y, this.transform.position.y, _linearT)));
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.GetComponent<PlayerController>()) {
            Debug.Log("Player was hit.");
            _playerHit = true;
            _playerController.MovementLocked = true;
        }
    }

    private IEnumerator TongueTestRoutine() {
        for (int i = 0; i < 10; i++) {
            StartCoroutine(GrowAndRetreatTongueRoutine());
            yield return new WaitForSeconds(3f);
        }
    }

    private IEnumerator GrowAndRetreatTongueRoutine() {
        _boxCollider2D.enabled = true;
        
        float startSpriteRendererX = _spriteRenderer.size.x;
        float startBoxCollider2DSizeX = _boxCollider2D.size.x;
        float startBoxCollider2DOffsetX = _boxCollider2D.offset.x;

        float timePassed = 0f;

        while (_spriteRenderer.size.x < _tongueRange && !_playerHit) {
            timePassed += Time.deltaTime;
            _linearT = timePassed / _tongueGrowTime;

            _spriteRenderer.size = new Vector2(Mathf.Lerp(startSpriteRendererX, _tongueRange, _linearT), _spriteRenderer.size.y);
            
            _boxCollider2D.size = new Vector2(Mathf.Lerp(startBoxCollider2DSizeX, _tongueRange, _linearT), _boxCollider2D.size.y);

            _boxCollider2D.offset = new Vector2(Mathf.Lerp(startBoxCollider2DOffsetX, _tongueRange, _linearT) / 2, _boxCollider2D.offset.y);  

            yield return null;

        }

        Debug.Log("Start sprite renderer X: " + startSpriteRendererX);
        Debug.Log("Current sprite renderer X: " + _spriteRenderer.size.x);
        timePassed = 0f;

        while (_spriteRenderer.size.x > startSpriteRendererX) {
            //Debug.Log("Retreating Tongue");
            timePassed += Time.deltaTime;
            _linearT = timePassed / _tongueGrowTime;

            _spriteRenderer.size = new Vector2(Mathf.Lerp(_tongueRange, startSpriteRendererX, _linearT), _spriteRenderer.size.y);
            
            _boxCollider2D.size = new Vector2(Mathf.Lerp(_tongueRange, startBoxCollider2DSizeX, _linearT), _boxCollider2D.size.y);

            _boxCollider2D.offset = new Vector2(Mathf.Lerp(_tongueRange, startBoxCollider2DOffsetX, _linearT) / 2, _boxCollider2D.offset.y);  

            yield return null;
        }

        _playerHit = false;
        _playerController.MovementLocked = false;
        _boxCollider2D.enabled = false;
    }
}
