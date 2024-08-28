using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private float _gameOverDelay = 3f;
    public bool IsGameActive { get; private set; } = true;

    private void OnEnable() {
        PlayerHealth.OnPlayerDeath += PlaneHealth_OnPlayerDeath;
    }

    private void OnDisable() {
        PlayerHealth.OnPlayerDeath -= PlaneHealth_OnPlayerDeath;
    }

    private void PlaneHealth_OnPlayerDeath() {
        StartCoroutine(GameOverRoutine());
    }

    private IEnumerator GameOverRoutine() {
        IsGameActive = false;
        yield return new WaitForSeconds(_gameOverDelay);
        // display game over UI/trigger event
        Debug.Log("Displaying Game Over.");
    }
}
