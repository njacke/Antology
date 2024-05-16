using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : Equipment
{
    [SerializeField] private AbilityInfo _abilityInfo;
    public AbilityInfo AbilityInfo { get { return _abilityInfo; } }
    public override EquipmentType Type { get { return EquipmentType.Ability; } }
    public enum AbilitySpeed {
        Slow,
        Normal,
        Fast
    }
}
