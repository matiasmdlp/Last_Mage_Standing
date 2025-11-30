using UnityEngine;
using System.Collections.Generic;

/// Clase auxiliar para definir qué enemigos pueden aparecer y a qué nivel.
[System.Serializable] 
public class SpawnableEnemy
{
    public GameObject enemyPrefab;
    public int requiredLevel; 
}
public class EnemySpawner : MonoBehaviour
{
    public List<SpawnableEnemy> enemyPool;
    public GameObject eliteEnemyPrefab;
    public float spawnInterval = 1.5f;
    public int maxEnemies = 20;
    public float spawnRadius = 15f;

    private Transform playerTransform;

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        // Inicia una llamada repetitiva a SpawnEnemy después de 2s, y luego por cada 'spawnInterval'.
        InvokeRepeating("SpawnEnemy", 2f, spawnInterval);
    }

    /// Genera un enemigo aleatorio de la lista de enemigos disponibles según el nivel del jugador.
    void SpawnEnemy()
    {
        if (playerTransform == null || LevelSystem.instance == null) return;
        if (GameObject.FindGameObjectsWithTag("Enemy").Length >= maxEnemies) return;

        // Crea una lista temporal con los enemigos que pueden aparecer en el nivel actual.
        List<GameObject> availableEnemies = new List<GameObject>();
        foreach (SpawnableEnemy enemy in enemyPool)
        {
            if (LevelSystem.instance.level >= enemy.requiredLevel)
            {
                availableEnemies.Add(enemy.enemyPrefab);
            }
        }

        if (availableEnemies.Count > 0)
        {
            int randomIndex = Random.Range(0, availableEnemies.Count);
            GameObject enemyToSpawn = availableEnemies[randomIndex];

            Vector2 spawnDirection = Random.insideUnitCircle.normalized * spawnRadius;
            Vector2 spawnPosition = (Vector2)playerTransform.position + spawnDirection;

            Instantiate(enemyToSpawn, spawnPosition, Quaternion.identity);
        }
    }
    
    /// Genera un enemigo de élite. Este método es llamado externamente (por LevelSystem).
    public void SpawnElite()
    {
        if (playerTransform == null || eliteEnemyPrefab == null) return;
        
        Debug.Log("¡Un enemigo de ÉLITE ha aparecido!");

        Vector2 spawnDirection = Random.insideUnitCircle.normalized * spawnRadius;
        Vector2 spawnPosition = (Vector2)playerTransform.position + spawnDirection;

        Instantiate(eliteEnemyPrefab, spawnPosition, Quaternion.identity);
    }
}