using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public float ShieldDuration { set { shieldDuration = value; shieldDurationAssigned = true; } }
    private float shieldDuration;
    private float shieldLifetime;
    private bool shieldDurationAssigned = false;

    private void Update() {
        if (shieldDurationAssigned) {
            shieldLifetime += Time.deltaTime;

            if (shieldLifetime > shieldDuration) {
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
