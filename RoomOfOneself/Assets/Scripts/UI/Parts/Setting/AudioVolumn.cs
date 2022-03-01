using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

/// <summary>
/// this script should be attach to a UI slider
/// when player change the value of the slider, they'll change an audio mixer's volumn
/// the method will be added automatically when start, no need to configurate it in OnValueChange area
/// </summary>
[RequireComponent(typeof(Slider))]
public class AudioVolumn : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The target audio mixer group")]
    AudioMixer audioMixer;

    [SerializeField]
    [Tooltip("The exposed parameter of the audio mixer to change its volumn")]
    string exposedPara;

    string saveFileKey;

    [SerializeField]
    [Tooltip("The default volumn of the audio mixer")]
    float defaultVolumn = 0f;

    float maxVolumn = 20f;
    float minVolumn = -80f;

    float currentVolumn;

    SaveKeyRegister saveKeyRegister;

    Slider slider;

    private void Start()
    {
        slider = GetComponent<Slider>();       
        slider.maxValue = maxVolumn;
        slider.minValue = minVolumn;

        saveKeyRegister = GameManager.instance.saveKeyRegister;

        saveFileKey = exposedPara + "Volumn";
        if (PlayerPrefs.HasKey(saveFileKey))
        {
            currentVolumn = PlayerPrefs.GetFloat(saveFileKey);
        }
        else
        {
            currentVolumn = defaultVolumn;
            saveKeyRegister.RegisterKey(saveFileKey, true);
        }
        
        slider.value = currentVolumn;
        SetVolumn(currentVolumn);
        slider.onValueChanged.AddListener(SetVolumn);
    }

    public void SetVolumn(float newValue)
    {
        currentVolumn = newValue;
        audioMixer.SetFloat(exposedPara, currentVolumn);
        PlayerPrefs.SetFloat(saveFileKey, currentVolumn);
    }
}
