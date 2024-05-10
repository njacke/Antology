using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using System;
using UnityEditor.ShaderGraph.Internal;

public class PlayerCombat : MonoBehaviour
{
    public float DefenseSkillCooldownRemaining { set { defenseSkillCooldownRemaining = value; } }
    public bool DefenseSkillIsOnCooldown { set { defenseSkillIsOnCooldown = value; } }
    public float UtilitySkillCooldownRemaining { set { utilitySkillCooldownRemaining = value; } }
    public bool UtilitySkillIsOnCooldown { set { utilitySkillIsOnCooldown = value; } }

    private float defenseSkillCooldownRemaining = 0f;
    private bool defenseSkillIsOnCooldown = false;
    private float utilitySkillCooldownRemaining = 0f;
    private bool utilitySkillIsOnCooldown = false;

    private bool isBusy = false;

    private PlayerControls playerControls;
    private PlayerController playerController;

    public static event Action OnPrimaryAttackUsed;
    public static event Action OnPrimaryAttackAction;
    public static event Action OnPrimaryAttackEnd;

    public static event Action OnSecondaryAttackUsed;
    public static event Action OnSecondaryAttackAction;
    public static event Action OnSecondaryAttackEnd;

    public static event Action OnDefenseSkillUsed;
    public static event Action OnDefenseSkillAction;
    public static event Action OnDefenseSkillEnd;

    public static event Action OnUtilitySkillUsed;
    public static event Action OnUtilitySkillAction;
    public static event Action OnUtilitySkillEnd;

    private void Awake() {
        playerControls = new PlayerControls();
    }

    private void Start() {
        playerControls.Combat.PrimaryAttack.started += _ => PrimaryAttack();
        playerControls.Combat.SecondaryAttack.started += _ => SecondaryAttack();
        playerControls.Combat.DefenseSkill.started += _ => DefenseSkill();
        playerControls.Combat.UtilitySkill.started += _ => UtilitySkill();

        playerController = FindObjectOfType<PlayerController>();
    }

    private void Update()
    {
        TrackDefenseSkillCooldown();
        TrackUtilitySkillCooldown();
    }

    private void OnEnable() {
        playerControls.Enable();
    }

    private void OnDisable() {
        playerControls.Disable();
    }    

    // PRIMARY ATTACK
    private void PrimaryAttack() {
        if (!isBusy) {
            StartCoroutine(PrimaryAttackRoutine());
        }
    }

    private IEnumerator PrimaryAttackRoutine() {
        isBusy = true;
        OnPrimaryAttackUsed?.Invoke();
        Debug.Log("Using PrimaryAttack.");
        
        yield return new WaitForSeconds(playerController.MovementGradePeriod);
        playerController.MovementLocked = true;        
    }

    private void PrimaryAttackActionAnimEvent() {
        OnPrimaryAttackAction?.Invoke();
    }

    private void PrimaryAttackEndAnimEvent() {
        OnPrimaryAttackEnd?.Invoke();
        EnableControls();
    }

    // SECONDARY ATTACK
    private void SecondaryAttack() {
        if (!isBusy) {
            StartCoroutine(SecondaryAttackRoutine());
        }
    }

    private IEnumerator SecondaryAttackRoutine() {
        isBusy = true;
        OnSecondaryAttackUsed?.Invoke();
        Debug.Log("Using SecondaryAttack.");
        
        yield return new WaitForSeconds(playerController.MovementGradePeriod);
        playerController.MovementLocked = true;        
    }

    private void SecondaryAttackActionAnimEvent() {
        OnSecondaryAttackAction?.Invoke();
    }

    private void SecondaryAttackEndAnimEvent() {
        OnSecondaryAttackEnd?.Invoke();
        EnableControls();
    }

    // DEFENSE SKILL
    private void DefenseSkill() {
        if (!isBusy && !defenseSkillIsOnCooldown) {
            StartCoroutine(DefenseSkillRoutine());
        }
    }

    private IEnumerator DefenseSkillRoutine() {
        isBusy = true;
        OnDefenseSkillUsed?.Invoke();
        Debug.Log("Using DefenseSkill.");

        yield return new WaitForSeconds(playerController.MovementGradePeriod);
        playerController.MovementLocked = true;        
    }

    private void TrackDefenseSkillCooldown() {
        if (defenseSkillCooldownRemaining > 0f) {
            defenseSkillCooldownRemaining -= Time.deltaTime;
        }
        else if (defenseSkillIsOnCooldown) {
            defenseSkillIsOnCooldown = false;
            Debug.Log("DefenseSkill is ready to use.");
        }
    }

    private void DefenseSkillActionAnimEvent() {
        OnDefenseSkillAction?.Invoke();
    }

    private void DefenseSkillEndAnimEvent() {
        OnDefenseSkillEnd?.Invoke();
        EnableControls();
    }

    // UTILITY SKILL
    private void UtilitySkill() {
        if (!isBusy && !utilitySkillIsOnCooldown) {
            StartCoroutine(UtilitySkillRoutine());
        }
    }

    private IEnumerator UtilitySkillRoutine() {
        isBusy = true;
        OnUtilitySkillUsed?.Invoke();
        Debug.Log("Using UtilitySkill.");

        yield return new WaitForSeconds(playerController.MovementGradePeriod);
        playerController.MovementLocked = true;
    }

    private void TrackUtilitySkillCooldown() {
        if (utilitySkillCooldownRemaining > 0f) {
            utilitySkillCooldownRemaining -= Time.deltaTime;
        }
        else if (utilitySkillIsOnCooldown) {
            utilitySkillIsOnCooldown = false;
            Debug.Log("UtilitySkill is ready to use.");
        }
    }

    private void UtilitySkillActionAnimEvent() {
        OnUtilitySkillAction?.Invoke();
    }

    private void UtilitySkillEndAnimEvent() {
        OnUtilitySkillEnd?.Invoke();
        EnableControls();
    }

    // OTHER
    private void EnableControls() {
        isBusy = false;
        playerController.MovementLocked = false;
    }

    private void TrackCooldown(float cooldownRemaining, bool isOnCooldown) {
        if (cooldownRemaining > 0f) {
            cooldownRemaining -= Time.deltaTime;
        }
        else if (isOnCooldown) {
            isOnCooldown = false;
        }
    }
}
