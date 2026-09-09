using System.Collections;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public int health;
    public int score;
    [SerializeField] private Material flashMaterial;
    [SerializeField] private float flashDuration;

    private SpriteRenderer spriteRenderer;
    private Material originalMaterial;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalMaterial = spriteRenderer.material;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            health--;
            StartCoroutine(FlashRoutine());
            if (health <= 0)
            {
                // Añadir puntuación antes de destruir el asteroide
                ScoreManager.AddScore(score);
                Destroy(gameObject);
            }

        }
        if (!collision.CompareTag("Player"))
        {
            Destroy(collision.gameObject);
        }
            
    }

    private IEnumerator FlashRoutine()
    {
        spriteRenderer.material = flashMaterial;

        // Esperar el tiempo indicado
        yield return new WaitForSeconds(flashDuration);

        // Volver al material original
        spriteRenderer.material = originalMaterial;
    }

}
