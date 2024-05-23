using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int _baseHealth = 1;

    private int _currentArmor = 0;

    private EquipmentManager _equipmentManager;

    private void Awake() {
        _equipmentManager = GetComponent<EquipmentManager>();
    }

    private void Start()
    {
        //StartCoroutine(TakeDamageTestRoutine());
    }

    private void OnEnable() {
        EquipmentManager.OnEquipmentChange += UpdateCurrentArmor;
    }

    private void OnDisable() {
        EquipmentManager.OnEquipmentChange -= UpdateCurrentArmor;
    }

    private IEnumerator TakeDamageTestRoutine() {
        for ( int i = 0; i < 4; i++) {
            yield return new WaitForSeconds(3f);
            Debug.Log("Taking damage.");
            TakeDamage(1);
        }
    }

    private void UpdateCurrentArmor() {
        int currentArmor = 0;

        foreach (var armor in _equipmentManager.ArmorDict) {
            if (armor.Value != null) {
               currentArmor += armor.Value.ArmorInfo.Health;
            }
        }

        _currentArmor = currentArmor;
        //Debug.Log("Current armor: " + _currentArmor);
    }

    public void TakeDamage(int damageAmount) {
        for (int i = 0; i < damageAmount; i++) {
            if (_currentArmor > 0) {                
                foreach (var armor in _equipmentManager.ArmorDict) {
                    if (armor.Value != null) {
                        Debug.Log("Armor found.");
                        _equipmentManager.UnequipItem(armor.Key, Equipment.EquipmentType.Armor); // dict changes
                        break; // 1 "damage" per loop to 1st armor found
                    }
                }
            }
            else {
                _baseHealth--;
            }
        }

        if (_baseHealth <= 0) {
            // handle player death
            Debug.Log("Player Died");
        }
    }
}
