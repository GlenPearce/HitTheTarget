using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    //Menus UI script + content including unlocks and high scores
    [Header("MenuUI")]
    public GameObject MainUI;
    public GameObject PlayOptions;
    public GameObject OptionUI;
    public GameObject ShiftsUI;
    public GameObject unlocksUI;

    [Header("UnlockUI")]
    float hs1, hs2, hs3;
    public Sprite gold;
    public Sprite bronze;
    public Sprite silver;
    public GameObject HSTrophy;
    public Text goldTxt;
    public Text silverTxt;
    public Text bronzeTxt;
    public Text highScore1;
    public Text highScore2;
    public Text highScore3;
    public GameObject level1;
    public GameObject level2;
    public GameObject level3;
    public float goldScore;
    public float silverScore;
    public float bronzeScore;


    int wep = 1, level = 1;
    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            PlayerPrefs.SetInt("SelectedWeapon", wep);

            //Initilise trophies and score
            hs1 = PlayerPrefs.GetInt("level1Score");
            hs2 = PlayerPrefs.GetInt("level2Score");
            hs3 = PlayerPrefs.GetInt("level3Score");
            highScore1.text = hs1.ToString();
            highScore2.text = hs2.ToString();
            highScore3.text = hs3.ToString();
            ShowTrophy();
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

    //Level select, also changes the level display and trophy goals
    public void LevelSelect(int i)
    {
        i += 1;
        level = i;

        if ( i == 1)
        {
            level1.SetActive(true);
            level2.SetActive(false);
            level3.SetActive(false);

        }
        if (i == 2)
        {
            level1.SetActive(false);
            level2.SetActive(true);
            level3.SetActive(false);

        }
        if (i == 3)
        {
            level1.SetActive(false);
            level2.SetActive(false);
            level3.SetActive(true);

        }
        ShowTrophy();
    }

    //Chosen weapon 1 = pistol, 2 = M6, 3 = Railgun
    public void WeaponSelect(int j)
    {
        j += 1;
        //string wepName = "Pistol";
        wep = j;

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
    public void Unlocks(bool open)
    {
        if (open)
        {
            unlocksUI.SetActive(true);
        }
        else
        {
            unlocksUI.SetActive(false);
        }
    }

    public void ShowTrophy()
    {
        if (level == 1)
        {
            HSTrophy.SetActive(true);
            if (hs1 >= goldScore)
            {
                HSTrophy.GetComponent<Image>().sprite = gold;
            }
            else if (hs1 >= silverScore)
            {
                HSTrophy.GetComponent<Image>().sprite = silver;
            }
            else if (hs1 >= bronzeScore)
            {
                HSTrophy.GetComponent<Image>().sprite = bronze;
            }
            else
            {
                HSTrophy.SetActive(false);
            }

        }
        if (level == 2)
        {
            HSTrophy.SetActive(true);
            if (hs2 >= goldScore)
            {
                HSTrophy.GetComponent<Image>().sprite = gold;
            }
            else if (hs2 >= silverScore)
            {
                HSTrophy.GetComponent<Image>().sprite = silver;
            }
            else if (hs2 >= bronzeScore)
            {
                HSTrophy.GetComponent<Image>().sprite = bronze;
            }
            else
            {
                HSTrophy.SetActive(false);
            }

        }
        if (level == 3)
        {
            HSTrophy.SetActive(true);
            if (hs3 >= goldScore)
            {
                HSTrophy.GetComponent<Image>().sprite = gold;
            }
            else if (hs3 >= silverScore)
            {
                HSTrophy.GetComponent<Image>().sprite = silver;
            }
            else if (hs3 >= bronzeScore)
            {
                HSTrophy.GetComponent<Image>().sprite = bronze;
            }
            else
            {
                HSTrophy.SetActive(false);
            }

        }

    }

}
