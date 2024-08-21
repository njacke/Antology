using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public static Action<Shield> OnShieldActivated;
    public static Action<Shield> OnShieldDeactivated;
    public float ShieldDuration { set { _shieldDuration = value; _shieldDurationAssigned = true; } }
    private float _shieldDuration;
    private float _shieldLifetime;
    private bool _shieldDurationAssigned = false;

    private void Start() {
        Debug.Log("Shield activated.");
        OnShieldActivated?.Invoke(this);
    }

    private void Update() {
        if (_shieldDurationAssigned) {
            _shieldLifetime += Time.deltaTime;

            if (_shieldLifetime > _shieldDuration) {
                OnShieldDeactivated?.Invoke(this);
                Debug.Log("Shield expired.");        
            }
        }
    }
}
