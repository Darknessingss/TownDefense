using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseGameManager : MonoBehaviour
{
    private bool StopGame = false;

    [SerializeField] private GameObject PauseMenu;


    public void Pause()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            PauseMenu.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            Resume();
        }
    }

    public void Resume()
    {
        PauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }

    public void Exit()
    {
        SceneManager.LoadScene("MenuScene");
        Time.timeScale = 1;
    }    
}