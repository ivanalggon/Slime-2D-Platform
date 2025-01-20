using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    public void Retry()
    {
        // Cargar la escena del juego
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }
    public void Menu()
    {
        // Cargar la escena del menú
        UnityEngine.SceneManagement.SceneManager.LoadScene("PrincipalMenu");
    }
}
