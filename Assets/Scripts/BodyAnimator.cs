using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BodyAnimator : MonoBehaviour
{
    private Animator _myAnimator;

    readonly int PRIMARY_ATTACK_SLOW_HASH = Animator.StringToHash("PASlow");
    readonly int PRIMARY_ATTACK_NORMAL_HASH = Animator.StringToHash("PANormal");
    readonly int PRIMARY_ATTACK_FAST_HASH = Animator.StringToHash("PAFast");

    readonly int SECONDARY_ATTACK_SLOW_HASH = Animator.StringToHash("SASlow");
    readonly int SECONDARY_ATTACK_NORMAL_HASH = Animator.StringToHash("SANormal");
    readonly int SECONDARY_ATTACK_FAST_HASH = Animator.StringToHash("SAFast");

    readonly int DEFENSE_SKILL_SLOW_HASH = Animator.StringToHash("DSSlow");
    readonly int DEFENSE_SKILL_NORMAL_HASH = Animator.StringToHash("DSNormal");
    readonly int DEFENSE_SKILL_FAST_HASH = Animator.StringToHash("DSFast");

    readonly int UTILITY_SKILL_SLOW_HASH = Animator.StringToHash("USSlow");
    readonly int UTILITY_SKILL_NORMAL_HASH = Animator.StringToHash("USNormal");
    readonly int UTILITY_SKILL_FAST_HASH = Animator.StringToHash("USFast");

    private void Awake() {
        _myAnimator = GetComponent<Animator>();
    }

    public void PrimaryAttackBodyAnimation(EquipmentManager.AbilitySpeed abilitySpeed) {
        switch (abilitySpeed) {
            case EquipmentManager.AbilitySpeed.Slow:
                _myAnimator.SetTrigger(PRIMARY_ATTACK_SLOW_HASH);
                break;
            case EquipmentManager.AbilitySpeed.Normal:
                _myAnimator.SetTrigger(PRIMARY_ATTACK_NORMAL_HASH);
                break;
            case EquipmentManager.AbilitySpeed.Fast:
                _myAnimator.SetTrigger(PRIMARY_ATTACK_FAST_HASH);
                break;
            default:
                break;
        }   
    }

    public void SecondaryAttackBodyAnimation(EquipmentManager.AbilitySpeed abilitySpeed) {
        Debug.Log("Animator SA entered");
        switch (abilitySpeed) {
            case EquipmentManager.AbilitySpeed.Slow:
                _myAnimator.SetTrigger(SECONDARY_ATTACK_SLOW_HASH);
                break;
            case EquipmentManager.AbilitySpeed.Normal:
                _myAnimator.SetTrigger(SECONDARY_ATTACK_NORMAL_HASH);
                break;
            case EquipmentManager.AbilitySpeed.Fast:
                _myAnimator.SetTrigger(SECONDARY_ATTACK_FAST_HASH);
                break;
            default:
                break;
        }    
    }

    public void DefenseSkillBodyAnimation(EquipmentManager.AbilitySpeed abilitySpeed) {
        switch (abilitySpeed) {
            case EquipmentManager.AbilitySpeed.Slow:
                _myAnimator.SetTrigger(DEFENSE_SKILL_SLOW_HASH);
                break;
            case EquipmentManager.AbilitySpeed.Normal:
                _myAnimator.SetTrigger(DEFENSE_SKILL_NORMAL_HASH);
                break;
            case EquipmentManager.AbilitySpeed.Fast:
                _myAnimator.SetTrigger(DEFENSE_SKILL_FAST_HASH);
                break;
            default:
                break;
        }
    }
    
    public void UtilitySkillBodyAnimation(EquipmentManager.AbilitySpeed abilitySpeed) {
        switch (abilitySpeed) {
            case EquipmentManager.AbilitySpeed.Slow:
                _myAnimator.SetTrigger(UTILITY_SKILL_SLOW_HASH);
                break;
            case EquipmentManager.AbilitySpeed.Normal:
                _myAnimator.SetTrigger(UTILITY_SKILL_NORMAL_HASH);
                break;
            case EquipmentManager.AbilitySpeed.Fast:
                _myAnimator.SetTrigger(UTILITY_SKILL_FAST_HASH);
                break;
            default:
                break;
        }   
    }
}
