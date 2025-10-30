using System;
using UnityEngine;

public class GameSettings : MonoBehaviour

{
    [Header("Game State")]
    public int warriorsCount = 0;
    public int peasantsCount = 0;
    private int _enemiesCount = 0;

    public static event Action<int> EnemiesCountChanged;

    public int EnemiesCount
    {
        get => _enemiesCount;
        private set
        {
            _enemiesCount = value;
            EnemiesCountChanged?.Invoke(EnemiesCount);
        }
    }

    void Update()
    {
        UpdateUnitCounts();
    }
    void UpdateUnitCounts()
    {
        warriorsCount = GameObject.FindGameObjectsWithTag("Warrior").Length;
        peasantsCount = GameObject.FindGameObjectsWithTag("Peasant").Length;
        EnemiesCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
    }
}