using UnityEngine;
using System.Collections.Generic;

public class BuyerButtons : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject WarriorPrefab;
    public GameObject PeasantPrefab;

    [Header("Spawn Points")]
    public Transform WarriorSpawn;
    public Transform PeasantSpawn;

    [Header("Spawn Area")]
    public BoxCollider spawnWarrior;
    public BoxCollider spawnPeasant;

    [Header("Limits")]
    public int peasantLimit = 8;

    private List<GameObject> spawnedPeasants = new List<GameObject>();
    private List<GameObject> spawnedWarriors = new List<GameObject>();

    private void Update()
    {
        CheckAliveUnits();
    }

    public void OnWarriorButtonClick()
    {
        SpawnUnit(WarriorPrefab, WarriorSpawn, spawnWarrior, false);
    }

    public void OnPeasantButtonClick()
    {
        if (spawnedPeasants.Count < peasantLimit)
        {
            SpawnUnit(PeasantPrefab, PeasantSpawn, spawnPeasant, true);
        }
        else
        {
            Debug.Log($"Лимит крестьян достигнут! Максимум: {peasantLimit}");
        }
    }

    private void SpawnUnit(GameObject unitPrefab, Transform spawnPoint, BoxCollider spawnArea, bool isPeasant)
    {
        if (unitPrefab != null && spawnPoint != null && spawnArea != null)
        {
            Vector3 spawnPosition = GetRandomPositionInBox(spawnArea);
            
            GameObject newUnit = Instantiate(unitPrefab, spawnPosition, spawnPoint.rotation);
            
            if (isPeasant)
            {
                spawnedPeasants.Add(newUnit);
                Debug.Log($"Spawned Peasant. Total: {spawnedPeasants.Count}/{peasantLimit}");
            }
            else
            {
                spawnedWarriors.Add(newUnit);
            }
            
            Debug.Log($"Spawned {unitPrefab.name} at {spawnPosition}");
        }
        else
        {
            Debug.LogError("Missing references for spawning unit!");
        }
    }

    private Vector3 GetRandomPositionInBox(BoxCollider boxCollider)
    {
        Vector3 center = boxCollider.transform.TransformPoint(boxCollider.center);
        Vector3 size = boxCollider.size;
        
        size.x *= boxCollider.transform.lossyScale.x;
        size.y *= boxCollider.transform.lossyScale.y;
        size.z *= boxCollider.transform.lossyScale.z;
        
        float randomX = Random.Range(-size.x / 2, size.x / 2);
        float randomZ = Random.Range(-size.z / 2, size.z / 2);
        
        Vector3 randomPosition = center + new Vector3(randomX, 0, randomZ);
        randomPosition.y = center.y;
        
        return randomPosition;
    }

    private void CheckAliveUnits()
    {
        for (int i = spawnedPeasants.Count - 1; i >= 0; i--)
        {
            if (spawnedPeasants[i] == null || !spawnedPeasants[i].activeInHierarchy)
            {
                spawnedPeasants.RemoveAt(i);
                Debug.Log("Мертвый крестьянин удален из списка");
            }
        }

        for (int i = spawnedWarriors.Count - 1; i >= 0; i--)
        {
            if (spawnedWarriors[i] == null || !spawnedWarriors[i].activeInHierarchy)
            {
                spawnedWarriors.RemoveAt(i);
            }
        }
    }

    public void ForceCleanup()
    {
        CheckAliveUnits();
    }

    public int GetCurrentPeasantCount()
    {
        return spawnedPeasants.Count;
    }

    public int GetPeasantLimit()
    {
        return peasantLimit;
    }

    public void SetPeasantLimit(int newLimit)
    {
        peasantLimit = newLimit;
    }
}