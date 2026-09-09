using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    private UIDocument _uiDocument;
    private VisualElement mainMenuContainer;
    private VisualElement optionsMenuContainer;

    private Button playButton;
    private Button optionsButton;
    private Button backButton;

    private void OnEnable()
    {
        _uiDocument = GetComponent<UIDocument>();
        VisualElement root = _uiDocument.rootVisualElement;


        playButton = root.Q<Button>("PlayButton");
        optionsButton = root.Q<Button>("OptionsButton");
        backButton = root.Q<Button>("BackButton");

        mainMenuContainer = root.Q<VisualElement>("MainMenu");
        optionsMenuContainer = root.Q<VisualElement>("OptionsMenu");

        playButton.RegisterCallback<ClickEvent>(evt => PlayGame());


        optionsButton?.RegisterCallback<ClickEvent>(evt => ShowScreen(optionsMenuContainer, mainMenuContainer));

        backButton?.RegisterCallback<ClickEvent>(evt => ShowScreen(mainMenuContainer, optionsMenuContainer));
    }

    private void PlayGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    private void ShowScreen(VisualElement screenToShow, VisualElement screenToHide)
    {
        screenToShow.style.display = DisplayStyle.Flex;  // Muestra la pantalla
        screenToHide.style.display = DisplayStyle.None;  // Oculta la otra
    }
}
