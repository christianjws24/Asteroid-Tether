using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    public int health;
    public float speed;

    private Vector2 moveDirection;

    public InputActionReference move;

    [Header("UI Referencias")]
    // 1. AGREGA ESTA VARIABLE: Para poder comunicarte con el menú
    [SerializeField] private GameOverMenu gameOverMenu;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 2. OPTIMIZACIÓN: Si el juego ya terminó, no proceses movimiento
        if (Time.timeScale == 0f)
        {
            moveDirection = Vector2.zero;
            return;
        }

        moveDirection = move.action.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveDirection.x * speed, moveDirection.y * speed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Asteroid"))
        {
            // 3. CAMBIO AQUÍ: Activa el menú de Game Over y marca estado global
            if (gameOverMenu != null)
            {
                gameOverMenu.SetGameOverActive(true);
            }
            else
            {
                // Fallback por si olvidas arrastrarlo en el inspector
                Time.timeScale = 0;
                Debug.LogWarning("¡No has asignado el GameOverMenu en el PlayerController!");
            }

            // Marcar estado de juego como terminado para que otros sistemas lo respeten
            GameStateManager.SetGameOver(true);
        }
    }
}
