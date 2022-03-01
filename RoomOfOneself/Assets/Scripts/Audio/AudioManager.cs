using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// this is the script to manage all sounds in game including init 2d sounds when game start, init 3d sounds when a scene loaded and release them when a scene is unloaded, and play or stop playing certain sound with fade effect
/// </summary>
public class AudioManager : MonoBehaviour
{
    [Tooltip("2d sounds that will play on camera")]
    [SerializeField]
    List<Sound> flatSounds;

    [Tooltip("3d sounds that will play on certain gameObject")]
    [SerializeField]
    List<SpacialSound> spacialSounds;

    /// <summary>
    /// the audio dictionary to save all initiated audio sources ready to play
    /// </summary>
    Dictionary<string, AudioSource> audioDict;


    private void Start()
    {
        audioDict = new Dictionary<string, AudioSource>();

        Init2DSounds();

        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    /// <summary>
    /// this will only be called once when game starts to initiate all 2d sounds
    /// </summary>
    void Init2DSounds()
    {
        if (flatSounds == null)
        {
            flatSounds = new List<Sound>();
        }
        else
        {
            foreach (Sound s in flatSounds)
            {
                GameObject gameObject = new GameObject(s.name);
                gameObject.transform.SetParent(transform);

                AudioSource audioSource = gameObject.AddComponent<AudioSource>();
                audioSource.clip = s.audioClip;
                audioSource.outputAudioMixerGroup = s.audioMixerGroup;
                audioSource.playOnAwake = s.playOnAwake;
                audioSource.loop = s.isLoop;
                audioSource.volume = s.volumn;
                audioSource.pitch = s.pitch;

                audioDict.Add(s.name, audioSource);
            }
        }
    }

    /// <summary>
    /// this should be called by the scripts which will call PlayIfHasAudio() method to play a spatial sound later when a new scene is loaded 
    /// </summary>
    /// <param name="gameObject"></param>
    public string Init3DSound(string newName, GameObject gameObject)
    {
        if (spacialSounds == null)
            return null;

        string audioSourceKeyToCall = null;

        for(int i = 0; i < spacialSounds.Count; i++)
        {
            if (spacialSounds[i].name == newName)
            {
                AudioSource audioSource;
                if (!gameObject.TryGetComponent<AudioSource>(out audioSource))
                    audioSource = gameObject.AddComponent<AudioSource>();

                audioSource.clip = spacialSounds[i].audioClip;
                audioSource.outputAudioMixerGroup = spacialSounds[i].audioMixerGroup;
                audioSource.playOnAwake = spacialSounds[i].playOnAwake;
                audioSource.loop = spacialSounds[i].isLoop;
                audioSource.volume = spacialSounds[i].volumn;
                audioSource.pitch = spacialSounds[i].pitch;
                audioSource.spatialBlend = spacialSounds[i].spatialBlend;
                audioSource.minDistance = Mathf.Max(0f, spacialSounds[i].minDistance);
                audioSource.maxDistance = Mathf.Max(spacialSounds[i].minDistance, spacialSounds[i].maxDistance);

                bool hadKey = true;               
                while(hadKey)
                {
                    int tempIndex = Random.Range(0, 999);
                    string tempString = spacialSounds[i].name + tempIndex.ToString();
                    if (!audioDict.ContainsKey(tempString))
                    {
                        audioSourceKeyToCall = tempString;
                        hadKey = false;
                    }                       
                }

                audioDict.Add(audioSourceKeyToCall, audioSource);
            }
        }

        return audioSourceKeyToCall;
      
    }

    /// <summary>
    /// when a previous scene is unloaded, find out all null spacial audio sources in that unloaded scene and remove them from the audio dictionary
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
    void OnSceneUnloaded(Scene scene)
    {
        RemoveMissingSpacialSources();
    }

    void RemoveMissingSpacialSources()
    {
        List<string> audioDictNames = audioDict.Keys.ToList<string>();
        List<AudioSource> audioDictSources = audioDict.Values.ToList<AudioSource>();
        
        for (int i = 0; i < audioDictSources.Count; i++)
        {
            if (audioDictSources[i] == null)
                audioDict.Remove(audioDictNames[i]);
        }
        
    }

    /// <summary>
    /// begin to play an audio with optional fade in effect
    /// </summary>
    /// <param name="name"></param>
    /// <param name="fadeInSec"></param>
    public void PlayIfHasAudio(string name, float fadeInSec)
    {
        if (name == null)
            return;

        if (!audioDict.ContainsKey(name))
        {
            Debug.Log(name + " not founded!");
            return;
        }
            
        if (audioDict[name].isPlaying && audioDict[name].loop)
            return;

        StartCoroutine(FadeInCertainSound(name, fadeInSec));
 
    }

    IEnumerator FadeInCertainSound(string name, float fadeInDuration)
    {
        float currentTime = 0f;
        float targetVolumn = audioDict[name].volume;

        audioDict[name].volume = 0f;
        audioDict[name].Play();

        while (currentTime < fadeInDuration)
        {
            currentTime += Time.deltaTime;
            audioDict[name].volume = Mathf.Lerp(0f, targetVolumn, currentTime / fadeInDuration);
            yield return null;
        }

        audioDict[name].volume = targetVolumn;
        yield break;
    }

    /// <summary>
    /// stop playing a certain audio with optional fade out effect
    /// </summary>
    /// <param name="name"></param>
    /// <param name="fadeOutSec"></param>
    public void StopPlayCertainAudio(string name, float fadeOutSec)
    {
        if (name == null)
            return;

        if (!audioDict.ContainsKey(name))
        {
            Debug.Log(name + " not founded!");
            return;
        }

        if (!audioDict[name].isPlaying)
        {
            Debug.Log(name + " is not playing!");
            return;
        }

        StartCoroutine(FadeOutCertainSound(name, fadeOutSec));
    }

    IEnumerator FadeOutCertainSound(string name, float fadeOutDuration)
    {
        float currentTime = 0f;
        float startVolumn = audioDict[name].volume;

        while (currentTime < fadeOutDuration)
        {
            currentTime += Time.deltaTime;
            audioDict[name].volume = Mathf.Lerp(startVolumn, 0f, currentTime / fadeOutDuration);
            yield return null;
        }

        audioDict[name].Stop();
        audioDict[name].volume = startVolumn;
        yield break;
    }

}
