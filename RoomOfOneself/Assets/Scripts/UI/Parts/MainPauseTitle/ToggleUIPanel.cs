using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// this script is used to attach to a UI button
/// when player click the button, a panel will be opened or closed
/// remember to drag and drop it in the OnClick area and choose between the two public methods to desice if the target panel should be toggled immediate or wait for some seconds
/// </summary>
[RequireComponent(typeof(Button))]
public class ToggleUIPanel : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The target panel to be toggled by the UI button")]
    UIPanel uiPanel;

    /// <summary>
    /// toggle the target panel when player click it
    /// </summary>
    public void ToggleNow()
    {
        uiPanel.Toggle();
    }

    /// <summary>
    /// toggle the target panel in several seconds after player click the button 
    /// </summary>
    /// <param name="sec"></param>
    public void ToggleInSec(float sec)
    {
        Invoke("ToggleNow", sec);
    }

}
