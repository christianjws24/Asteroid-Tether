using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("UI Toolkit")]
    public UIDocument pauseMenu;

    [Header("Input de Pausa")]
    public InputActionReference pauseActionReference;

    [Header("Control de Mapas de Input")]
    public InputActionAsset playerInputAsset;
    [SerializeField] private string gameplayMapName = "Player"; // Cambia "Player" por el nombre exacto de tu mapa de juego

    private Button resumeButton;
    private Button exitButton;

    private VisualElement rootElement;
    private bool isPaused = false;

    private void OnEnable()
    {
        if (pauseActionReference != null)
        {
            pauseActionReference.action.Enable();
            pauseActionReference.action.performed += OnPausePerformed;
        }
    }

    private void OnDisable()
    {
        if (pauseActionReference != null)
        {
            pauseActionReference.action.performed -= OnPausePerformed;
            pauseActionReference.action.Disable();
        }
    }

    private void Start()
    {
        if (pauseMenu != null)
        {
            rootElement = pauseMenu.rootVisualElement;

            resumeButton = rootElement.Q<Button>("ResumeButton");
            exitButton = rootElement.Q<Button>("ExitButton");

            if (resumeButton != null) resumeButton.clicked += ResumeGame;
            if (exitButton != null) exitButton.clicked += ExitGame;
        }

        SetPaused(false);
    }

    private void OnPausePerformed(InputAction.CallbackContext context)
    {
        TogglePause();
    }

    private void TogglePause()
    {
        SetPaused(!isPaused);
    }

    private void SetPaused(bool v)
    {
        isPaused = v;

        if (rootElement != null)
        {
            rootElement.style.display = isPaused ? DisplayStyle.Flex : DisplayStyle.None;
        }

        Time.timeScale = isPaused ? 0f : 1f;

        // --- CORRECCIÓN DEL MOUSE: Desactivar solo el mapa de gameplay ---
        if (playerInputAsset != null)
        {
            var gameplayMap = playerInputAsset.FindActionMap(gameplayMapName);
            var uiMap = playerInputAsset.FindActionMap("UI"); // Busca el mapa de UI si existe

            if (gameplayMap != null)
            {
                if (isPaused) gameplayMap.Disable(); // Apaga movimiento/disparo
                else gameplayMap.Enable();           // Reactiva movimiento/disparo
            }

            if (uiMap != null)
            {
                if (isPaused) uiMap.Enable();        // Asegura que el mouse de la UI funcione
                else uiMap.Disable();
            }
        }

        // Control del cursor físico

    }

    private void ResumeGame()
    {
        SetPaused(false);
    }

    private void ExitGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
