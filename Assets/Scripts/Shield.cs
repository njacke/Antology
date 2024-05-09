using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField] private float shieldDuration = 1f;
    [SerializeField] private float shieldCooldown = 3f;
    private float shieldLifetime;
    public float ShieldCooldown { get { return shieldCooldown; } }

    private void Update() {
        shieldLifetime += Time.deltaTime;
        if (shieldLifetime > shieldDuration) {
            Destroy(gameObject);
            Debug.Log("Shield expired.");        
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("Shield collision triggered");
        // if enemy projectile/attack -> destroy shield
    }
}
