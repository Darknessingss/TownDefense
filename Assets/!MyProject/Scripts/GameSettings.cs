using UnityEngine;

public class GameSettings : MonoBehaviour

{
    [Header("Game State")]
    public int warriorsCount = 0;
    public int peasantsCount = 0;
    public int enemiesCount = 0;

    void Update()
    {
        UpdateUnitCounts();
    }
    void UpdateUnitCounts()
    {
        warriorsCount = GameObject.FindGameObjectsWithTag("Warrior").Length;
        peasantsCount = GameObject.FindGameObjectsWithTag("Peasant").Length;
        enemiesCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
    }
}