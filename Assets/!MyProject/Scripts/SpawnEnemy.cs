using UnityEngine;
using UnityEngine.UI;

public class SpawnEnemy : MonoBehaviour
{
    public GameObject enemyPrefab;
    public int currentWave = 1;
    public int maxWave = 10;
    public Slider timerSlider;
    public float waveCooldown = 10f;

    private int enemiesPerWave;
    private float currentTimer;
    private bool isTimerRunning = false;
    private bool isFirstWave = true;

    public int MinEnemy = 1;
    public int MaxEnemy = 6;

    void Start()
    {
        {
            timerSlider.minValue = 0f;
            timerSlider.maxValue = 1f;
            timerSlider.value = 1f;
        }
        StartTimer();
    }

    void Update()
    {
        if (isTimerRunning)
        {
            UpdateTimer();
        }
    }

    void StartTimer()
    {
        currentTimer = waveCooldown;
        isTimerRunning = true;

        {
            timerSlider.value = 1f;
        }
    }

    void UpdateTimer()
    {
        currentTimer -= Time.deltaTime;

        {
            timerSlider.value = currentTimer / waveCooldown;
        }

        if (currentTimer <= 0)
        {
            isTimerRunning = false;

            if (isFirstWave)
            {
                SpawnFirstWave();
                isFirstWave = false;
                StartTimer();
            }
            else
            {
                SpawnPartialWave();

                if (currentWave <= maxWave)
                {
                    StartTimer();
                }
            }
        }
    }

    void SpawnFirstWave()
    {
        enemiesPerWave = 1;

        Vector3 randomPoint = GetRandomPointInCollider(GetComponent<Collider>());
        Instantiate(enemyPrefab, randomPoint, Quaternion.identity);

        Debug.Log($"First wave: Spawned 1 enemy");

        currentWave = 2;
    }

    void SpawnPartialWave()
    {
        EnemiesForWave();

        int enemiesToSpawn = Mathf.Min(Random.Range(MinEnemy, MaxEnemy), enemiesPerWave);

        for (int i = 0; i < enemiesToSpawn; i++)
        {
            Vector3 randomPoint = GetRandomPointInCollider(GetComponent<Collider>());
            Instantiate(enemyPrefab, randomPoint, Quaternion.identity);
        }

        Debug.Log($"Wave {currentWave}: Spawned {enemiesToSpawn} enemies (Total this wave: {enemiesPerWave})");

        currentWave++;

        if (currentWave > maxWave)
        {
            EndGame();
        }
    }

    void EnemiesForWave()
    {
        float progress = (float)(currentWave - 1) / (maxWave - 1);
        enemiesPerWave = Mathf.RoundToInt(Mathf.Lerp(1, MaxEnemy, progress));
        enemiesPerWave = Mathf.Max(1, enemiesPerWave);

        Debug.Log($"Wave {currentWave}: Progress = {progress:F2}, Enemies = {enemiesPerWave}");
    }

    void EndGame()
    {
        isTimerRunning = false;

        Debug.Log($"Game Over! Final wave: {currentWave}");

        {
            timerSlider.gameObject.SetActive(false);
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

    public void SetWaveCooldown(float newCooldown)
    {
        waveCooldown = newCooldown;
        if (isTimerRunning)
        {
            currentTimer = (currentTimer / waveCooldown) * newCooldown;
        }
    }

    public void ResetTimer()
    {
        StartTimer();
    }

    public void ForceNextWave()
    {

        if (isFirstWave)
        {
            SpawnFirstWave();
            isFirstWave = false;
        }
        else
        {
            SpawnPartialWave();
        }

        if (isTimerRunning)
        {
            StartTimer();
        }
    }


    public void RestartGame()
    {
        currentWave = 1;
        isFirstWave = true;

        {
            timerSlider.gameObject.SetActive(true);
            timerSlider.value = 1f;
        }

        StartTimer();
    }
}