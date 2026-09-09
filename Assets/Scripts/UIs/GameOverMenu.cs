using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    [Header("UI Toolkit")]
    public UIDocument gameOverDocument;

    private Button retryButton;
    private Button exitButton;
    private VisualElement rootElement;

    void Start()
    {
        if (gameOverDocument == null)
        {
            gameOverDocument = GetComponent<UIDocument>();
        }

        if (gameOverDocument != null)
        {
            rootElement = gameOverDocument.rootVisualElement;

            retryButton = rootElement.Q<Button>("RetryButton");
            exitButton = rootElement.Q<Button>("ExitButton");

            if (retryButton != null) retryButton.clicked += RetryGame;
            if (exitButton != null) exitButton.clicked += ExitGame;
        }

        SetGameOverActive(false);
    }

    public void SetGameOverActive(bool active)
    {
        if (rootElement != null)
        {
            rootElement.style.display = active ? DisplayStyle.Flex : DisplayStyle.None;
        }

        if (active)
        {
            Time.timeScale = 0f;
        }
    }

    private void RetryGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void ExitGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
