using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "New Ability")]

public class AbilityInfo : ScriptableObject
{
    public int ID;
    public string Name;
    public EquipmentManager.EquipmentSlot Slot;
    public EquipmentManager.AbilitySpeed Speed;
    public float Cooldown;
    public float Duration;
    public float Damage;
}
