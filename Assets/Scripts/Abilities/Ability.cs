using UnityEngine;

public class Ability : Equipment
{
    [SerializeField] private AbilityInfo _abilityInfo;
    public AbilityInfo AbilityInfo { get { return _abilityInfo; } }
    public override EquipmentType Type { get { return EquipmentType.Ability; } }
    
    private SpriteRenderer _spriteRenderer;

    public enum AbilitySpeed {
        Slow,
        Normal,
        Fast
    }

    private void Awake() {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        SetReadySprite();
    }

    private void OnEnable() {
        PlayerCombat.OnCooldownStart += PlayerCombat_OnCooldownStart;
        PlayerCombat.OnCooldownEnd += PlayerCombat_OnCooldownEnd;
    }

    private void OnDisable() {
        PlayerCombat.OnCooldownStart -= PlayerCombat_OnCooldownStart;
        PlayerCombat.OnCooldownEnd -= PlayerCombat_OnCooldownEnd;        
    }

    private void PlayerCombat_OnCooldownStart(EquipmentManager.EquipmentSlot slot) {
        if (slot == _abilityInfo.Slot) {
            SetCDSprite();
        }
    }

    private void PlayerCombat_OnCooldownEnd(EquipmentManager.EquipmentSlot slot) {
        if (slot == _abilityInfo.Slot) {
            SetReadySprite();
        }
    }

    private void SetReadySprite() {
        _spriteRenderer.sprite = _abilityInfo.ReadySprite;
    }

    private  void SetCDSprite() {
        _spriteRenderer.sprite = _abilityInfo.CdSprite;
    }
}
