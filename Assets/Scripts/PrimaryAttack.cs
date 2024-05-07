using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimaryAttack : MonoBehaviour
{
    private PlayerControls playerControls;
    private Animator myAnimator;
    public bool IsAttacking { get { return isAttacking; } }
    private bool isAttacking = false;

    private void Awake() {
        playerControls = new PlayerControls();

        myAnimator = GetComponent<Animator>();
    }

    private void Start() {
        playerControls.Combat.PrimaryAttack.started += _ => Attack();
    }

    private void OnEnable() {
        playerControls.Enable();
    }

    private void OnDisable() {
        playerControls.Disable();
    }    

    private void Attack() {
        if (!isAttacking) {
            isAttacking = true;
            myAnimator.SetTrigger("PrimaryAttack");
        }
    }

    private void EndAttack () {
        isAttacking = false;
    }

}
