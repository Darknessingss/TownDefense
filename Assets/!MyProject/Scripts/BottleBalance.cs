using System;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Random = UnityEngine.Random;

public class BottleBalance : MonoBehaviour
{
    [Header("Balance")]
    [SerializeField] private TMP_Text balanceText;
    [SerializeField] private TMP_Text peasantLimitText;
    public int BottleWallet = 0;

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

    [Header("ButtonUI")]
    [SerializeField] private Button PeasantBuyButtonStatus;
    [SerializeField] private Button WarriorBuyButtonStatus;

    public static class DestroyWarriorForBalance
    {
        public static Action<int> ZeroBalanceDestroyWarrior;
    }

    private void Awake()
    {
        DestroyWarriorForBalance.ZeroBalanceDestroyWarrior += BottleBalanceDestroyer;
    }

    private void OnDestroy()
    {
        DestroyWarriorForBalance.ZeroBalanceDestroyWarrior -= BottleBalanceDestroyer;
    }

    private void Start()
    {
        UpdatePeasantLimitUI();
        UpdateUIBuyPeasant();
        UpdateUIBuyWarrior();
    }

    private void BottleBalanceDestroyer(int BottleWallet)
    {
        if(BottleWallet < 0)
        {
            DestroyOnWarrior();
            this.BottleWallet = 0;
            UpdateBottleUI();
            Debug.Log("Баланс ушел в минус! Один Warrior уничтожен, баланс обнулен.");
        }
    }

    private void DestroyOnWarrior()
    {
        GameObject[] warriorDestroy = GameObject.FindGameObjectsWithTag("Warrior");

        if(warriorDestroy.Length > 0)
        {
            GameObject warriorToDestroy = warriorDestroy[0];
            Destroy(warriorToDestroy);

            Debug.Log($"Уничтожен Warrior. Осталось Warriors: {warriorDestroy.Length - 1}");
        }
        else
        {
            Debug.Log("Рыцарь не обнаружен, уничтожаем фермера");
            GameObject[] peasantDestroy = GameObject.FindGameObjectsWithTag("Peasant");
            if(peasantDestroy.Length > 0)
            {
                GameObject peasantToDestroy = peasantDestroy[0];
                Destroy(peasantToDestroy);

                Debug.Log($"Уничтожен Peasant. Осталось Peasant: {peasantDestroy.Length - 1}");
            }    
        }
    }

    public void OnWarriorBuyButtonClick()
    {
        int warriorPrice = 5;
        {
            if (BottleWallet >= 5)
            {
                BottleWallet -= warriorPrice;
                UpdateBottleUI();
                SpawnUnit(WarriorPrefab, WarriorSpawn, spawnWarrior, false);
            }
            if (BottleWallet < 0)
            {
                DestroyWarriorForBalance.ZeroBalanceDestroyWarrior?.Invoke(BottleWallet);
            }
        }
    }

    public void OnPeasantBuyButtonClick()
    {
        int peasantPrice = 2;
        {
            if (BottleWallet >= 2)
            {
                BottleWallet -= peasantPrice;
                UpdateBottleUI();
                if (spawnedPeasants.Count < peasantLimit)
                {
                    SpawnUnit(PeasantPrefab, PeasantSpawn, spawnPeasant, true);
                    UpdatePeasantLimitUI();
                }
                else
                {
                    Debug.Log($"Лимит крестьян достигнут! Максимум: {peasantLimit}");
                }
                if (BottleWallet < 0)
                {
                    DestroyWarriorForBalance.ZeroBalanceDestroyWarrior?.Invoke(BottleWallet);
                }
            }
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

        UpdatePeasantLimitUI();
        UpdateUIBuyPeasant();
        UpdateUIBuyWarrior();
    }

    private void UpdateUIBuyPeasant()
    {
        if (PeasantBuyButtonStatus != null)
        {
            bool canBuy = BottleWallet >= 2 && spawnedPeasants.Count < peasantLimit;
            PeasantBuyButtonStatus.interactable = canBuy;
        }
    }

    private void UpdateUIBuyWarrior()
    {
        if (WarriorBuyButtonStatus != null)
        {
            bool canBuy = BottleWallet >= 5;
            WarriorBuyButtonStatus.interactable = canBuy;
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
        UpdatePeasantLimitUI();
    }

    private void Update()
    {
        CheckAliveUnits();
        balanceText.text = $"{BottleWallet}";
    }

    public void AddBottle(int amount)
    {
        BottleWallet += amount;
        UpdateBottleUI();
    }

    public void MinusBottle(int amount)
    {
        BottleWallet -= amount;
        UpdateBottleUI();

        if (BottleWallet < 0)
        {
            DestroyWarriorForBalance.ZeroBalanceDestroyWarrior?.Invoke(BottleWallet);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("BottleFull"))
        {
            BottleWallet++;
            UpdateBottleUI();
        }
    }

    public void UpdateBottleUI()
    {
        balanceText.text = $"{BottleWallet}";
    }

    public void UpdatePeasantLimitUI()
    {
        if (peasantLimitText != null)
        {
            peasantLimitText.text = $"Limit {spawnedPeasants.Count}/{peasantLimit}";
        }
    }
}