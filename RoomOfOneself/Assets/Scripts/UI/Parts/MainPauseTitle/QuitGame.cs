using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// this script should be attach to a UI button
/// when player click the button, they'll exit the game
/// the method will be added automatically when start, no need to configurate it in OnClick area
/// </summary>
[RequireComponent(typeof(Button))]
public class QuitGame : MonoBehaviour
{
    Button button;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClickQuit);
    }

    public void OnClickQuit()
    {
        #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }


}
