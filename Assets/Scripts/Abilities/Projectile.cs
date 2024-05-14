using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 10f;

    private void Update() {
        MoveProjectile();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Destroy(gameObject);
    }

    private void MoveProjectile() {
        transform.Translate(_moveSpeed * Time.deltaTime * Vector3.up);
    }
}
