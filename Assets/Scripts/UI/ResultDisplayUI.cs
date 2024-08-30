using TMPro;
using UnityEngine;

public class ResultDisplayUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _victoryResult;
    [SerializeField] TextMeshProUGUI _defeatResult;

    //TODO: make event based when time?
    public void DisplayResult(bool isWin) {
        if (isWin) {
            _victoryResult.enabled = true;
            _victoryResult.enabled = true;

        } else {
            _victoryResult.enabled = false;
            _defeatResult.enabled = true;
        }
    }

    public void HideResult() {
            _victoryResult.enabled = false;
            _defeatResult.enabled = false;
    }
}
