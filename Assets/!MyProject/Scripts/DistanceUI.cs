using UnityEngine;
using UnityEngine.UI;

public class DistanceUI : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject uiPanel;
    
    [Header("Settings")]
    public bool pauseGame = false; 
    public bool showCursor = true; 

    private bool wasCursorVisible;
    private CursorLockMode previousCursorLockState;

    private void Start()
    {
        if (uiPanel != null)
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
        if (uiPanel != null)
        {
            wasCursorVisible = Cursor.visible;
            previousCursorLockState = Cursor.lockState;
            
            uiPanel.SetActive(true);
            
            if (showCursor)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            
            if (pauseGame)
                Time.timeScale = 0f;
        }
        
        Debug.Log("UI показан, курсор включен");
    }

    private void HideUI()
    {
        if (uiPanel != null)
        {
            uiPanel.SetActive(false);
            
            Cursor.visible = wasCursorVisible;
            Cursor.lockState = previousCursorLockState;
            
            if (pauseGame)
                Time.timeScale = 1f;
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