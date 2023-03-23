using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script used for skipping cutscenes
/// </summary>
public class SkipCutscene : MonoBehaviour {
    [SerializeField]
    private GameObject nextSceneHolder;

    /// <summary>
    /// Sets active nextSceneHolder object, that normally loads the next scene
    /// in the end of the timeline of cutscene. It contains script, that loads next
    /// scene, when it gets enabled, be it by this script, or in the timeline. 
    /// </summary>
    public void OnSkipCutscene() {
        nextSceneHolder.SetActive(true);
    }
}
