using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// this script should be attach to a UI TextMeshPro Dropdown
/// when player change the value of the dropdown, they'll change the resolution of the game
/// the method will be added automatically when start, no need to configurate it in OnValueChange area
/// </summary>
[RequireComponent(typeof(TMP_Dropdown))]
public class ScreenResolution : MonoBehaviour
{
    TMP_Dropdown dropdown;

    Resolution[] resolutions;
    int currentResolutionIndex = 0;

    readonly string saveFileKey = "resolutionIndex";
    SaveKeyRegister saveKeyRegister;

    private void Start()
    {
        dropdown = GetComponent<TMP_Dropdown>();
        resolutions = Screen.resolutions;

        saveKeyRegister = GameManager.instance.saveKeyRegister;

        if (PlayerPrefs.HasKey(saveFileKey))
            currentResolutionIndex = PlayerPrefs.GetInt(saveFileKey);

        List<string> dropdownOptions = new List<string>();
        for (int i = 0; i < resolutions.Length; i++)
        {
            string temptString = resolutions[i].width + "x" + resolutions[i].height + ", " + resolutions[i].refreshRate + "hz";
            dropdownOptions.Add(temptString);

            if (!PlayerPrefs.HasKey(saveFileKey))
            {
                if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height && resolutions[i].refreshRate == Screen.currentResolution.refreshRate)
                    currentResolutionIndex = i;

                saveKeyRegister.RegisterKey(saveFileKey, true);
            }
        }

        dropdown.ClearOptions();
        dropdown.AddOptions(dropdownOptions);

        dropdown.value = currentResolutionIndex;
        dropdown.RefreshShownValue();

        Debug.Log(dropdown.value);

        dropdown.onValueChanged.AddListener(SetResolution);
    }


    public void SetResolution (int newIndex)
    {
        currentResolutionIndex = newIndex;
        Resolution indexResolution = resolutions[currentResolutionIndex];
        Screen.SetResolution(indexResolution.width, indexResolution.height, Screen.fullScreen);
        PlayerPrefs.SetInt(saveFileKey, currentResolutionIndex);
    }
}
