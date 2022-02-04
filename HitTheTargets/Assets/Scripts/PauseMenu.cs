using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GamePaused = false;
    public GameObject pauseMenuUI;

    Playermov playermov;

    void Start()
    {
        playermov = GameObject.FindWithTag("Player").GetComponent<Playermov>();
        
    }

    // Update is called once per frame
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
        playermov.moveEnable = true;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Pause()
    {
        playermov.moveEnable = false;
        Cursor.lockState = CursorLockMode.None;
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GamePaused = true;
    }

    public void Restart(int i)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(i);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
