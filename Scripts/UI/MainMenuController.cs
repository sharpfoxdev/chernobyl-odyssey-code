using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Class containing callback functions for main menu buttons. 
/// </summary>
public class MainMenuController : MonoBehaviour {
    /// <summary>
    /// Callback for pressing play button, loads following scene
    /// </summary>
    public void PlayButtonPressed() {
        SceneManager.LoadScene("IntroCutscene");
    }
    /// <summary>
    /// Callback for pressing exit button, shuts down the game
    /// </summary>
    public void ExitButtonPressed() {
        Application.Quit();
    }

    /// <summary>
    /// Callback for pressing credits button, loads scene with credits
    /// </summary>
    public void CreditsButtonPressed() {
        SceneManager.LoadScene("Credits");
    }
    /// <summary>
    /// UI callback, when main menu button is pressed in the credits,
    /// loads scene with main menu
    /// </summary>
    public void MainMenuButtonPressed() {
        SceneManager.LoadScene("MainMenu");
    }
}
