using UnityEngine;
using System.Collections.Generic;
using System.Linq; 

[System.Serializable] 
public class HighScoreEntry
{
    public string name;
    public int score;
}

/// Contenedor para la lista de puntuaciones.
[System.Serializable]
public class HighScoreList
{
    public List<HighScoreEntry> scores = new List<HighScoreEntry>();
}

/// Clase estática para gestionar el guardado y la carga de las puntuaciones más altas.
public static class HighScoreManager
{
    private const string HighScoreKey = "HighScores";
    private const int MaxEntries = 10;

    public static void AddScore(string playerName, int playerScore)
    {
        HighScoreList highScores = LoadScores();

        highScores.scores.Add(new HighScoreEntry { name = playerName, score = playerScore });

        highScores.scores = highScores.scores.OrderByDescending(s => s.score).ToList();

        if (highScores.scores.Count > MaxEntries)
        {
            highScores.scores.RemoveRange(MaxEntries, highScores.scores.Count - MaxEntries);
        }

        SaveScores(highScores);
    }

    /// Carga las puntuaciones guardadas desde PlayerPrefs.
    public static HighScoreList LoadScores()
    {
        if (PlayerPrefs.HasKey(HighScoreKey))
        {
            string json = PlayerPrefs.GetString(HighScoreKey);
            return JsonUtility.FromJson<HighScoreList>(json);
        }
        else
        {
            return new HighScoreList();
        }
    }

    private static void SaveScores(HighScoreList highScores)
    {
        string json = JsonUtility.ToJson(highScores);
        PlayerPrefs.SetString(HighScoreKey, json);
        PlayerPrefs.Save();
    }
}