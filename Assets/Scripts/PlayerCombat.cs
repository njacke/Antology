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

    private PlayerControls playerControls;
    private Animator myAnimator;
    private SpriteRenderer topWeaponSpriteRenderer;

    public bool ControlsLocked { get { return controlsLocked; } }
    private bool controlsLocked = false;

    readonly int PRIMARY_ATTACK_HASH = Animator.StringToHash("PrimaryAttack");
    readonly int SECONDARY_ATTACK_HASH = Animator.StringToHash("SecondaryAttack");
    readonly int DEFENSE_SKILL_HASH = Animator.StringToHash("DefenseSkill");
    readonly int UTILITY_SKILL_HASH = Animator.StringToHash("UtilitySkill");

    private void Awake() {
        playerControls = new PlayerControls();

        myAnimator = GetComponent<Animator>();
    }

    private void Start() {
        playerControls.Combat.PrimaryAttack.started += _ => PrimaryAttack();
        playerControls.Combat.SecondaryAttack.started += _ => SecondaryAttack();
        playerControls.Combat.DefenseSkill.started += _ => DefenseSkill();
        playerControls.Combat.UtilitySkill.started += _ => UtilitySkill();

        topWeaponSpriteRenderer = topWeapon.GetComponent<SpriteRenderer>();
        topWeaponSpriteRenderer.sprite = topWeaponSprite;
    }

    private void OnEnable() {
        playerControls.Enable();
    }

    private void OnDisable() {
        playerControls.Disable();
    }    

    private void PrimaryAttack() {
        if (!controlsLocked) {
            controlsLocked = true;
            myAnimator.SetTrigger(PRIMARY_ATTACK_HASH);
            Debug.Log("Using PrimaryAttack.");
        }
    }

    private void SecondaryAttack() {
        if (!controlsLocked) {
            controlsLocked = true;
            myAnimator.SetTrigger(SECONDARY_ATTACK_HASH);
            Debug.Log("Using SecondaryAttack.");
        }
    }

    private void SpawnTopWeaponProjectilesAnimEvent(){

        Quaternion projectileRotation;

        projectileRotation = transform.rotation * Quaternion.Euler(0, 0, 90f);

        Instantiate(topProjectilePrefab, topSpawnLeft.position, projectileRotation);
        Instantiate(topProjectilePrefab, topSpawnRight.position, projectileRotation);
    }

    private void DefenseSkill() {
        if (!controlsLocked) {
            controlsLocked = true;
            myAnimator.SetTrigger(DEFENSE_SKILL_HASH);
            Debug.Log("Using DefenseSkill.");
        }
    }

    private void UtilitySkill() {
        if (!controlsLocked) {
            controlsLocked = true;
            myAnimator.SetTrigger(UTILITY_SKILL_HASH);
            Debug.Log("Using UtilitySkill.");
        }
    }

    private void EnableControlsAnimEvent () {
        controlsLocked = false;
    }

}
