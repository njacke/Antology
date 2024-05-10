using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reflex : MonoBehaviour
{
    [SerializeField] private float skillDuration = 3f;
    [SerializeField] private float skillCooldown = 10f;

    private Transform player;
    private PlayerController playerController;
    private PlayerCombat playerCombat;

    private void Start() {
        player = FindObjectOfType<PlayerController>().transform;
        playerController = player.GetComponent<PlayerController>();
        playerCombat = player.GetComponent<PlayerCombat>();    
    }

    private void OnEnable() {
        PlayerCombat.OnUtilitySkillAction += SkillAction;
        PlayerCombat.OnDefenseSkillEnd += SkillEnd;
    }

    private void OnDisable() {
        PlayerCombat.OnUtilitySkillAction -= SkillAction;
        PlayerCombat.OnUtilitySkillEnd -= SkillEnd;
    }

    private void SkillAction() {
        StartCoroutine(ReduceDashCooldownRoutine());
    }

    private void SkillEnd() {
        // do smth if needed
    }

    private IEnumerator ReduceDashCooldownRoutine() {
        playerController.DashCooldown = 0f;
        playerController.DashCooldownRemaining = 0f;
        playerCombat.UtilitySkillCooldownRemaining = skillCooldown;
        playerCombat.UtilitySkillIsOnCooldown = true;

        yield return new WaitForSeconds(skillDuration);
        playerController.SetDefaultDashCooldown();
    }
}
