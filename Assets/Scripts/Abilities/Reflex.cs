using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reflex : Ability, IAbilityAction
{
    private PlayerController _playerController;

    private void Start() {
        _playerController = GetComponentInParent<PlayerController>();
    }

    public void AbilityAction() {
        StartCoroutine(ReduceDashCooldownRoutine());
    }

    private IEnumerator ReduceDashCooldownRoutine() {
        _playerController.DashCooldown = 0f;
        _playerController.DashCooldownRemaining = 0f;

        yield return new WaitForSeconds(AbilityInfo.Duration);
        _playerController.SetDefaultDashCooldown();
    }
}
