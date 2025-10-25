using Unity.VisualScripting;
using UnityEngine;

public class ChestCollecter : MonoBehaviour
{
    [SerializeField] private BottleBalance bottleBalance;
    [SerializeField] private int BottleRewards = 5;
    [SerializeField] private int MinReward = 2;
    [SerializeField] private int MaxReward = 9;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            int randomReward = Random.Range(MaxReward, MinReward);
            bottleBalance.AddBottle(randomReward);
            Destroy(this.gameObject);
        }
    }
}
