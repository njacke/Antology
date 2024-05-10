using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Bite : MonoBehaviour
{
    private BoxCollider2D myCollider;

    private void Awake() {
        myCollider = GetComponent<BoxCollider2D>();
    }

    private void Start() {
        myCollider.enabled = false;
    }

    private void OnEnable() {
        PlayerCombat.OnPrimaryAttackAction += AttackAction;
        PlayerCombat.OnPrimaryAttackEnd += AttackEnd;
    }

    private void OnDisable() {
        PlayerCombat.OnPrimaryAttackAction -= AttackAction;
        PlayerCombat.OnPrimaryAttackEnd -= AttackEnd;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("Target hit with bite.");
    }

    private void AttackAction() {
        myCollider.enabled = true;
    }

    private void AttackEnd() {
        myCollider.enabled = false;
    }
}
