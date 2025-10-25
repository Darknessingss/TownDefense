using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.UIElements;
using UnityEditor.IMGUI.Controls;


public class BottleBalance : MonoBehaviour
{
    [SerializeField] private TMP_Text balanceText;

    public int BottleWallet = 0;

    [SerializeField] private float respawnBottle = 10f;
    public int WarriorCost = 5;
    [SerializeField] private TMP_Text WarriorText;
    public int PeasantCost = 2;
    [SerializeField] private TMP_Text PeasantText;



    private void Start()
    {
        UpdateBottleUI();
    }

    private void Update()
    {
        balanceText.text = $"{BottleWallet}";
    }

    public void AddBottle(int amount)
    {
        BottleWallet += amount;
        UpdateBottleUI();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (CompareTag("BottleFull"))
        {
            BottleWallet++;
            UpdateBottleUI();
        }
        Debug.Log("Бутылки были собраны");
    }

    public void UpdateBottleUI()
    {
        balanceText.text = $"{BottleWallet}";
    }
}
