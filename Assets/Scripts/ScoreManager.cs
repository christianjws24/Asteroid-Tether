using System;
using UnityEngine;

public static class ScoreManager
{
    // Evento que notifica el score total actual
    public static event Action<int> OnScoreChanged;
    // Evento que notifica cuando cambia el score máximo (high score)
    public static event Action<int> OnHighScoreChanged;

    private static int score;
    private static int highScore;

    static ScoreManager()
    {
        // Cargar high score persistente
        highScore = PlayerPrefs.GetInt("HighScore", 0);
    }

    public static void AddScore(int amount)
    {
        if (amount == 0) return;
        score += amount;
        OnScoreChanged?.Invoke(score);

        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
            OnHighScoreChanged?.Invoke(highScore);
        }
    }

    public static int GetScore()
    {
        return score;
    }

    public static int GetHighScore()
    {
        return highScore;
    }

    public static void Reset()
    {
        score = 0;
        OnScoreChanged?.Invoke(score);
    }

    public static void ResetHighScore()
    {
        highScore = 0;
        PlayerPrefs.SetInt("HighScore", 0);
        PlayerPrefs.Save();
        OnHighScoreChanged?.Invoke(highScore);
    }
}
