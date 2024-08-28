using UnityEngine;

[CreateAssetMenu(menuName = "New Ability")]

public class AbilityInfo : ScriptableObject
{
    public int ID;
    public string Name;
    public EquipmentManager.EquipmentSlot Slot;
    public Ability.AbilitySpeed Speed;
    public float Cooldown;
    public float Duration;
    public int Damage;
    public Sprite ReadySprite;
    public Sprite CdSprite;
}
