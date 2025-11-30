using UnityEngine;
using UnityEngine.SceneManagement;

/// Gestiona el estado de pausa del juego.
public class PauseManager : MonoBehaviour
{
    public GameObject pausePanel;
    public static bool isPaused = false;

    void Start()
    {
        ResumeGame();
    }

    void Update()
    {
        // Detecta si se presiona la tecla Escape para pausar o reanudar.
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void ResumeGame()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void PauseGame()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        isPaused = false;
        SceneManager.LoadScene("MainMenu");
    }
}
