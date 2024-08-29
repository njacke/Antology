using System;
using System.Collections;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public static Action OnGameStarted;

    [SerializeField] private float _gameOverDelay = 3f;
    [SerializeField] private GameObject _loadoutUI;
    [SerializeField] private GameObject _controlsUI;
    public bool IsGameActive { get; private set; } = true;

    private void Start() {
        Time.timeScale = 0f;
    }

    private void OnEnable() {
        PlayerHealth.OnPlayerDeath += PlayerHealth_OnPlayerDeath;
        ControlsMenuUI.OnLoadoutSelected += ControlsMenuUI_OnLoadoutSelected;
        ControlsMenuUI.OnFightSelected += ControlsMenuUI_OnFightSelected;
        LoadoutMenuUI.OnControlsSelected += LoadoutMenuUI_OnControlsSelected;
        LoadoutMenuUI.OnFightSelected += LoadoutMenuUI_OnFightSelected;
    }

    private void OnDisable() {
        PlayerHealth.OnPlayerDeath -= PlayerHealth_OnPlayerDeath;
        ControlsMenuUI.OnLoadoutSelected -= ControlsMenuUI_OnLoadoutSelected;
        ControlsMenuUI.OnFightSelected -= ControlsMenuUI_OnFightSelected;
        LoadoutMenuUI.OnControlsSelected -= LoadoutMenuUI_OnControlsSelected;
        LoadoutMenuUI.OnFightSelected -= LoadoutMenuUI_OnFightSelected;
    }

    private void LoadoutMenuUI_OnFightSelected() {
        _loadoutUI.SetActive(false);
        GameReset();
    }

    private void LoadoutMenuUI_OnControlsSelected() {
        _loadoutUI.SetActive(false);
        _controlsUI.SetActive(true);
    }

    private void ControlsMenuUI_OnFightSelected() {
        _controlsUI.SetActive(false);
        GameReset();
    }


    private void ControlsMenuUI_OnLoadoutSelected() {
        _controlsUI.SetActive(false);
        _loadoutUI.SetActive(true);
    }

    private void PlayerHealth_OnPlayerDeath() {
        StartCoroutine(GameOverRoutine());
    }

    private void GameReset() {
        Time.timeScale = 1f;
        IsGameActive = true;
        OnGameStarted?.Invoke();
    }

    private IEnumerator GameOverRoutine() {
        IsGameActive = false;
        yield return new WaitForSeconds(_gameOverDelay);
        Time.timeScale = 0f;
        _loadoutUI.SetActive(true);

        // display game over UI/trigger event
        Debug.Log("Displaying Game Over.");
    }
}
