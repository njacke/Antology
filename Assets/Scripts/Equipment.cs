using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Equipment : MonoBehaviour
{
    public abstract EquipmentType Type { get; }
    public enum EquipmentType {
        Ability,
        Armor
    }
}
