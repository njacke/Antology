using System;
using TMPro;
using UnityEngine;

public class WinScreenMenuUI : MonoBehaviour
{
    public static Action OnRestartSelected;

    [SerializeField] private TextMeshProUGUI _winText;
    [SerializeField] private int _decimalPlaces = 2;

    private void OnEnable() {
        UpdateWinText();        
    }

    private void UpdateWinText() {
        string killTimeString = ConvertFloatToTimeString(GameManager.Instance.CurrentFightTime, _decimalPlaces);

        //TODO: make dynamic when more bosses
        _winText.text = "Anteater has been defeated with kill time of " + killTimeString + ".";
    }

    private string ConvertFloatToTimeString(float totalSeconds, int decimalPlaces = 0) {
        TimeSpan timeSpan = TimeSpan.FromSeconds(totalSeconds);
        int minutes = timeSpan.Minutes;
        float seconds = timeSpan.Seconds + timeSpan.Milliseconds / 1000f;

        string timeString = string.Format("{0}:{1:F" + decimalPlaces + "}", minutes, seconds);

        return timeString;
    }

    public void OnRestartClick() {
        OnRestartSelected?.Invoke();
    }
}
