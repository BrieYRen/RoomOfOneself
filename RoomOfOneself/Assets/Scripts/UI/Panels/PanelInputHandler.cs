using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// this script is used to check if a panel is toggled by its hotkey, and which hotkeys are allowed at the moment
/// </summary>
public class PanelInputHandler : MonoBehaviour
{
    InputType currentInputType = InputType.AllowAll;

    Dictionary<KeyCode, List<UIPanel>> keyPanelPairs;

    KeyCode allowedKeycode = KeyCode.None;
    UIPanel allowedUIPanel = null;
    KeyTriggerType allowedInputTriggerType = KeyTriggerType.Click;


    private void Update()
    {
        InputCheck();
    }

    /// <summary>
    /// check if a panel is toggled by its hotkey
    /// </summary>
    void InputCheck()
    {
        switch (currentInputType)
        {
            case InputType.LockAll:
                break;


            case InputType.AllowOne:
                if (allowedKeycode == KeyCode.None || allowedUIPanel == null)
                    return;

                if ((allowedInputTriggerType == KeyTriggerType.Hold && Input.GetKeyUp(allowedKeycode)) || (allowedInputTriggerType == KeyTriggerType.Click && Input.GetKeyDown(allowedKeycode)))
                {
                    allowedUIPanel.Close();
                }
                              
                break;

            case InputType.AllowAll:
                if (keyPanelPairs == null)
                    return;

                foreach (KeyValuePair<KeyCode, List<UIPanel>> kvp in keyPanelPairs)
                {
                    if (Input.GetKey(kvp.Key))
                    {
                        for (int i = 0; i < kvp.Value.Count; i++)
                        {
                            if (kvp.Value[i].TriggerType == KeyTriggerType.Hold)
                                kvp.Value[i].Show();
                        }
                    }
                    if (Input.GetKeyUp(kvp.Key))
                    {
                        for (int i = 0; i < kvp.Value.Count; i++)
                        {
                            if (kvp.Value[i].TriggerType == KeyTriggerType.Hold)
                                kvp.Value[i].Close();
                        }
                    }
                    if (Input.GetKeyDown(kvp.Key))
                    {
                        for (int i = 0; i < kvp.Value.Count; i++)
                        {
                            if (kvp.Value[i].TriggerType == KeyTriggerType.Click)
                                kvp.Value[i].Toggle();
                        }
                    }
                }

                break;
        }
    }

    #region Register n Unregister Input

    /// <summary>
    /// public method to register a panel with its hotkey
    /// </summary>
    /// <param name="keyCode"></param>
    /// <param name="uIPanel"></param>
    public void RegisterInput(KeyCode keyCode, UIPanel uIPanel)
    {
        if (keyPanelPairs == null)
            keyPanelPairs = new Dictionary<KeyCode, List<UIPanel>>();

        if (keyCode == KeyCode.None)
            return;

        List<UIPanel> tempPanels;
        if (keyPanelPairs.TryGetValue(keyCode, out tempPanels))
        {
            if (tempPanels != null)
            {
                tempPanels.Add(uIPanel);
                keyPanelPairs[keyCode] = tempPanels;
            }
        }
        else
        {
            keyPanelPairs.Add(keyCode, new List<UIPanel>() { uIPanel });
        }

    }

    /// <summary>
    /// public method to unregister a panel with its hotkey
    /// </summary>
    /// <param name="keyCode"></param>
    /// <param name="uIPanel"></param>
    public void UnregisterInput(KeyCode keyCode, UIPanel uIPanel)
    {
        if (keyPanelPairs == null)
            return;

        List<UIPanel> tempPanels;
        if (keyPanelPairs.TryGetValue(keyCode, out tempPanels))
        {
            if (tempPanels.Contains(uIPanel))
                keyPanelPairs[keyCode].Remove(uIPanel);
        }

    }

    #endregion


    #region InputType Transitions

    /// <summary>
    /// public method to transition input type from AllowAll to AllowOne mode
    /// </summary>
    /// <param name="keyCode"></param>
    /// <param name="uIPanel"></param>
    /// <param name="inputTriggerType"></param>
    public void EnterAllowOneMode(KeyCode keyCode, UIPanel uIPanel, KeyTriggerType inputTriggerType)
    {
        if (currentInputType != InputType.AllowAll)
            return;

        allowedKeycode = keyCode;
        allowedUIPanel = uIPanel;
        allowedInputTriggerType = inputTriggerType;

        currentInputType = InputType.AllowOne;
    }

    /// <summary>
    /// public method to transition input type from AllowOne back to AllowAll
    /// </summary>
    public void ExitAllowOneMode()
    {
        currentInputType = InputType.AllowAll;

        allowedKeycode = KeyCode.None;
        allowedUIPanel = null;
    }

    /// <summary>
    /// public method to transition input type from AllowAll to LockAll
    /// </summary>
    public void EnterLockAllMode()
    {
        if (currentInputType != InputType.AllowAll)
            return;

        currentInputType = InputType.LockAll;
    }

    /// <summary>
    /// public method to transition input type from LockAll back to AllowAll
    /// </summary>
    public void ExitLockAllMode()
    {
        if (currentInputType != InputType.LockAll)
            return;

        currentInputType = InputType.AllowAll;
    }

    #endregion

}

public enum KeyTriggerType
{
    /// <summary>
    /// click the certain keycode to trigger
    /// </summary>
    Click,

    /// <summary>
    /// press and hold the keycode to trigger, and release to exit
    /// </summary>
    Hold,
}

public enum InputType
{
    /// <summary>
    /// all hotkey is allowed, the default input type
    /// </summary>
    AllowAll,

    /// <summary>
    /// only allow one hotkey, all others are locked, can only changed from the AllowAll mode
    /// </summary>
    AllowOne,

    /// <summary>
    /// all hotkeys are locked, can only changed from the AllowAll mode
    /// </summary>
    LockAll,
}
