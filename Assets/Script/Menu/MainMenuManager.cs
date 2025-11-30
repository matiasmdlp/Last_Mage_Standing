using UnityEngine;
using UnityEngine.SceneManagement; 
using TMPro; 
using System.Collections.Generic;

/// Gestiona la funcionalidad de la interfaz de usuario en el menú principal.
public class MainMenuManager : MonoBehaviour
{
    public GameObject recordsPanel;
    public GameObject creditsPanel;

    /// Lista de componentes de texto para mostrar las puntuaciones más altas
    public List<TextMeshProUGUI> scoreTexts;

    void Start()
    {
        recordsPanel.SetActive(false);
        creditsPanel.SetActive(false);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void ShowRecords()
    {
        recordsPanel.SetActive(true);
        DisplayHighScores();
    }

    public void ShowCredits()
    {
        creditsPanel.SetActive(true);
    }

    /// Oculta todos los paneles adicionales. Llamado por botones de "Atrás".
    public void HidePanels()
    {
        recordsPanel.SetActive(false);
        creditsPanel.SetActive(false);
    }

    /// Carga y muestra las puntuaciones más altas en los elementos de texto de la UI.
    void DisplayHighScores()
    {
        HighScoreList highScores = HighScoreManager.LoadScores();

        for (int i = 0; i < scoreTexts.Count; i++)
        {
            if (i < highScores.scores.Count)
            {
                scoreTexts[i].text = (i + 1) + ". " + highScores.scores[i].name + " - " + highScores.scores[i].score;
            }
            else
            {
                scoreTexts[i].text = (i + 1) + ". ---";
            }
        }
    }

    public void QuitGame()
    {
        Debug.Log("Saliendo del juego...");

        Application.Quit();
    }
}