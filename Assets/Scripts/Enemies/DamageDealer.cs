using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    public static Action<int> OnDamageDealtToPlayer;
    [SerializeField] private int _damageAmount = 1;
    
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.GetComponent<PlayerController>()) {
            OnDamageDealtToPlayer?.Invoke(_damageAmount);
            Debug.Log("Player took damage");
        }
    }
}
