using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SpawnEnemy : MonoBehaviour
{
    public GameObject enemyPrefab;
    public int currentWave = 1;
    public int maxWave = 10;
    public int maxEnemiesAtWave10 = 6;
    public Slider timerSlider;
    public float waveCooldown = 10f;

    private int enemiesPerWave;
    private int enemiesSpawnedThisWave;
    private float currentTimer;
    private bool isTimerRunning = false;
    private bool isFirstWave = true;
    private bool gameEnded = false;

    // Новые переменные для постепенного спавна
    private List<float> spawnTimes = new List<float>();
    private int currentSpawnIndex = 0;

    void Start()
    {
        timerSlider.minValue = 0f;
        timerSlider.maxValue = 1f;
        timerSlider.value = 1f;

        StartTimer();
    }

    void Update()
    {
        if (isTimerRunning && !gameEnded)
        {
            UpdateTimer();
            CheckProgressiveSpawning();
        }
    }

    void StartTimer()
    {
        if (gameEnded) return;

        currentTimer = waveCooldown;
        isTimerRunning = true;
        enemiesSpawnedThisWave = 0;
        currentSpawnIndex = 0;
        spawnTimes.Clear();

        timerSlider.value = 1f;
    }

    void UpdateTimer()
    {
        currentTimer -= Time.deltaTime;
        timerSlider.value = currentTimer / waveCooldown;

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
                // Завершаем волну - спавним оставшихся врагов
                SpawnRemainingEnemies();

                if (currentWave <= maxWave)
                {
                    StartTimer();
                }
            }
        }
    }

    void CheckProgressiveSpawning()
    {
        if (spawnTimes.Count == 0 || currentSpawnIndex >= spawnTimes.Count) return;

        float timePassed = waveCooldown - currentTimer;
        float spawnTime = spawnTimes[currentSpawnIndex];

        if (timePassed >= spawnTime && enemiesSpawnedThisWave < enemiesPerWave)
        {
            SpawnSingleEnemy();
            currentSpawnIndex++;
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
        CalculateSpawnTimes();

        Debug.Log($"Wave {currentWave}: Will spawn {enemiesPerWave} enemies progressively");

        // Первый враг спавнится сразу
        if (enemiesPerWave > 0)
        {
            SpawnSingleEnemy();
            currentSpawnIndex++;
        }
    }

    void CalculateSpawnTimes()
    {
        spawnTimes.Clear();

        if (enemiesPerWave <= 1) return;

        // Равномерно распределяем время спавна оставшихся врагов
        for (int i = 1; i < enemiesPerWave; i++)
        {
            // Враги спавнятся равномерно в течение всей волны
            // Например: для 3 врагов - 0%, 50%, 100% времени
            // Для 4 врагов - 0%, 33%, 66%, 100% и т.д.
            float spawnTime = (float)i / (enemiesPerWave - 1) * waveCooldown;
            spawnTimes.Add(spawnTime);
        }

        // Логируем времена спавна для отладки
        string spawnLog = $"Wave {currentWave} spawn times: ";
        for (int i = 0; i < spawnTimes.Count; i++)
        {
            spawnLog += $"{spawnTimes[i]:F1}s ";
        }
        Debug.Log(spawnLog);
    }

    void SpawnSingleEnemy()
    {
        Vector3 randomPoint = GetRandomPointInCollider(GetComponent<Collider>());
        Instantiate(enemyPrefab, randomPoint, Quaternion.identity);

        enemiesSpawnedThisWave++;

        float timePassed = waveCooldown - currentTimer;
        Debug.Log($"Wave {currentWave}: Spawned enemy {enemiesSpawnedThisWave}/{enemiesPerWave} at {timePassed:F1}s");
    }

    void SpawnRemainingEnemies()
    {
        // Спавним всех оставшихся врагов в конце волны
        while (enemiesSpawnedThisWave < enemiesPerWave)
        {
            SpawnSingleEnemy();
        }

        currentWave++;

        if (currentWave > maxWave)
        {
            EndGame();
        }
    }

    void EnemiesForWave()
    {
        float progress = (float)(currentWave - 1) / (maxWave - 1);
        enemiesPerWave = Mathf.RoundToInt(Mathf.Lerp(1, maxEnemiesAtWave10, progress));
        enemiesPerWave = Mathf.Max(1, enemiesPerWave);

        Debug.Log($"Wave {currentWave}: Progress = {progress:F2}, Enemies = {enemiesPerWave}");
    }

    void EndGame()
    {
        gameEnded = true;
        isTimerRunning = false;

        Debug.Log($"Game Over! Final wave: {currentWave}");

        timerSlider.gameObject.SetActive(false);
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
        if (gameEnded) return;

        // Пересчитываем времена спавна при изменении длительности волны
        float oldCooldown = waveCooldown;
        waveCooldown = newCooldown;

        if (isTimerRunning)
        {
            // Масштабируем текущий таймер
            currentTimer = (currentTimer / oldCooldown) * newCooldown;

            // Масштабируем времена спавна
            for (int i = 0; i < spawnTimes.Count; i++)
            {
                spawnTimes[i] = (spawnTimes[i] / oldCooldown) * newCooldown;
            }
        }
    }

    public void ResetTimer()
    {
        if (gameEnded) return;
        StartTimer();
    }

    public void ForceNextWave()
    {
        if (gameEnded) return;

        if (isFirstWave)
        {
            SpawnFirstWave();
            isFirstWave = false;
        }
        else
        {
            // При принудительном переходе спавним всех врагов сразу
            SpawnPartialWave();
            SpawnRemainingEnemies();
        }

        if (isTimerRunning)
        {
            StartTimer();
        }
    }

    public bool IsGameEnded()
    {
        return gameEnded;
    }

    public void RestartGame()
    {
        gameEnded = false;
        currentWave = 1;
        isFirstWave = true;
        enemiesSpawnedThisWave = 0;
        currentSpawnIndex = 0;
        spawnTimes.Clear();

        timerSlider.gameObject.SetActive(true);
        timerSlider.value = 1f;

        StartTimer();
    }
}