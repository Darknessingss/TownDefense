using UnityEngine;
using System.Collections;

public class WarriorMovement : MonoBehaviour
{
    public int maxHealth = 300;
    public int currentHealth;

    public GameObject bottleBalanceObject;
    private BottleBalance bottleBalance;
    private Coroutine IntervalProductMinus;

    public float BottleUserTime = 60f;

    void Start()
    {
        currentHealth = maxHealth;

        if (bottleBalanceObject != null)
        {
            bottleBalance = bottleBalanceObject.GetComponent<BottleBalance>();
        }

        if (bottleBalance == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                bottleBalance = player.GetComponent<BottleBalance>();
                if (bottleBalance != null)
                {
                    bottleBalanceObject = player;
                }
            }
        }
        bottleBalance = FindObjectOfType<BottleBalance>();

        if (bottleBalance == null)
        {
            Debug.LogError("BottleBalance not found!");
        }
        else
        {
            IntervalProductMinus = StartCoroutine(ProduceBottlesRoutine());
            Debug.Log($"BottleBalance found on: {bottleBalance.gameObject.name}");
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }

    IEnumerator ProduceBottlesRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(BottleUserTime);
            if (bottleBalance != null)
            {
                bottleBalance.MinusBottle(1);
            }
        }
    }

    void OnDestroy()
    {
        if (IntervalProductMinus != null)
        {
            StopCoroutine(IntervalProductMinus);
        }
    }
}