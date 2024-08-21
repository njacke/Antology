using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Bite : Ability, IAbilityAction, IAbilityEnd
{
    private BoxCollider2D _myCollider;

    private void Awake() {
        _myCollider = GetComponent<BoxCollider2D>();
    }

    private void Start() {
        _myCollider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("Checking for IDamagable");
        IDamagable damagable = other.GetComponent<IDamagable>();
        damagable?.TakeDamage(AbilityInfo.Damage);
    }

    public void AbilityAction() {
        _myCollider.enabled = true;
    }

    public void AbilityEnd() {
        _myCollider.enabled = false;
    }
}
