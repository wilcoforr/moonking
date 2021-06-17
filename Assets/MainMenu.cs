using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Main Menu functionality.
/// TODO: 
///     - Options menu for resolution/volume slider
///     - add a checkbox that saves the last level completed
/// </summary>
public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("LevelOne");
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadNextScene()
    {
        switch (PlayerMovement.LevelCount)
        {
            case 2:
                UnityEngine.SceneManagement.SceneManager.LoadScene("LevelTwo");
                break;
            case 3:
                UnityEngine.SceneManagement.SceneManager.LoadScene("LevelThree");
                break;
            case 4:
                UnityEngine.SceneManagement.SceneManager.LoadScene("PlayerWon");
                break;
            //should never get here but just in case? How else can we break the scene loading/level system?
            default:
                UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
                break;
        }
    }


    public void QuitGame()
    {
        Application.Quit();
    }
}
