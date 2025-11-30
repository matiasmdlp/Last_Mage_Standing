using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

/// Gestiona la interfaz de usuario de la pantalla de "Game Over".
public class GameOverUI : MonoBehaviour
{
    /// Texto para mostrar la puntuación final obtenida.
    public TextMeshProUGUI finalScoreText;
    /// Campo de texto para que el jugador introduzca su nombre.
    public TMP_InputField nameInputField;

    /// Se ejecuta cada vez que el GameObject se activa.
    /// Muestra la puntuación y pausa el juego.
    void OnEnable()
    {
        finalScoreText.text = "Final Score: " + LevelSystem.finalScore.ToString();
        Time.timeScale = 0f;
    }

    /// Guarda la puntuación y vuelve al menú principal.
    /// Este método es llamado por un botón en la UI.
    public void SaveScore()
    {
        string playerName = nameInputField.text;
        if (string.IsNullOrEmpty(playerName))
        {
            playerName = "AAA";
        }

        HighScoreManager.AddScore(playerName, LevelSystem.finalScore);

        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}