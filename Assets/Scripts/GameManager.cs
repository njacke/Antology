using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public static Action OnGameStarted;

    [SerializeField] private float _gameOverDelay = 3f;
    [SerializeField] private float _winScreenDelay = 3f;
    [SerializeField] private float _resultDisplayDelay = 2f;
    [SerializeField] private GameObject _loadoutUI;
    [SerializeField] private GameObject _controlsUI;
    [SerializeField] private GameObject _winScreenUI;
    [SerializeField] private ResultDisplayUI _resultDisplayUI;

    public bool IsGameActive { get; private set; } = true;
    public float CurrentFightTime { get; private set; } = 0f;

    private void Start() {
        Time.timeScale = 0f;
    }

    private void Update() {
        if (IsGameActive) {
            CurrentFightTime += Time.deltaTime;
        }
    }

    private void OnEnable() {
        PlayerHealth.OnPlayerDeath += PlayerHealth_OnPlayerDeath;
        ControlsMenuUI.OnLoadoutSelected += ControlsMenuUI_OnLoadoutSelected;
        ControlsMenuUI.OnFightSelected += ControlsMenuUI_OnFightSelected;
        LoadoutMenuUI.OnControlsSelected += LoadoutMenuUI_OnControlsSelected;
        LoadoutMenuUI.OnFightSelected += LoadoutMenuUI_OnFightSelected;
        WinScreenMenuUI.OnRestartSelected += WinScreenMenuUI_OnRestartSelected;
        Entity.OnDeath += Entity_OnDeath;
    }

    private void OnDisable() {
        PlayerHealth.OnPlayerDeath -= PlayerHealth_OnPlayerDeath;
        ControlsMenuUI.OnLoadoutSelected -= ControlsMenuUI_OnLoadoutSelected;
        ControlsMenuUI.OnFightSelected -= ControlsMenuUI_OnFightSelected;
        LoadoutMenuUI.OnControlsSelected -= LoadoutMenuUI_OnControlsSelected;
        LoadoutMenuUI.OnFightSelected -= LoadoutMenuUI_OnFightSelected;
        WinScreenMenuUI.OnRestartSelected -= WinScreenMenuUI_OnRestartSelected;
        Entity.OnDeath -= Entity_OnDeath;
    }

    private void Entity_OnDeath() {
        StartCoroutine(WinRoutine());
    }

    private void WinScreenMenuUI_OnRestartSelected() {
        SceneManager.LoadScene(0);
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
        _resultDisplayUI.HideResult();
        Time.timeScale = 1f;
        IsGameActive = true;
        CurrentFightTime = 0f;
        OnGameStarted?.Invoke();
    }

    private IEnumerator GameOverRoutine() {
        IsGameActive = false;
        yield return new WaitForSecondsRealtime(_resultDisplayDelay);
        _resultDisplayUI.DisplayResult(false);
        yield return new WaitForSecondsRealtime(_gameOverDelay);
        Time.timeScale = 0f;
        _loadoutUI.SetActive(true);
    }

    private IEnumerator WinRoutine() {
        yield return new WaitForSecondsRealtime(_resultDisplayDelay);
        _resultDisplayUI.DisplayResult(true);
        yield return new WaitForSecondsRealtime(_winScreenDelay);
        IsGameActive = false;
        Time.timeScale = 0f;
        _winScreenUI.SetActive(true);
    }
}
