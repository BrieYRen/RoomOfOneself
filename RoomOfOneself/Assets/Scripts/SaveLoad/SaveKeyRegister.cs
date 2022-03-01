using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// this script is used to record all game save file keys into two lists for further use such as save all, load all or delete all keys and return to default
/// </summary>
public class SaveKeyRegister : MonoBehaviour
{
    /// <summary>
    /// save files about game options like sound volumn, graphic quality, etc
    /// </summary>
    [HideInInspector]
    public List<string> optionSaveKeys = new List<string>();

    /// <summary>
    /// save files about game progression
    /// </summary>
    [HideInInspector]
    public List<string> gameSaveKeys = new List<string>();

    /// <summary>
    /// public method to add a new save file key
    /// </summary>
    /// <param name="newKey"></param>
    /// <param name="isOptionKey"></param>
    public void RegisterKey(string newKey, bool isOptionKey)
    {
        List<string> targetList;
        targetList = isOptionKey ? optionSaveKeys : gameSaveKeys;

        if (!targetList.Contains(newKey))
            targetList.Add(newKey);
    }

    /// <summary>
    /// public method to remove an existing save file key
    /// </summary>
    /// <param name="newKey"></param>
    /// <param name="isOptionKey"></param>
    public void UnregisterKey(string newKey, bool isOptionKey)
    {
        List<string> targetList;
        targetList = isOptionKey ? optionSaveKeys : gameSaveKeys;

        if (targetList.Contains(newKey))
            targetList.Remove(newKey);
    }

}
