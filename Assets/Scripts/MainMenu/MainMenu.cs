using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void StartPlaying()
    {
        // Load the game scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }
    public void ShowCredits()
    {
        // Load the credits scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("Credits");
    }
    public void ReturnMainMenu()
    {
        // Load the main menu scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("PrincipalMenu");
    }
    public void ExitGame()
    {
        // Exit the game
        Application.Quit();
    }
}
