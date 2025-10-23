using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
public class TestScriptsUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TextMeshProUGUI buttonText;
    [SerializeField] private string basetext = "Start";
    [SerializeField] private string mystery = "<3/s';_=!#$@";
    [SerializeField] private int textLength = 4;
    [SerializeField] private float changeInterval = 0.2f;
    
    private bool MysteryText = true;
    private Coroutine animationCoroutine;

    void Start() => StartMysteryAnimation();

    void StartMysteryAnimation()
    {
        animationCoroutine = StartCoroutine(MysteryAnimation());
    }

    IEnumerator MysteryAnimation()
    {
        while (MysteryText)
        {
            buttonText.text = GenerateRandomMysteryText();
            yield return new WaitForSeconds(changeInterval);
        }
    }

    string GenerateRandomMysteryText()
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        for (int i = 0; i < textLength; i++)
        {
            sb.Append(mystery[Random.Range(0, mystery.Length)]);
        }
        return sb.ToString();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        MysteryText = false;
        buttonText.text = basetext;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        MysteryText = true;
        StartMysteryAnimation();
    }

    void OnDisable()
    {
        StopCoroutine(animationCoroutine);
    }
}