using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// this script should be attach to a UI button
/// when player click the button, they'll exit main menu and restart the game with no previous progression
/// the method will be added automatically when start, no need to configurate it in OnClick area
/// </summary>
[RequireComponent(typeof(Button))]
public class NewGame : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The main menu panel to be closed when player click the button")]
    UIPanel toClosePanel;

    Button button;

    SaveKeyRegister saveKeyRegister;
    SceneLoader sceneLoader;

    /// <summary>
    /// change it if the build index of the first level is not 1
    /// </summary>
    readonly int firstLevelIndex = 1;

    private void Start()
    {
        button = GetComponent<Button>();

        saveKeyRegister = GameManager.instance.saveKeyRegister;
        sceneLoader = GameManager.instance.sceneLoader;

        button.onClick.AddListener(OnClickButton);
    }
    
    public void OnClickButton()
    {
        DeleteGameSaveFiles();
        LoadFirstLevel();

        StartCoroutine(CloseMainMenuInSec(sceneLoader.fadeDuration));
    }
   
    /// <summary>
    /// delete all previous progressions
    /// </summary>
    void DeleteGameSaveFiles()
    {
        if (saveKeyRegister.gameSaveKeys == null)
            return;

        for (int i = 0; i < saveKeyRegister.gameSaveKeys.Count; i++)
        {
            PlayerPrefs.DeleteKey(saveKeyRegister.gameSaveKeys[i]);
            saveKeyRegister.UnregisterKey(saveKeyRegister.gameSaveKeys[i], false);
        }
    }

    /// <summary>
    /// restart the game from the first level
    /// </summary>
    void LoadFirstLevel()
    {
        sceneLoader.LoadCertainScene(firstLevelIndex);
    }

    /// <summary>
    /// wait until the scene is loaded to prevent fade out twice
    /// </summary>
    /// <param name="waitSecs"></param>
    /// <returns></returns>
    IEnumerator CloseMainMenuInSec(float waitSecs)
    {
        yield return new WaitForSecondsRealtime(waitSecs);

        toClosePanel.Close();
    }

}
