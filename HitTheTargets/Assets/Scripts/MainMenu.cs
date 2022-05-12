using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public class MainMenu : MonoBehaviour
{
    //Menus UI script + content including unlocks and high scores
    [Header("MenuUI")]
    public GameObject MainUI;
    public GameObject PlayOptions;
    public GameObject OptionUI;
    public GameObject ShiftsUI;
    public GameObject unlocksUI;
    public GameObject HowtoPlayUI;

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
    public Text lockReason;
    public Text lockReasonWep;
    public GameObject startBtn;
    public GameObject shiftBtn;
    public GameObject gravBtn;
    public GameObject speedBtn;
    public GameObject sizeBtn;
    public GameObject dashBtn;

    [Header("Audio UI")]
    public Slider fxSlide;
    public Slider musicSlide;
    public AudioMixer musicVol;
    public AudioMixer fxVol;

    [Header("Other Options")]
    public Slider mouseSens;
    public Playermov player;
    int graphicsQuality;
    

    int wep = 1, level = 1;
    private void Start()
    {
        //Mouse sens default + set to player if in scene
        if (PlayerPrefs.GetFloat("MouseSens") == 0)
        {
            PlayerPrefs.SetFloat("MouseSens", 1);
        }
        mouseSens.value = PlayerPrefs.GetFloat("MouseSens");

        //Sets the default to 0 and if float is anything other, sets it to that
        if (PlayerPrefs.GetFloat("MusicVol") != 0)
        {
            musicSlide.value = PlayerPrefs.GetFloat("MusicVol");
            musicVol.SetFloat("MusicVol", musicSlide.value);
        }
        if (PlayerPrefs.GetFloat("FxVol") != 0)
        {
            fxSlide.value = PlayerPrefs.GetFloat("FxVol");
            fxVol.SetFloat("FxVol", fxSlide.value);
        }

        //Graphics quality default
        QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("GQual"));

        Debug.Log(PlayerPrefs.GetFloat("MouseSens"));

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

            goldTxt.text = goldScore.ToString();
            silverTxt.text = silverScore.ToString();
            bronzeTxt.text = bronzeScore.ToString();

            //Checks for shift unlocks
            if (PlayerPrefs.GetInt("dash") == 0 & PlayerPrefs.GetInt("grav") == 0 & PlayerPrefs.GetInt("speed") == 0 & PlayerPrefs.GetInt("size") == 0)
            {
                shiftBtn.SetActive(false);
            }
            if (PlayerPrefs.GetInt("dash") == 1)
            {
                dashBtn.SetActive(true);
            }
            if (PlayerPrefs.GetInt("grav") == 1)
            {
                gravBtn.SetActive(true);
            }
            if (PlayerPrefs.GetInt("speed") == 1)
            {
                speedBtn.SetActive(true);
            }
            if (PlayerPrefs.GetInt("size") == 1)
            {
                sizeBtn.SetActive(true);
            }

        }


    }

    public void Play()
    {
        MainUI.SetActive(false);
        PlayOptions.SetActive(true);
    }

    public void Options()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            MainUI.SetActive(false);
        }
        OptionUI.SetActive(true);
    }

    public void HowtoPlay()
    {
        MainUI.SetActive(false);
        HowtoPlayUI.SetActive(true);
    }

    public void HowtoPlayBack()
    {
        MainUI.SetActive(true);
        HowtoPlayUI.SetActive(false);
    }

    public void OptionBack()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            MainUI.SetActive(true);
        }
        
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
        wep = j;

        if (wep == 2 & hs1 < silverScore)
        {
            startBtn.SetActive(false);
            lockReasonWep.text = "Haven't unlocked M4!";
        }
        else if (wep == 3 & hs2 < goldScore)
        {
            startBtn.SetActive(false);
            lockReasonWep.text = "Haven't unlocked Railgun!";
        }
        else
        {
            lockReasonWep.text = "";
        }
        startCheck();


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

    //shows the trophy earned, plus disables play button and shows reason if something hasn't been unlocked
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

            lockReason.text = "";
           
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

            if (hs1 == 0)
            {
                startBtn.SetActive(false);
                lockReason.text = "Have not unlocked level 2!";
            }
            else
            {
                lockReason.text = "";
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

            if (hs2 == 0)
            {
                startBtn.SetActive(false);
                lockReason.text = "Have not unlocked level 3!";
            }
            else
            {
                lockReason.text = "";
            }
        }
        startCheck();

    }

    //Checks there is no loked options chosen
    public void startCheck()
    {
        if (lockReasonWep.text == "" & lockReason.text == "")
        {
            startBtn.SetActive(true);
        }
    }

    //is called when a shift orb is collected in game
    public void unlockShift(string shift)
    {
        PlayerPrefs.SetInt(shift, 1);
    }


    //Options Menu
    public void FxSlider()
    {
        fxVol.SetFloat("FxVol", fxSlide.value);
        PlayerPrefs.SetFloat("FxVol", fxSlide.value);
    }
    public void MusicSlider()
    {
        musicVol.SetFloat("MusicVol", musicSlide.value);
        PlayerPrefs.SetFloat("MusicVol", musicSlide.value);
    }
    public void GQuality(int i)
    {
        QualitySettings.SetQualityLevel(i);
        PlayerPrefs.SetInt("GQual", i);
    }

    public void MouseSens()
    {
        PlayerPrefs.SetFloat("MouseSens", mouseSens.value);
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            player.MouseSensUpdate();
        }
    }
}
