using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlsMenuUI : MonoBehaviour
{
    public static Action OnLoadoutSelected;
    public static Action OnFightSelected;

    private void OnEnable() {
        // reset button status to unselected for all buttons   
    }

    public void OnLoadoutClick() {
        OnLoadoutSelected?.Invoke();
    }

    public void OnFightClick() {
        OnFightSelected?.Invoke();
    }
}
