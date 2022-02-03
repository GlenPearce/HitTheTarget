using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public static bool GamePaused = false;

    public GameObject pauseMenuUI;
    public GameObject MainUI;
    public GameObject PlayOptions;
    public GameObject OptionUI;

    
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GamePaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GamePaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GamePaused = true;
    }

    public void Play()
    {
        MainUI.SetActive(false);
        PlayOptions.SetActive(true);
    }

    public void Options()
    {
        MainUI.SetActive(false);
        OptionUI.SetActive(true);
    }

    public void OptionBack()
    {
        MainUI.SetActive(true);
        OptionUI.SetActive(false);
    }

    public void Back()
    {
        MainUI.SetActive(true);
        PlayOptions.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Menu()
    {
        SceneManager.LoadScene(0);
    }

    public void PlayLvl1()
    {
        SceneManager.LoadScene(1);
    }

    public void PlayLvl2()
    {
        SceneManager.LoadScene(2);
    }

    public void PlayLvl3()
    {
        SceneManager.LoadScene(3);
    }
}
