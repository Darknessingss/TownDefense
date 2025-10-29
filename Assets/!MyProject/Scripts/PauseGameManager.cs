using System.Collections.Generic;
using TMPro.Examples;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseGameManager : MonoBehaviour
{
    private bool isPaused = false;

    public MonoBehaviour PlayerMovement;

    [SerializeField] private GameObject PauseMenu;

    void Start()
    {
        if (PauseMenu != null)
        {
            PauseMenu.SetActive(false);
        }
        Time.timeScale = 1;
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            if (PauseMenu != null)
            {
                PauseMenu.SetActive(true);
                PlayerMovement.enabled = false;
            }
            Time.timeScale = 0;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Resume();
        }
    }

    public void Resume()
    {
        isPaused = false;

        if (PauseMenu != null)
        {
            PauseMenu.SetActive(false);
            PlayerMovement.enabled = true;
        }
        Time.timeScale = 1;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Exit()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MenuScene");
    }
}