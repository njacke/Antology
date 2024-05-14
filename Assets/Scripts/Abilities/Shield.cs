using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public float ShieldDuration { set { _shieldDuration = value; _shieldDurationAssigned = true; } }
    private float _shieldDuration;
    private float _shieldLifetime;
    private bool _shieldDurationAssigned = false;

    private void Update() {
        if (_shieldDurationAssigned) {
            _shieldLifetime += Time.deltaTime;

            if (_shieldLifetime > _shieldDuration) {
                Destroy(gameObject);
                Debug.Log("Shield expired.");        
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("Shield collision triggered");
        // if enemy projectile/attack -> destroy shield
    }
}
