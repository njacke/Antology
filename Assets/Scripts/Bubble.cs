using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    [SerializeField] private GameObject shieldPrefab;
    [SerializeField] float shieldDuration = 1f;
    [SerializeField] float skillCooldown = 5f;

    private Transform player;
    private PlayerCombat playerCombat;

    private void Start() {
        player = FindObjectOfType<PlayerController>().transform;
        playerCombat = player.GetComponent<PlayerCombat>();
    }

    private void OnEnable() {
        PlayerCombat.OnDefenseSkillAction += SkillAction;
        PlayerCombat.OnDefenseSkillEnd += SkillEnd;
    }

    private void OnDisable() {
        PlayerCombat.OnDefenseSkillAction -= SkillAction;
        PlayerCombat.OnDefenseSkillEnd -= SkillEnd;
    }

    private void SkillAction() {
        SpawnShield();
    }

    private void SkillEnd() {
        // do smth if needed
    }

    private void SpawnShield() {
        var spawnedShield = Instantiate(shieldPrefab, this.transform.position, Quaternion.identity);
        spawnedShield.transform.SetParent(player);
        spawnedShield.GetComponent<Shield>().ShieldDuration = shieldDuration;

        playerCombat.DefenseSkillCooldownRemaining = skillCooldown;
        playerCombat.DefenseSkillIsOnCooldown = true;
    }
}
