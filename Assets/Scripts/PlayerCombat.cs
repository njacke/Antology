using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] GameObject topWeapon;
    [SerializeField] Sprite topWeaponSprite;
    [SerializeField] GameObject topProjectilePrefab;
    [SerializeField] Transform topSpawnLeft;
    [SerializeField] Transform topSpawnRight;
    [SerializeField] GameObject midSkill;
    [SerializeField] Sprite midSkillSprite;
    [SerializeField] GameObject midSkillPrefab;
    [SerializeField] GameObject botSkill;
    [SerializeField] Sprite botSkillSprite;
    [SerializeField] float utilitySkillMoveSpeed = 10f;
    [SerializeField] float utilitySkillDuration = 3f;
    [SerializeField] float utilitySkillCooldown = 10f;
    [SerializeField] float movementGracePeriod = .08f;

    private float defenseSkillCooldownRemaining = 0f;
    private bool defenseSkillIsOnCooldown = false;
    private float utilitySkillCooldownRemaining = 0f;
    private bool utilitySkillIsOnCooldown = false;
    private bool isBusy = false;

    private PlayerControls playerControls;
    private Animator myAnimator;
    private SpriteRenderer topWeaponSpriteRenderer;
    private SpriteRenderer midSkillSpriteRenderer;
    private SpriteRenderer botSkillSpriteRenderer;
    private PlayerController playerController;


    readonly int PRIMARY_ATTACK_HASH = Animator.StringToHash("PrimaryAttack");
    readonly int SECONDARY_ATTACK_HASH = Animator.StringToHash("SecondaryAttack");
    readonly int DEFENSE_SKILL_HASH = Animator.StringToHash("DefenseSkill");
    readonly int UTILITY_SKILL_HASH = Animator.StringToHash("UtilitySkill");

    private void Awake() {
        playerControls = new PlayerControls();

        myAnimator = GetComponent<Animator>();

        topWeaponSpriteRenderer = topWeapon.GetComponent<SpriteRenderer>();
        midSkillSpriteRenderer = midSkill.GetComponent<SpriteRenderer>();
        botSkillSpriteRenderer = botSkill.GetComponent<SpriteRenderer>();
    }

    private void Start() {

        playerControls.Combat.PrimaryAttack.started += _ => PrimaryAttack();
        playerControls.Combat.SecondaryAttack.started += _ => SecondaryAttack();
        playerControls.Combat.DefenseSkill.started += _ => DefenseSkill();
        playerControls.Combat.UtilitySkill.started += _ => UtilitySkill();

        topWeaponSpriteRenderer.sprite = topWeaponSprite;
        midSkillSpriteRenderer.sprite = midSkillSprite;
        botSkillSpriteRenderer.sprite = botSkillSprite;

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
        myAnimator.SetTrigger(PRIMARY_ATTACK_HASH);
        Debug.Log("Using PrimaryAttack.");
        
        yield return new WaitForSeconds(movementGracePeriod);
        playerController.MovementLocked = true;        
    }

    // SECONDARY ATTACK
    private void SecondaryAttack() {
        if (!isBusy) {
            StartCoroutine(SecondaryAttackRoutine());
        }
    }

    private IEnumerator SecondaryAttackRoutine() {
        isBusy = true;
        myAnimator.SetTrigger(SECONDARY_ATTACK_HASH);
        Debug.Log("Using SecondaryAttack.");
        
        yield return new WaitForSeconds(movementGracePeriod);
        playerController.MovementLocked = true;        
    }

    private void SpawnSecondaryAttackProjectilesAnimEvent() {
        Quaternion projectileRotation;
        projectileRotation = transform.rotation * Quaternion.Euler(0, 0, 90f);

        Instantiate(topProjectilePrefab, topSpawnLeft.position, projectileRotation);
        Instantiate(topProjectilePrefab, topSpawnRight.position, projectileRotation);
    }

    // DEFENSE SKILL
    private void DefenseSkill() {
        if (!isBusy && !defenseSkillIsOnCooldown) {
            StartCoroutine(DefenseSkillRoutine());
        }
    }

    private IEnumerator DefenseSkillRoutine() {
        isBusy = true;
        myAnimator.SetTrigger(DEFENSE_SKILL_HASH);
        Debug.Log("Using DefenseSkill.");

        yield return new WaitForSeconds(movementGracePeriod);
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

    private void SpawnDefenseSkillShieldAnimEvent() {
        var spawnedShield = Instantiate(midSkillPrefab, this.transform.position, Quaternion.identity);
        spawnedShield.transform.SetParent(this.transform);

        defenseSkillCooldownRemaining = spawnedShield.GetComponent<Shield>().ShieldCooldown;
        defenseSkillIsOnCooldown = true;
    }

    // UTILITY SKILL
    private void UtilitySkill() {
        if (!isBusy && !utilitySkillIsOnCooldown) {
            StartCoroutine(UtilitySkillRoutine());
        }
    }

    private IEnumerator UtilitySkillRoutine() {
        isBusy = true;
        myAnimator.SetTrigger(UTILITY_SKILL_HASH);
        Debug.Log("Using UtilitySkill.");

        yield return new WaitForSeconds(movementGracePeriod);
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

    private void ActivateUtilitySkillAnimEvent() {
        StartCoroutine(ActivateUtilitySkillRoutine());
    }

    private IEnumerator ActivateUtilitySkillRoutine() {
        playerController.MoveSpeed = utilitySkillMoveSpeed;
        utilitySkillCooldownRemaining = utilitySkillCooldown;
        utilitySkillIsOnCooldown = true;

        yield return new WaitForSeconds(utilitySkillDuration);
        playerController.ResetMoveSpeed();
    }

    private void EndAttackAnimEvent() {
        isBusy = false;
        playerController.MovementLocked = false;
    }
}
