using System.Collections; 
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// Gestiona la experiencia (XP), el nivel, y la puntuación del jugador.
/// También controla la interfaz de usuario (barra de XP, texto de nivel).
public class LevelSystem : MonoBehaviour
{
    public static LevelSystem instance;

    public int level = 1;
    public float currentXP = 0;
    public float requiredXP = 100;

    public Slider xpBar;
    public TextMeshProUGUI levelText;
    public GameObject upgradePanel;
    public int score = 0;
    public TextMeshProUGUI scoreText;
    public static int finalScore;
    public EnemySpawner enemySpawner;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (upgradePanel != null)
            upgradePanel.SetActive(false); // Asegura que el panel de mejoras esté oculto al inicio.

        UpdateUI();
        UpdateScoreUI();
        upgradePanel.SetActive(false);
    }

    /// Añade una cantidad de experiencia al jugador y comprueba si sube de nivel.
    public void AddXP(float xpGained)
    {
        currentXP += xpGained;
        if (currentXP >= requiredXP)
        {
            LevelUp();
        }
        UpdateUI();
    }

    /// Gestiona la subida de nivel del jugador.
    void LevelUp()
    {
        level++;

        if (level % 5 == 0)
        {
            if (enemySpawner != null)
            {
                enemySpawner.SpawnElite();
            }
        }

        currentXP -= requiredXP;
        requiredXP *= 1.5f;

        Time.timeScale = 0f;
        if (upgradePanel != null)
            upgradePanel.SetActive(true);

        Debug.Log("¡SUBISTE DE NIVEL! Nivel actual: " + level);
    }

    /// Actualiza los elementos de la UI relacionados con el nivel y la XP.
    void UpdateUI()
    {
        if (xpBar != null)
            xpBar.value = currentXP / requiredXP;

        if (levelText != null)
            levelText.text = "Level: " + level;
    }

    /// Oculta el panel de mejoras y reanuda el juego. Es llamado por los botones de mejora.
    public void HideUpgradePanel()
    {
        Time.timeScale = 1f;
        if (upgradePanel != null)
            upgradePanel.SetActive(false);
    }

    /// Añade puntos a la puntuación total del jugador.
    public void AddScore(int amount)
    {
        score += amount;
        finalScore = score;
        UpdateScoreUI();
    }

    /// Actualiza el texto de la puntuación en la UI.
    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score.ToString();
        }
    }
}