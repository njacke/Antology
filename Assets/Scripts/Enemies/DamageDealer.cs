using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    private PlayerHealth _playerHealth;

    private void Start() {
        _playerHealth = FindObjectOfType<PlayerHealth>();
        
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.GetComponent<PlayerController>()) {
            _playerHealth.TakeDamage(1);
            Debug.Log("Player took damage");
        }
    }
}
