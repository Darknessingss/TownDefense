using UnityEngine;
using UnityEngine.UI;

public class DistanceUI : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject uiPanel;
    
    [Header("Settings")]
    public bool showCursor = true; 

    private bool wasCursorVisible;
    private CursorLockMode previousCursorLockState;

    private void Start()
    {
            uiPanel.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ShowUI();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HideUI();
        }
    }

    private void ShowUI()
    {
        {
            wasCursorVisible = Cursor.visible;
            previousCursorLockState = Cursor.lockState;
            
            uiPanel.SetActive(true);
            
            if (showCursor)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            
        }
        
        Debug.Log("UI показан, курсор включен");
    }

    private void HideUI()
    {
        {
            uiPanel.SetActive(false);
            
            Cursor.visible = wasCursorVisible;
            Cursor.lockState = previousCursorLockState;
        }
        
        Debug.Log("UI скрыт, курсор восстановлен");
    }

    public void CloseUI()
    {
        HideUI();
    }

    private void Update()
    {
        if (uiPanel != null && uiPanel.activeInHierarchy && Input.GetKeyDown(KeyCode.Escape))
        {
            HideUI();
        }
    }
}