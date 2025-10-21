using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    public GameObject enemyPrefab;
    public int currentWave = 1;
    public int baseEnemies = 1;
    public int maxEnemyWave = 1;

    private int enemiesPerWave;

    void Start()
    {
        EnemiesForWave();
        SpawnEnemies();
    }

    public void StartNextWave()
    {
        currentWave++;
        EnemiesForWave();
        SpawnEnemies();
    }

    void EnemiesForWave()
    {
        enemiesPerWave = Mathf.RoundToInt(baseEnemies + (currentWave - 1) * (maxEnemyWave - baseEnemies) / 9f);
    }
    
    void SpawnEnemies()
    {
        for (int i = 0; i < enemiesPerWave; i++)
        {
            Vector3 randomPoint = GetRandomPointInCollider(GetComponent<Collider>());
            Instantiate(enemyPrefab, randomPoint, Quaternion.identity);
        }
    }

    Vector3 GetRandomPointInCollider(Collider collider)
    {
        Bounds bounds = collider.bounds;
        
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);
        float z = Random.Range(bounds.min.z, bounds.max.z);
        
        return new Vector3(x, y, z);
    }
}