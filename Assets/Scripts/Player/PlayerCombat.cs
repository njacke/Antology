using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerCombat : MonoBehaviour
{
    public static Action<EquipmentManager.EquipmentSlot> OnCooldownStart;
    public static Action<EquipmentManager.EquipmentSlot> OnCooldownEnd;

    [SerializeField] private Ability _headAbility;  
    [SerializeField] private Ability _topAbility;  
    [SerializeField] private Ability _midAbility;  
    [SerializeField] private Ability _botAbility;

    private PlayerControls _playerControls;
    private PlayerController _playerController;
    private BodyAnimator _bodyAnimator;
    private EquipmentManager _equipmentManager;

    private float _primaryAttackCooldownRemaining = 0f;
    private float _secondaryAttackCooldownRemaining = 0f;
    private float _utilitySkillCooldownRemaining = 0f;
    private float _defenseSkillCooldownRemaining = 0f;
    private bool _primaryAttackIsOnCooldown = false;
    private bool _secondaryAttackIsOnCooldown = false;
    private bool _defenseSkillIsOnCooldown = false;
    private bool _utilitySkillIsOnCooldown = false;

    private bool _canAttack = true;
    public bool CanAttack {set { _canAttack = value; } }

    private void Awake() {
        _playerControls = new PlayerControls();
        _playerController = GetComponent<PlayerController>();
        _bodyAnimator = GetComponent<BodyAnimator>();
        _equipmentManager = GetComponent<EquipmentManager>();
    }

    private void Start() {
        _playerControls.Combat.PrimaryAttack.started += _ => PrimaryAttack();
        _playerControls.Combat.SecondaryAttack.started += _ => SecondaryAttack();
        _playerControls.Combat.DefenseSkill.started += _ => DefenseSkill();
        _playerControls.Combat.UtilitySkill.started += _ => UtilitySkill();
    }

    private void Update() {
        TrackAllCooldowns();
    }

    private void OnEnable() {
        _playerControls.Enable();
        EquipmentManager.OnEquipmentChange += GetAbilitiesFromEquipment;
    }

    private void OnDisable() {
        _playerControls.Disable();
        EquipmentManager.OnEquipmentChange -= GetAbilitiesFromEquipment;
    }    

    public void GetAbilitiesFromEquipment() {
        _headAbility = _equipmentManager.HeadAbility;
        _topAbility = _equipmentManager.TopAbility;
        _midAbility = _equipmentManager.MidAbility;
        _botAbility = _equipmentManager.BotAbility;
    }

    // PRIMARY ATTACK
    private void PrimaryAttack() {
        if (GameManager.Instance.IsGameActive && _canAttack && !_primaryAttackIsOnCooldown && _headAbility != null) {
            StartCoroutine(PrimaryAttackRoutine());
        }
    }

    private IEnumerator PrimaryAttackRoutine() {
        _canAttack = false;
        _bodyAnimator.PrimaryAttackBodyAnimation(_headAbility.AbilityInfo.Speed);
        Debug.Log("Using PrimaryAttack.");
        
        yield return new WaitForSeconds(_playerController.MovementGradePeriod);
        _playerController.MovementLocked = true;        
    }

    private void PrimaryAttackActionAnimEvent() {
        (_headAbility as IAbilityAction)?.AbilityAction();
        SetCooldown(_headAbility, ref _primaryAttackCooldownRemaining, ref _primaryAttackIsOnCooldown);
    }

    private void PrimaryAttackEndAnimEvent() {
        (_headAbility as IAbilityEnd)?.AbilityEnd();
        EnableControls();
    }

    // SECONDARY ATTACK
    private void SecondaryAttack() {
        if (GameManager.Instance.IsGameActive && _canAttack && !_secondaryAttackIsOnCooldown && _topAbility != null) {
            StartCoroutine(SecondaryAttackRoutine());
        }
    }

    private IEnumerator SecondaryAttackRoutine() {
        _canAttack = false;
        _bodyAnimator.SecondaryAttackBodyAnimation(_topAbility.AbilityInfo.Speed);
        Debug.Log("Using SecondaryAttack.");
        
        yield return new WaitForSeconds(_playerController.MovementGradePeriod);
        _playerController.MovementLocked = true;        
    }

    private void SecondaryAttackActionAnimEvent() {
        (_topAbility as IAbilityAction)?.AbilityAction();

        SetCooldown(_topAbility, ref _secondaryAttackCooldownRemaining, ref _secondaryAttackIsOnCooldown);
    }


    private void SecondaryAttackEndAnimEvent() {
        (_topAbility as IAbilityEnd)?.AbilityEnd();
        EnableControls();
    }

    // DEFENSE SKILL
    private void DefenseSkill() {
        if (GameManager.Instance.IsGameActive && _canAttack && !_defenseSkillIsOnCooldown && _midAbility != null) {
            StartCoroutine(DefenseSkillRoutine());
        }
    }

    private IEnumerator DefenseSkillRoutine() {
        _canAttack = false;
        _bodyAnimator.DefenseSkillBodyAnimation(_midAbility.AbilityInfo.Speed);
        Debug.Log("Using DefenseSkill.");

        yield return new WaitForSeconds(_playerController.MovementGradePeriod);
        _playerController.MovementLocked = true;        
    }

    private void DefenseSkillActionAnimEvent() {
        (_midAbility as IAbilityAction)?.AbilityAction();

        SetCooldown(_midAbility, ref _defenseSkillCooldownRemaining, ref _defenseSkillIsOnCooldown);
    }

    private void DefenseSkillEndAnimEvent() {
        (_midAbility as IAbilityEnd)?.AbilityEnd();
        EnableControls();
    }

    // UTILITY SKILL
    private void UtilitySkill() {
        if (GameManager.Instance.IsGameActive && _canAttack && !_utilitySkillIsOnCooldown && _botAbility != null) {
            StartCoroutine(UtilitySkillRoutine());
        }
    }

    private IEnumerator UtilitySkillRoutine() {
        _canAttack = false;
        _bodyAnimator.UtilitySkillBodyAnimation(_botAbility.AbilityInfo.Speed);
        Debug.Log("Using UtilitySkill.");

        yield return new WaitForSeconds(_playerController.MovementGradePeriod);
        _playerController.MovementLocked = true;
    }

    private void UtilitySkillActionAnimEvent() {
        (_botAbility as IAbilityAction)?.AbilityAction();

        SetCooldown(_botAbility, ref _utilitySkillCooldownRemaining, ref _utilitySkillIsOnCooldown);
    }

    private void UtilitySkillEndAnimEvent() {
        (_botAbility as IAbilityEnd)?.AbilityEnd();
        EnableControls();
    }

    // OTHER
    private void EnableControls() {
        _canAttack = true;
        _playerController.MovementLocked = false;
    }

    // COOLDOWNS
    private void SetCooldown(Ability ability, ref float cooldownRemaining, ref bool isOnCooldown) {
        if (ability.AbilityInfo.Cooldown > 0f) {
            cooldownRemaining = ability.AbilityInfo.Cooldown;
            isOnCooldown = true;
            OnCooldownStart?.Invoke(ability.AbilityInfo.Slot);
        }
    }

    private void TrackCooldown(Ability ability, ref float cooldownRemaining, ref bool isOnCooldown, string debugString) {
        if (cooldownRemaining > 0f) {
            cooldownRemaining -= Time.deltaTime;
        }
        else if (isOnCooldown) {
            isOnCooldown = false;
            OnCooldownEnd?.Invoke(ability.AbilityInfo.Slot);
            Debug.Log(debugString);
        }
    }

    private void TrackAllCooldowns() {
        string debugPA = "PrimaryAttack is ready to use";
        TrackCooldown(_headAbility, ref _primaryAttackCooldownRemaining, ref _primaryAttackIsOnCooldown, debugPA);

        string debugSA = "SecondaryAttack is ready to use";
        TrackCooldown(_topAbility, ref _secondaryAttackCooldownRemaining, ref _secondaryAttackIsOnCooldown, debugSA);

        string debugDS = "DefenseSkill is ready to use";
        TrackCooldown(_midAbility, ref _defenseSkillCooldownRemaining, ref _defenseSkillIsOnCooldown, debugDS);

        string debugUS = "UtilitySkill is ready to use";
        TrackCooldown(_botAbility, ref _utilitySkillCooldownRemaining, ref _utilitySkillIsOnCooldown, debugUS);
    }
}
