using System;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthUI : MonoBehaviour
{
    [SerializeField] Image _fillHP;

    private void OnEnable() {
        Entity.OnDamageTaken += Entity_OnDamageTaken;
    }

    private void OnDisable() {
        Entity.OnDamageTaken -= Entity_OnDamageTaken;
    }

    private void Entity_OnDamageTaken(int currentHealth, int baseHealth) {
        Debug.Log("Current hp: " + currentHealth + " Base HP: " + baseHealth);
        _fillHP.fillAmount = Mathf.Clamp(currentHealth / (float)baseHealth, 0, 1);   
    }
}
