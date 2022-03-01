using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// this script should be attach to a UI toggle
/// when player change the value of the toggle, they'll change the game mode between fullscreen and windowed
/// the method will be added automatically when start, no need to configurate it in OnValueChange area
/// </summary>
[RequireComponent(typeof(Toggle))]
public class ScreenMode : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Check true if the game is fullscreen by defualt")]
    bool defaultValue = true;

    readonly string saveFileKey = "scrMode";

    bool currentValue;
    Toggle toggle;

    SaveKeyRegister saveKeyRegister;

    private void Start()
    {
        toggle = GetComponent<Toggle>();
        saveKeyRegister = GameManager.instance.saveKeyRegister;

        if (PlayerPrefs.HasKey(saveFileKey))
        {
            if (PlayerPrefs.GetInt(saveFileKey) == 0)
                currentValue = false;
            else
                currentValue = true;
        }
        else
        {
            currentValue = defaultValue;
            saveKeyRegister.RegisterKey(saveFileKey, true);
        }
        
        toggle.isOn = currentValue;
        SetScreenMode(currentValue);
        toggle.onValueChanged.AddListener(SetScreenMode);
    }

    public void SetScreenMode(bool ifFullscreen)
    {
        currentValue = ifFullscreen;
        Screen.fullScreen = currentValue;

        if (currentValue)
            PlayerPrefs.SetInt(saveFileKey, 1);
        else
            PlayerPrefs.SetInt(saveFileKey, 0);

    }
}
