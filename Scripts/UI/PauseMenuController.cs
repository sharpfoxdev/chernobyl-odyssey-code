using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles pause menu, a bit similar to the main menu controller, 
/// but also is handling stopping and playing the game time
/// </summary>
public class PauseMenuController : MonoBehaviour
{
    private bool gamePaused = false;
    // object holding the UI of pause menu (buttons, background)
    [SerializeField]
    private GameObject pauseMenu;

    /// <summary>
    /// This callback is currently mapped to escape, pauses the game, 
    /// or unpauses it, if the game was already paused
    /// </summary>
    public void OnKeydownPauseGame() {
        if (gamePaused) {
            ResumeGame();
        }
        else {
            PauseGame();
        }
    }
    /// <summary>
    /// Stops the game time and displays pause menu UI
    /// </summary>
    private void PauseGame() {
        pauseMenu.SetActive(true);
        gamePaused = true;
        //this stops the game time
        Time.timeScale = 0f;
    }
    /// <summary>
    /// Plays the stopped game time and disappears pause menu UI
    /// </summary>
    private void ResumeGame() {
        //plays game time again
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        gamePaused = false;

    }
    /// <summary>
    /// UI callback for when the resume button is pressed in the pause menu. Resumes the game. 
    /// </summary>
    public void ResumeButtonPressed() {
        ResumeGame();
    }

    /// <summary>
    /// UI callback, when exit button is pressed in the pause menu, kills the game. 
    /// </summary>
    public void ExitButtonPressed() {
        Application.Quit();
    }

    /// <summary>
    /// UI callback, when main menu button is pressed in the pause menu,
    /// loads scene with main menu
    /// </summary>
    public void MainMenuButtonPressed() {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
