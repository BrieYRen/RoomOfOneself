using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// the base sound class, can be used to store a 2d sound data
/// </summary>
[System.Serializable]
public class Sound
{
    [Header("Basic Settings")]

    [Tooltip("Name of the sound. Make if unique from other sounds for it will be used as key to find and play this sound.")]
    public string name;

    [Tooltip("Drag and Drop the audio clip here")]
    public AudioClip audioClip;
    [Tooltip("Drag and Drop an AudioMixierGroup here to determine which mixer group this audio clip is belong to")]
    public AudioMixerGroup audioMixerGroup;

    [Tooltip("Check true if the sound will play as soon as it's activated")]
    public bool playOnAwake;
    [Tooltip("Check true if want the sound to loop-play")]
    public bool isLoop;

    [Tooltip("The volumn of the sound, default value is 1")]
    [Range(0f, 1f)]
    public float volumn = 1f;
    [Tooltip("The pitch of the sound, default value is 1")]
    [Range(.1f, 3f)]
    public float pitch = 1f;


}
