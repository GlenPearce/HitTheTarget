using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject MainUI;
    public GameObject PlayOptions;
    //public GameObject PauseUI;

    // Start is called before the first frame update
    void Play()
    {
        MainUI.SetActive(false);
        PlayOptions.SetActive(true);
    }

    void Back()
    {
        MainUI.SetActive(true);
        PlayOptions.SetActive(false);
    }

    void Menu()
    {
        SceneManager.LoadScene(0);
    }

    void PlayLvl1()
    {
        SceneManager.LoadScene(1);
    }

    void PlayLvl2()
    {
        SceneManager.LoadScene(2);
    }

    void PlayLvl3()
    {
        SceneManager.LoadScene(3);
    }
}
