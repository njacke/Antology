using UnityEngine;

public class Bubble : Ability, IAbilityAction
{
    [SerializeField] private GameObject _shieldPrefab;

    private Transform _player;

    private void Start() {
        _player = FindObjectOfType<PlayerController>().transform;
    }

    public void AbilityAction() {
        SpawnShield();
    }

    private void SpawnShield() {
        Debug.Log("Spawning shield");
        var spawnedShield = Instantiate(_shieldPrefab, this.transform.position, Quaternion.identity);
        spawnedShield.transform.SetParent(_player);
        spawnedShield.GetComponent<Shield>().ShieldDuration = AbilityInfo.Duration;
    }
}
