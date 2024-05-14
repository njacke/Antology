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
    [SerializeField] private GameObject _testItem;

    private Transform _player;

    private Dictionary<EquipmentSlot, Transform> _abilitySlotDict;

    public Transform HeadAbilitySlot { get { return _headAbilitySlot; } }
    public Transform TopAbilitySlot { get { return _topAbilitySlot; } }
    public Transform MidAbilitySlot { get { return _midAbilitySlot; } }
    public Transform BotAbilitySlot { get { return _botAbilitySlot; } }

    public static event Action OnEquipmentChange;

    public enum EquipmentSlot {
        Head,
        Top,
        Mid,
        Bot
    }

    public enum AbilitySpeed {
        Slow,
        Normal,
        Fast
    }

    public enum EquipmentType {
        Ability,
        Armor
    }

    private void Awake() {
        _abilitySlotDict = new Dictionary<EquipmentSlot, Transform>() {
            {EquipmentSlot.Head, _headAbilitySlot},
            {EquipmentSlot.Top, _topAbilitySlot},
            {EquipmentSlot.Mid, _midAbilitySlot},
            {EquipmentSlot.Bot, _botAbilitySlot},    
        };
    }

    private void Start() {
        _player = GetComponent<PlayerController>().transform;
    }

    private IEnumerator EquipItemRoutine(GameObject equipment) {
        var newAbilityEquipment = equipment.GetComponent<Ability>();
    
        if (newAbilityEquipment != null) {
            UnequipItem(EquipmentType.Ability, newAbilityEquipment.AbilityInfo.Slot);

            var newEquipmentObject = Instantiate(equipment, _player.transform.position, _player.transform.rotation);
            newEquipmentObject.transform.SetParent(_abilitySlotDict[newAbilityEquipment.AbilityInfo.Slot]);                   
        }

        yield return new WaitForSeconds(.05f); // make sure hierarchy is resolved before other script calls
        OnEquipmentChange?.Invoke();
    }
    private void UnequipItem(EquipmentType type, EquipmentSlot slot) {
        if (type == EquipmentType.Ability && _abilitySlotDict[slot].GetComponentInChildren<Ability>() != null) {
            var previousAbilityEquipment = _abilitySlotDict[slot].GetComponentInChildren<Ability>().gameObject;
            Destroy(previousAbilityEquipment);
        }
        else if (type == EquipmentType.Armor) {}
            // destroy armor object
    }
}

