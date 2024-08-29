using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadoutMenuUI : MonoBehaviour
{
    public static Action OnControlsSelected;
    public static Action OnFightSelected;

    private void OnEnable() {
        // reset button status to unselected for all buttons   
    }

    public void OnControlsClick() {
        OnControlsSelected?.Invoke();
    }

    public void OnFightClick() {
        OnFightSelected?.Invoke();
    }
}
