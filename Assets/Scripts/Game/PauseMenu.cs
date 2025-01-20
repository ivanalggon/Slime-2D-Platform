using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI; // Asignar el Canvas del menú de pausa.

    private bool isPaused = false;

    void Start()
    {
        pauseMenuUI.SetActive(false); // Esconder el menú al iniciar.
    }
    void Update()
    {
        // Detectar la tecla de pausa (Escape).
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume(); // Reanudar el juego.
            }
            else
            {
                Pause(); // Pausar el juego.
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false); // Esconde el menú.
        Time.timeScale = 1f;          // Reanuda el tiempo.
        isPaused = false;             // Actualiza el estado.
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);  // Muestra el menú.
        Time.timeScale = 0f;          // Detiene el tiempo.
        isPaused = true;              // Actualiza el estado.
    }

    public void LoadMenu()
    {
        // Cargar la escena del menú principal.
        UnityEngine.SceneManagement.SceneManager.LoadScene("PrincipalMenu");
    }
}