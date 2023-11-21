using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menus : MonoBehaviour
{
    [Header("All Menu's")]
    public GameObject pauseMenuUI;
    public GameObject EndGameMenuUI;
    public static bool GameIsStopped = false;


    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(GameIsStopped)
            {
                Resume();
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                Pause();
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        GameIsStopped = false;
    }

    public void Restart()
    {
        SceneManager.LoadScene("TDMRoom");
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game....");
        Application.Quit();
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsStopped = true;
    }
}
