using UnityEngine;

[CreateAssetMenu(menuName = "New Armor")]

public class ArmorInfo : ScriptableObject
{
    public int ID;
    public string Name;
    public EquipmentManager.EquipmentSlot Slot;
    public int Health;
}
