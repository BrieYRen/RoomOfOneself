using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// the only singleton of the game, used as a medium for scripts to communicated with managers
/// </summary>
public class GameManager : MonoBehaviour
{
    #region Singleton & DontDestroyOnload

    public static GameManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }           
        else
        {
            Destroy(gameObject);
            return;
        }          

    }

    #endregion

    /// <summary>
    /// the audio manager to init and play audio, it's also the parent of all 2d audio resource objects in game
    /// </summary>
    public AudioManager audioManager;

    /// <summary>
    /// the input handler to regist hotkeys for ui panels, determin which or none or all hotkeys are currently working, and check if an ui panel is triggered by its registered hotkey
    /// </summary>
    public PanelInputHandler panelInputHandler;

    /// <summary>
    /// the main ui prefab including a main menu, a pause menu, a setting menu, and a black panel for fade
    /// </summary>
    public GameObject mainUI;

    /// <summary>
    /// the scene manager to load certain scene with certain effects
    /// </summary>
    public SceneLoader sceneLoader;

    /// <summary>
    /// the save load manager to store all save file keys in certain categories
    /// </summary>
    public SaveKeyRegister saveKeyRegister;
}
