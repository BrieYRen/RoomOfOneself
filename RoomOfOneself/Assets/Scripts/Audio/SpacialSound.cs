using UnityEngine;

/// <summary>
/// a derived class from Sound, can be used to store a 3d sound data 
/// </summary>
[System.Serializable]
public class SpacialSound : Sound
{
    [Header("Spacial Settings")]

    [Tooltip("How much this sound is affected by spacial settings, value 0 means totally ignore spacial settings, value 1 means totally determined by spacial settings")]
    [Range(0f,1f)]
    public float spatialBlend = 1f;

    [Tooltip("Within how many meters the player can hear the max volumn of this sound")]
    public float minDistance = 1f;

    [Tooltip("Within how many meters the player can hear a hint of this sound")]
    public float maxDistance = 5f;
}
