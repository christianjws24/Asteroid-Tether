using UnityEngine;
using UnityEngine.UIElements;

public class GameUI : MonoBehaviour
{
    private UIDocument _uiDocument;
    private Label scoreLabel;
    private Label highScoreLabel;
    private int currentScore;

    private void OnEnable()
    {
        _uiDocument = GetComponent<UIDocument>();

        scoreLabel = _uiDocument.rootVisualElement.Q<Label>("Score");
        highScoreLabel = _uiDocument.rootVisualElement.Q<Label>("HighScore");
        // Subscribe to score changes and initialize display
        ScoreManager.OnScoreChanged += HandleScoreChanged;
        ScoreManager.OnHighScoreChanged += HandleHighScoreChanged;
        currentScore = ScoreManager.GetScore();
        UpdateScoreDisplay();
        // Inicializar high score
        if (highScoreLabel != null)
            highScoreLabel.text = $"Best: {ScoreManager.GetHighScore()}";
    }

    private void OnDisable()
    {
        ScoreManager.OnScoreChanged -= HandleScoreChanged;
        ScoreManager.OnHighScoreChanged -= HandleHighScoreChanged;
    }

    private void HandleScoreChanged(int newScore)
    {
        currentScore = newScore;
        UpdateScoreDisplay();
    }

    // Método público para sumar puntos o cambiar el score
    public void AddScore(int amount)
    {
        currentScore += amount;
        UpdateScoreDisplay();
    }

    private void UpdateScoreDisplay()
    {
        if (scoreLabel != null)
        {
            scoreLabel.text = $"Score: {currentScore}";
        }
    }

    private void HandleHighScoreChanged(int newHigh)
    {
        if (highScoreLabel != null)
            highScoreLabel.text = $"Best: {newHigh}";
    }
}
