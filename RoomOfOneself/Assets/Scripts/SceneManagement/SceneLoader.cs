using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// manage scene loading progress as well as its fade in and out effect
/// </summary>
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CanvasGroup))]
public class SceneLoader : MonoBehaviour
{
    /// <summary>
    /// how many seconds will the fade in and fade out effect last
    /// </summary>
    public float fadeDuration = 1.5f;

    Animator animator;
    readonly string fadeOutAnimTrigger = "fadeOut";
    readonly string fadeInAnimTrigger = "fadeIn";

    /// <summary>
    /// a save file key to store which scene the player is now in
    /// </summary>
    public readonly string sceneIndexSaveKey = "sceneIndex";
    SaveKeyRegister saveKeyRegister;

    private void Start()
    {
        animator = GetComponent<Animator>();
        saveKeyRegister = GameManager.instance.saveKeyRegister;

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    /// <summary>
    /// public method to load a scene by its build index
    /// </summary>
    /// <param name="sceneIndex"></param>
    public void LoadCertainScene(int sceneIndex)
    {
        StartCoroutine(LoadInSec(fadeDuration, sceneIndex));
    }

    /// <summary>
    /// public method to load the next scene
    /// </summary>
    public void LoadNextScene()
    {
        StartCoroutine(LoadInSec(fadeDuration, SceneManager.GetActiveScene().buildIndex + 1));
    }

    /// <summary>
    /// the loading procedure, including a fade out animation of current scene and the procedure to show when the new scene is asynchronous loading
    /// </summary>
    /// <param name="delaySec"></param>
    /// <param name="sceneIndex"></param>
    /// <returns></returns>
    IEnumerator LoadInSec(float delaySec, int sceneIndex)
    {
        if (animator != null)
            animator.SetTrigger(fadeOutAnimTrigger);
        
        yield return new WaitForSecondsRealtime(delaySec);

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Single);

        while(!asyncOperation.isDone)
        {
            // do something when the scene is loading if needed
            Debug.Log(asyncOperation.progress);

            yield return null;
        }
    }

    /// <summary>
    /// when a new scene is loaded, play the fade in animation
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="loadSceneMode"></param>
    void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (animator != null)
            animator.SetTrigger(fadeInAnimTrigger);
    }


    /// <summary>
    /// public method to save which scene index the player has unlocked
    /// </summary>
    /// <param name="saveNext"></param>
    public void SaveSceneProgression(bool saveNext)
    {
        int tempIndex = SceneManager.GetActiveScene().buildIndex;

        if (saveNext)
            tempIndex++;

        if (!PlayerPrefs.HasKey(sceneIndexSaveKey))
            saveKeyRegister.RegisterKey(sceneIndexSaveKey, false);

        PlayerPrefs.SetInt(sceneIndexSaveKey, tempIndex);    
    }

}
