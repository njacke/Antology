using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reflex : Ability, IAbilityAction
{
    [SerializeField] private ParticleSystem _vfxLeft;
    [SerializeField] private ParticleSystem _vfxRight;

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
        _vfxLeft.Play();
        _vfxRight.Play();

        yield return new WaitForSeconds(AbilityInfo.Duration);
        _vfxLeft.Stop();
        _vfxRight.Stop();
        _playerController.SetDefaultDashCooldown();
    }
}
