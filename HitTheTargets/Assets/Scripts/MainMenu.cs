using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    //Menus UI script
    public GameObject MainUI;
    public GameObject PlayOptions;
    public GameObject OptionUI;
    public GameObject ShiftsUI;

    int wep = 1, level = 1;

    public Text levelTxt, wepTxt;
    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            PlayerPrefs.SetInt("SelectedWeapon", wep);
        }
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

    public void LevelSelect(int i)
    {
        level = i;
        levelTxt.text = "Level: " + i;
    }

    //Chosen weapon 1 = pistol, 2 = M6, 3 = Railgun
    public void WeaponSelect(int j)
    {
        string wepName = "Pistol";
        wep = j;
        if (wep == 1)
        {
            wepName = "Pistol";
        }
        else if (wep == 2)
        {
            wepName = "M4";
        }
        else
        {
            wepName = "Railgun";
        } 
        wepTxt.text = "Weapon: " + wepName;
    }

    public void Go()
    {
        PlayerPrefs.SetInt("SelectedWeapon", wep);
        SceneManager.LoadScene(level);
    }

    public void Shifts(bool open)
    {
        if (open)
        {
            ShiftsUI.SetActive(true);
        }
        else
        {
            ShiftsUI.SetActive(false);
        }
    }
}
