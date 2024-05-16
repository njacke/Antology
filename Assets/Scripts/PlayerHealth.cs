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
        UpdateCurrentArmor();
        StartCoroutine(TakeDamageTestRoutine());
    }

    private IEnumerator TakeDamageTestRoutine() {
        for ( int i = 0; i < 4; i++) {
            yield return new WaitForSeconds(3f);
            Debug.Log("Taking damage.");
            TakeDamage(1);
        }
    }

    private void UpdateCurrentArmor() {
        var currentArmorDict = _equipmentManager.GetArmorDict();
        int currentArmor = 0;

        foreach (var armor in currentArmorDict) {
            if (armor.Value != null) {
               currentArmor += armor.Value.ArmorInfo.Health;
            }
        }

        _currentArmor = currentArmor;
    }

    private void TakeDamage(int damageAmount) {
        for (int i = 0; i < damageAmount; i++) {
            if (_currentArmor > 0) {
                var currentArmorDict = _equipmentManager.GetArmorDict();
                foreach (var armor in currentArmorDict) {
                    if (armor.Value != null) {
                        armor.Value.ArmorInfo.Health--;
                        if (armor.Value.ArmorInfo.Health <= 0) {
                            _equipmentManager.UnequipItem(armor.Key, Equipment.EquipmentType.Armor); // dict changes
                        }
                        break; // 1 "damage" per loop to 1st armor found
                    }
                }
            }
            else {
                _baseHealth--;
            }

            UpdateCurrentArmor();
        }

        if (_baseHealth <= 0) {
            // handle player death
            Debug.Log("Player Died");
        }
    }
}
