using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BodyAnimator : MonoBehaviour
{
    private Animator myAnimator;

    readonly int PRIMARY_ATTACK_HASH = Animator.StringToHash("PrimaryAttack");
    readonly int SECONDARY_ATTACK_HASH = Animator.StringToHash("SecondaryAttack");
    readonly int DEFENSE_SKILL_HASH = Animator.StringToHash("DefenseSkill");
    readonly int UTILITY_SKILL_HASH = Animator.StringToHash("UtilitySkill");

    private void Awake() {
        myAnimator = GetComponent<Animator>();
    }

    private void OnEnable() {
        PlayerCombat.OnPrimaryAttackUsed += PrimaryAttackBodyAnimation;
        PlayerCombat.OnSecondaryAttackUsed += SecondaryAttackBodyAnimation;
        PlayerCombat.OnDefenseSkillUsed += DefenseSkillBodyAnimation;
        PlayerCombat.OnUtilitySkillUsed += UtilitySkillBodyAnimation;
    }

    private void OnDisable() {
        PlayerCombat.OnPrimaryAttackUsed -= PrimaryAttackBodyAnimation;
        PlayerCombat.OnSecondaryAttackUsed -= SecondaryAttackBodyAnimation;
        PlayerCombat.OnDefenseSkillUsed -= DefenseSkillBodyAnimation;
        PlayerCombat.OnUtilitySkillUsed -= UtilitySkillBodyAnimation;
    }

    private void PrimaryAttackBodyAnimation() {
        myAnimator.SetTrigger(PRIMARY_ATTACK_HASH);
    }

    private void SecondaryAttackBodyAnimation() {
        myAnimator.SetTrigger(SECONDARY_ATTACK_HASH);
    }

    private void DefenseSkillBodyAnimation() {
        myAnimator.SetTrigger(DEFENSE_SKILL_HASH);
    }
    
    private void UtilitySkillBodyAnimation() {
        myAnimator.SetTrigger(UTILITY_SKILL_HASH);
    }
}
