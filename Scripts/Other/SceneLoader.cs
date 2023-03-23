using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This script is used in the first cutscene, that uses enabling and disbling of
/// various games object. This script is attached to the last object, that gets enabled, 
/// and when it gets enabled, it loads the first map
/// </summary>
public class SceneLoader : MonoBehaviour
{
    /// <summary>
    /// Loads first map upon enabling in the timeline sequence
    /// </summary>
    void OnEnable() {
        SceneManager.LoadScene("Map1", LoadSceneMode.Single);
    }
}
