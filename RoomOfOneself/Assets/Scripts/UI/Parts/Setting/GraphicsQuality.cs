using UnityEngine;
using TMPro;

/// <summary>
/// this script should be attach to a TextMeshPro Dropdown
/// when player change the value of the dropdown, they'll change the graphic quality of the game
/// the method will be added automatically when start, no need to configurate it in OnValueChange area
/// </summary>
[RequireComponent(typeof(TMP_Dropdown))]
public class GraphicsQuality : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The default graphics quality of the game")]
    int defaultIndex = 5;

    TMP_Dropdown dropdown;
    int currentQualityIndex;

    SaveKeyRegister saveKeyRegister;
    readonly string saveFileKey = "qualityIndex";


    private void Start()
    {
        dropdown = GetComponent<TMP_Dropdown>();
        saveKeyRegister = GameManager.instance.saveKeyRegister;

        if (PlayerPrefs.HasKey(saveFileKey))
        {
            currentQualityIndex = PlayerPrefs.GetInt(saveFileKey);
        }
        else
        {
            currentQualityIndex = defaultIndex;
            saveKeyRegister.RegisterKey(saveFileKey, true);
        }
        
        dropdown.value = currentQualityIndex;
        SetQuality(currentQualityIndex);
        dropdown.onValueChanged.AddListener(SetQuality);
    }

    public void SetQuality(int qualityIndex)
    {
        currentQualityIndex = qualityIndex;
        QualitySettings.SetQualityLevel(currentQualityIndex);
        PlayerPrefs.SetInt(saveFileKey, currentQualityIndex);
    }
}
