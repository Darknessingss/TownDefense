using System.Threading;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Runtime.CompilerServices;

public class SpawnEnemy : MonoBehaviour
{
    public GameObject enemyPrefab;
    public int currentWave = 1;
    public int maxWave = 10;
    public Slider timerSlider;
    public float waveCooldown = 10f;
    public TMP_Text waveCounterText;

    private int enemiesPerWave;
    private float currentTimer;
    private bool isTimerRunning = false;
    private bool isFirstWave = true;

    public int MinEnemy = 1;
    public int MaxEnemy = 6;

    public MonoBehaviour PlayerMovement;
    [SerializeField] private GameObject WinScreen;

    private void Awake()
    {
        GameSettings.EnemiesCountChanged += OnEnemyCountChanged;

    }

    private void OnEnemyCountChanged(int EnemyCount)
    {
       if(EnemyCount <= 0)
        {
            EndGame();
        }
    }

    void Start()
    {
        {
            timerSlider.minValue = 0f;
            timerSlider.maxValue = 1f;
            timerSlider.value = 1f;
        }
        StartTimer();

        if (WinScreen != null)
        {
            WinScreen.SetActive(false);
        }
        Time.timeScale = 1;

        UpdateWaveCounter();
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
        UpdateWaveCounter();
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

        UpdateWaveCounter();
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
        if (currentWave <= maxWave)
            return;

        Debug.Log($"Game Over! Final wave: {currentWave}");


        if (timerSlider != null)
        {
            timerSlider.gameObject.SetActive(false);
        }
        Winer();
      
        
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

        if (timerSlider != null)
        {
            timerSlider.gameObject.SetActive(true);
            timerSlider.value = 1f;
        }

        if (WinScreen != null)
        {
            WinScreen.SetActive(false); 
        }

        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        StartTimer();

        UpdateWaveCounter();
    }

    void Winer()
    {
        if (WinScreen != null)
        {
            WinScreen.SetActive(true);
            PlayerMovement.enabled = false; 
        }
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void OnDestroy()
    {
        GameSettings.EnemiesCountChanged -= OnEnemyCountChanged;
    }

    void UpdateWaveCounter()
    {
        if (waveCounterText != null)
        {
            waveCounterText.text = $"{currentWave - 1}/{maxWave}";
        }
    }
}