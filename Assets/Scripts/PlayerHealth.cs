using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int _baseHealth = 1;
    [SerializeField] private float _immuneDur = .5f;
    private float _immuneDurRemaining = 0f;
    private int _currentArmor = 0;
    private Shield _currentShield;

    private EquipmentManager _equipmentManager;

    private void Awake() {
        _equipmentManager = GetComponent<EquipmentManager>();
    }

    private void Update() {
        _immuneDurRemaining -= Time.deltaTime;
    }

    private void OnEnable() {
        EquipmentManager.OnEquipmentChange += EquipmentManager_OnEquipmentChange;
        DamageDealer.OnDamageDealtToPlayer += DamageDealer_OnDamageDealtToPlayer;
        Shield.OnShieldActivated += Shield_OnShieldActivated;
        Shield.OnShieldDeactivated += Shield_OnShieldDeactivated;
    }

    private void OnDisable() {
        EquipmentManager.OnEquipmentChange -= EquipmentManager_OnEquipmentChange;
        DamageDealer.OnDamageDealtToPlayer -= DamageDealer_OnDamageDealtToPlayer;
        Shield.OnShieldActivated -= Shield_OnShieldActivated;
        Shield.OnShieldDeactivated -= Shield_OnShieldDeactivated;
    }

    private void EquipmentManager_OnEquipmentChange() {
        UpdateCurrentArmor();
    }

    private void DamageDealer_OnDamageDealtToPlayer(int damageAmount) {
        TakeDamage(damageAmount);
    }

    private void Shield_OnShieldActivated(Shield sender) {
        _currentShield = sender;
    }

    private void Shield_OnShieldDeactivated(Shield sender) {
        Destroy(sender.gameObject);
        _currentShield = null;
    }

    private void UpdateCurrentArmor() {
        int currentArmor = 0;

        foreach (var armor in _equipmentManager.ArmorDict) {
            if (armor.Value != null) {
               currentArmor += armor.Value.ArmorInfo.Health;
            }
        }

        _currentArmor = currentArmor;
    }

    public void TakeDamage(int damageAmount) {
        // return if player is immune
        if (_immuneDurRemaining >= 0f) {
            return;
        }

        // take shield damage
        if (_currentShield != null) {
            Destroy(_currentShield.gameObject);
            _currentShield = null;
            _immuneDurRemaining = _immuneDur;
            Debug.Log("Damage taken, shield destroyed.");            
            return;
        }
        
        // take armor damage
        for (int i = 0; i < damageAmount; i++) {
            if (_currentArmor > 0) {                
                foreach (var armor in _equipmentManager.ArmorDict) {
                    if (armor.Value != null) {
                        Debug.Log("Armor found.");
                        _equipmentManager.UnequipItem(armor.Key, Equipment.EquipmentType.Armor); // dict changes
                        break; // 1 "damage" per loop to 1st armor found
                    }
                }
            } else {
                _baseHealth--;
            }
        }
        
        _immuneDurRemaining = _immuneDur;

        if (_baseHealth <= 0) {
            // handle player death
            Debug.Log("Player Died");
        }
    }
}
