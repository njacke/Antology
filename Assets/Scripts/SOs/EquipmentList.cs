using UnityEngine;

[CreateAssetMenu(menuName = "New Equipment List")]

public class EquipmentList : ScriptableObject
{
    public GameObject[] abilityHeadPrefabs;
    public GameObject[] abilityTopPrefabs;
    public GameObject[] abilityMidPrefabs;
    public GameObject[] abilityBotPrefabs;

    public GameObject[] armorHeadPrefabs;
    public GameObject[] armorTopPrefabs;
    public GameObject[] armorMidPrefabs;
    public GameObject[] armorBotPrefabs;
}

