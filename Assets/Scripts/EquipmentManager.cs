using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    [SerializeField] private Transform _headAbilitySlot;
    [SerializeField] private Transform _topAbilitySlot;
    [SerializeField] private Transform _midAbilitySlot;
    [SerializeField] private Transform _botAbilitySlot;

    [SerializeField] private Transform _headArmorSlot;
    [SerializeField] private Transform _topArmorSlot;
    [SerializeField] private Transform _midArmorSlot;
    [SerializeField] private Transform _botArmorSlot;

    [SerializeField] private EquipmentList _equipmentList;

    [SerializeField] private GameObject _testItem;

    private Transform _player;

    private Dictionary<EquipmentSlot, Transform> _abilitySlotDict;
    private Dictionary<EquipmentSlot, Ability> _abilityDict;
    private Dictionary<EquipmentSlot, Transform> _armorSlotDict;
    private Dictionary<EquipmentSlot, Armor> _armorDict;

    public Ability HeadAbility { get { return _abilityDict[EquipmentSlot.Head]; } }
    public Ability TopAbility { get { return _abilityDict[EquipmentSlot.Top]; } }
    public Ability MidAbility { get { return _abilityDict[EquipmentSlot.Mid]; } }
    public Ability BotAbility { get { return _abilityDict[EquipmentSlot.Bot]; } }

    public Armor HeadArmor { get { return _armorDict[EquipmentSlot.Head]; } }
    public Armor TopArmor { get { return _armorDict[EquipmentSlot.Top]; } }
    public Armor MidArmor { get { return _armorDict[EquipmentSlot.Mid]; } }
    public Armor BotArmor { get { return _armorDict[EquipmentSlot.Bot]; } }

    public static event Action OnEquipmentChange;

    public enum EquipmentSlot {
        Head,
        Top,
        Mid,
        Bot
    }

    private void Awake()
    {
        _player = GetComponent<PlayerController>().transform;

        InitialiseEquipmentTest();
        DictsSetup();
    }


    private void Start() {
        //StartCoroutine(EquipItemTestRoutine());
    }

    private IEnumerator EquipItemTestRoutine() {
        for ( int i = 0; i < 5; i++) {
            yield return new WaitForSeconds(3f);
            EquipItem (_testItem);
            Debug.Log("Equiped " + i);
        }
    }

    private void InitialiseEquipmentTest() {
        Instantiate(_equipmentList.abilityHeadPrefabs[0], _player.transform.position, _player.transform.rotation).transform.SetParent(_headAbilitySlot);
        Instantiate(_equipmentList.abilityTopPrefabs[0], _player.transform.position, _player.transform.rotation).transform.SetParent(_topAbilitySlot);
        Instantiate(_equipmentList.abilityMidPrefabs[0], _player.transform.position, _player.transform.rotation).transform.SetParent(_midAbilitySlot);
        Instantiate(_equipmentList.abilityBotPrefabs[0], _player.transform.position, _player.transform.rotation).transform.SetParent(_botAbilitySlot);

        Instantiate(_equipmentList.armorHeadPrefabs[0], _player.transform.position, _player.transform.rotation).transform.SetParent(_headArmorSlot);
        Instantiate(_equipmentList.armorTopPrefabs[0], _player.transform.position, _player.transform.rotation).transform.SetParent(_topArmorSlot);
        Instantiate(_equipmentList.armorMidPrefabs[0], _player.transform.position, _player.transform.rotation).transform.SetParent(_midArmorSlot);
        Instantiate(_equipmentList.armorBotPrefabs[0], _player.transform.position, _player.transform.rotation).transform.SetParent(_botArmorSlot);
    }
    
    private void DictsSetup()
    {
        _abilitySlotDict = new Dictionary<EquipmentSlot, Transform>() {
            { EquipmentSlot.Head, _headAbilitySlot },
            { EquipmentSlot.Top, _topAbilitySlot },
            { EquipmentSlot.Mid, _midAbilitySlot },
            { EquipmentSlot.Bot, _botAbilitySlot }
        };

        _abilityDict = new Dictionary<EquipmentSlot, Ability>() {
            { EquipmentSlot.Head, _headAbilitySlot.GetComponentInChildren<Ability>() },
            { EquipmentSlot.Top, _topAbilitySlot.GetComponentInChildren<Ability>() },
            { EquipmentSlot.Mid, _midAbilitySlot.GetComponentInChildren<Ability>() },
            { EquipmentSlot.Bot, _botAbilitySlot.GetComponentInChildren<Ability>() }
        };

        _armorSlotDict = new Dictionary<EquipmentSlot, Transform>() {
            { EquipmentSlot.Head, _headArmorSlot },
            { EquipmentSlot.Top, _topArmorSlot },
            { EquipmentSlot.Mid, _midArmorSlot },
            { EquipmentSlot.Bot, _botArmorSlot }
        };

        _armorDict = new Dictionary<EquipmentSlot, Armor>() {
            { EquipmentSlot.Head, _headArmorSlot.GetComponentInChildren<Armor>() },
            { EquipmentSlot.Top, _topArmorSlot.GetComponentInChildren<Armor>() },
            { EquipmentSlot.Mid, _midArmorSlot.GetComponentInChildren<Armor>() },
            { EquipmentSlot.Bot, _botArmorSlot.GetComponentInChildren<Armor>() }
        };
    }

    private void EquipItem(GameObject equipmentPrefab) {  
        
        if (!equipmentPrefab.TryGetComponent<Equipment>(out var equipment)) {
            return;
        }

        else if (equipment.Type == Equipment.EquipmentType.Ability) {
            var newAbility = equipment.GetComponent<Ability>();
            UnequipItem(newAbility.AbilityInfo.Slot, Equipment.EquipmentType.Ability);

            _abilityDict[newAbility.AbilityInfo.Slot] = Instantiate(equipment, _player.transform.position, _player.transform.rotation).GetComponent<Ability>();
            _abilityDict[newAbility.AbilityInfo.Slot].transform.SetParent(_abilitySlotDict[newAbility.AbilityInfo.Slot]);
        }        
        else {
            var newArmor = equipment.GetComponent<Armor>();
            UnequipItem(newArmor.ArmorInfo.Slot, Equipment.EquipmentType.Armor);

            _armorDict[newArmor.ArmorInfo.Slot] = Instantiate(equipment, _player.transform.position, _player.transform.rotation).GetComponent<Armor>();
            _armorDict[newArmor.ArmorInfo.Slot].transform.SetParent(_armorSlotDict[newArmor.ArmorInfo.Slot]);
        }

        OnEquipmentChange?.Invoke();
    }

    public void UnequipItem( EquipmentSlot slot, Equipment.EquipmentType type) {
        if (_abilityDict[slot] == null && _armorDict[slot] == null) {
            Debug.Log("Nothing equipped on that slot");
            return;
        }
        else if (type == Equipment.EquipmentType.Ability) {
            Debug.Log ("Unequipping ability");
            Destroy(_abilityDict[slot].gameObject);
            _abilityDict[slot] = null;
        }
        else {
            Debug.Log ("Unequipping armor");
            Destroy(_armorDict[slot].gameObject);
            _armorDict[slot] = null;
        }

        OnEquipmentChange?.Invoke();
    }

    public Dictionary<EquipmentSlot, Armor> GetArmorDict() {
        return _armorDict;
    }
}

