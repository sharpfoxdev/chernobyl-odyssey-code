using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script is used in scenes, where we do not want the game music from maps to persist (since, 
/// they are DontDestroyOnLoad. For example in menu or cutscenes. 
/// </summary>
public class GameplayMusicDestroyer : MonoBehaviour
{
    private void Awake() {
        GameObject[] gameplayMusicPlayers = GameObject.FindGameObjectsWithTag("GameplayMusic");
        foreach (GameObject gameplayMusicPlayer in gameplayMusicPlayers) {
            Destroy(gameplayMusicPlayer);
        }
    }
}
