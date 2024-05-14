using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : Ability, IAbilityAction
{   
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private Transform _spawnPointL;
    [SerializeField] private Transform _spawnPointR;
    private Transform _player;
    
    private void Start() {
        _player = FindObjectOfType<PlayerController>().transform;                
    }

    public void AbilityAction() {
        SpawnProjectiles();
    }

    private void SpawnProjectiles() {
        Debug.Log("Spawn projectiles called.");
        Instantiate(_projectilePrefab, _spawnPointL.position, _player.rotation);
        Instantiate(_projectilePrefab, _spawnPointR.position, _player.rotation);
    }

}
