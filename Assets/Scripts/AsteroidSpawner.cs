using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    public GameObject[] asteroidPrefabs;
    public float minSpawnRate;
    public float maxSpawnRate;

    [Header("Progression")]
    public float spawnDecreasePerSecond = 0.01f; // cuánto se reduce la tasa por segundo
    public float minSpawnRateLimit = 0.2f; // límite inferior para evitar spawn instantáneo

    public float minSpeed;
    public float maxSpeed;

    public float screenMargin;

    private Camera mainCamera;
    private float nextSpawnTime;
    private float elapsedPlayTime;
    private float initialMinSpawnRate;
    private float initialMaxSpawnRate;

    private void Start()
    {
        mainCamera = Camera.main;
        initialMinSpawnRate = minSpawnRate;
        initialMaxSpawnRate = maxSpawnRate;
    }

    private void Update()
    {
        // No spawnear si el juego terminó
        if (GameStateManager.IsGameOver) return;

        // Contabilizar tiempo de juego para la progresión
        elapsedPlayTime += Time.deltaTime;

        // Ajustar rates actuales reduciéndolos con el tiempo (más frecuentes)
        float currentMin = Mathf.Max(initialMinSpawnRate - spawnDecreasePerSecond * elapsedPlayTime, minSpawnRateLimit);
        float currentMax = Mathf.Max(initialMaxSpawnRate - spawnDecreasePerSecond * elapsedPlayTime, minSpawnRateLimit + 0.01f);

        if (Time.time >= nextSpawnTime)
        {
            SpawnAsteroid();
            nextSpawnTime = Time.time + Random.Range(currentMin, currentMax);
        }
    }

    private void SpawnAsteroid()
    {
        if (asteroidPrefabs.Length == 0) return;

        Vector2 spawnPosition = GetRandomSpawnPosition();
        Vector2 targetPosition = GetRandomScreenPosition();

        int randomIndex = Random.Range(0, asteroidPrefabs.Length);
        GameObject asteroid = Instantiate(asteroidPrefabs[randomIndex], spawnPosition, Quaternion.identity);

        Vector2 direction = (targetPosition - spawnPosition).normalized;

        float randomSpeed = Random.Range(minSpeed, maxSpeed);

        Rigidbody2D rb = asteroid.GetComponent<Rigidbody2D>();
        if(rb != null)
        {
            rb.linearVelocity = direction * randomSpeed;

            rb.angularVelocity = Random.Range(-50f, 50f);
        }
    }

    private Vector2 GetRandomSpawnPosition()
    {
        // Viewport va de 0 a 1 (0,0 es abajo-izquierda; 1,1 es arriba-derecha)
        float viewX = 0f;
        float viewY = 0f;

        // Elegimos un borde al azar: 0 = Izquierda, 1 = Derecha, 2 = Abajo, 3 = Arriba
        int edge = Random.Range(0, 4);

        switch (edge)
        {
            case 0: // Izquierda
                viewX = 0f;
                viewY = Random.Range(0f, 1f);
                break;
            case 1: // Derecha
                viewX = 1f;
                viewY = Random.Range(0f, 1f);
                break;
            case 2: // Abajo
                viewX = Random.Range(0f, 1f);
                viewY = 0f;
                break;
            case 3: // Arriba
                viewX = Random.Range(0f, 1f);
                viewY = 1f;
                break;
        }

        // Convertimos el punto de la pantalla a coordenadas del mundo del juego
        Vector3 worldPoint = mainCamera.ViewportToWorldPoint(new Vector3(viewX, viewY, mainCamera.nearClipPlane));
        Vector2 spawnPos = new Vector2(worldPoint.x, worldPoint.y);

        // Le sumamos el margen en la dirección correcta para que aparezca fuera de vista
        if (edge == 0) spawnPos.x -= screenMargin;
        if (edge == 1) spawnPos.x += screenMargin;
        if (edge == 2) spawnPos.y -= screenMargin;
        if (edge == 3) spawnPos.y += screenMargin;

        return spawnPos;
    }

    // Devuelve un punto aleatorio totalmente dentro de los límites visibles de la pantalla
    private Vector2 GetRandomScreenPosition()
    {
        // Tomamos un punto intermedio (por ejemplo, entre el 15% y el 85% del centro de la pantalla)
        // Esto evita que los asteroides solo rocen las esquinas exteriores
        float randomX = Random.Range(0.15f, 0.85f);
        float randomY = Random.Range(0.15f, 0.85f);

        Vector3 worldPoint = mainCamera.ViewportToWorldPoint(new Vector3(randomX, randomY, mainCamera.nearClipPlane));
        return new Vector2(worldPoint.x, worldPoint.y);
    }

}
