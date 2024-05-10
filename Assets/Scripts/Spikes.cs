using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{   
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform spawnPointL;
    [SerializeField] private Transform spawnPointR;
    private Transform player;
    private void Start() {
        player = FindObjectOfType<PlayerController>().transform;                
    }

    private void OnEnable() {
        PlayerCombat.OnSecondaryAttackAction += AttackAction;
        PlayerCombat.OnSecondaryAttackEnd += AttackEnd;
    }

    private void OnDisable() {
        PlayerCombat.OnSecondaryAttackAction -= AttackAction;
        PlayerCombat.OnSecondaryAttackEnd -= AttackEnd;
    }

    private void AttackAction() {
        SpawnProjectiles();
    }

    private void AttackEnd() {
        // do smth if needed
    }

    private void SpawnProjectiles() {
        Instantiate(projectilePrefab, spawnPointL.position, player.rotation);
        Instantiate(projectilePrefab, spawnPointR.position, player.rotation);
    }
}
