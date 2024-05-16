using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : Equipment
{
    [SerializeField] private ArmorInfo _armorInfo;
    public ArmorInfo ArmorInfo { get { return _armorInfo; } }
    public override EquipmentType Type { get { return EquipmentType.Armor; } }
}
