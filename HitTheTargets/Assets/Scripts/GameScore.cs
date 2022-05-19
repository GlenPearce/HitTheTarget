using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


//Script for game score and game start and end
//Also contains high scores and unlockables

public class GameScore : MonoBehaviour
{
    int targetAmnt, milliS, second, minute, levelScore;
    bool stopTime = false;
    string levelScoreStr;
    Rigidbody playerRB;
    Gun gun;

    public PauseMenu pausemenu;
    public Text targetCount, timer, levelScoreTxt, startCountdownTxt, highscoreTxt, hitPercentTxt;
    public Playermov playermov;
    public CanvasGroup finishCanGroup, killFade;
    CanvasGroup hudCanGrp;
    public GameObject finishCan, HudCan, newHigh;
    public Leaderboard leaderboard;
    int totalTargets;




    void Start()
    {
        gun = GameObject.FindWithTag("Player").GetComponent<Gun>();
        hudCanGrp = HudCan.GetComponent<CanvasGroup>();
        stopTime = true;
        playermov.moveEnable = false;

        targetAmnt = GameObject.FindGameObjectsWithTag("Target").Length;
        totalTargets = targetAmnt;
        targetCount.text = "Targets to get: " + targetAmnt;

        StartCoroutine("Countdown");
        playerRB = GameObject.FindWithTag("Player").GetComponent<Rigidbody>();
        playerRB.isKinematic = true;
    }

    IEnumerator Countdown()
    {
        for (int i = 5; i >= 1; i--)
        {
            Debug.Log(i);
            startCountdownTxt.text = (i - 2).ToString();
            if (i == 2)
            {
                startCountdownTxt.text = "GO!";
                
                pausemenu.escEnable = true;
            }
            if (i == 1)
            {
                GameObject.FindWithTag("Player").transform.rotation = Quaternion.Euler(0, 0, 0);
                startCountdownTxt.text = "";
                stopTime = false;
                playermov.moveEnable = true;
                playerRB.isKinematic = false;
            }
            yield return new WaitForSeconds(1);
        }
        yield return null;
    }

    private void Update()
    {
        //Code for timer

        if (stopTime == false)
        {
            milliS += (int)(Time.deltaTime * 1000f) % 1000;

            if (milliS >= 1000)
            {
                milliS = 0;
                second++;
            }
            if (second == 60)
            {
                second = 0;
                minute++;

                //If reaching max time, stop the timer
                if (minute == 99 & second == 99 & milliS == 99)
                {
                    stopTime = true;
                }
            }
            timer.text = minute.ToString("00") + ":" + second.ToString("00") + ":" + milliS.ToString("000");
        }

    }

   
    public void TargetHit()
    {
        targetAmnt--;
        targetCount.text = "Targets to get: " + targetAmnt;
        if (targetAmnt == 0)
        {
            Finish();
        }
    }

    void Finish()
    {
        //Hit Percentage Calculation
        string formatHit = "00.00";
        float hitPercent = (float)totalTargets / (float)gun.shotsTaken * 100;
        hitPercentTxt.text = "Your hit rate was " + hitPercent.ToString(formatHit) + "%!";


        //Time/Score Calculation
        string format = "00:00:000";


        levelScore = int.Parse(minute.ToString("00") + second.ToString("00") + milliS.ToString("000"));
        levelScoreStr = levelScore.ToString(format);

        levelScoreTxt.text = levelScoreStr;

        int currentLevel = SceneManager.GetActiveScene().buildIndex;

        //Sets high score
        if (levelScore < PlayerPrefs.GetInt("level1Score") & currentLevel == 1) 
        { 
            PlayerPrefs.SetInt("level1Score", levelScore);
            highscoreTxt.text = levelScore.ToString(format);
            newHigh.SetActive(true);
            leaderboard.SubmitScore(levelScore);
        }
        else if (levelScore < PlayerPrefs.GetInt("level2Score") & currentLevel == 2)
        {
            PlayerPrefs.SetInt("level2Score", levelScore);
            highscoreTxt.text = levelScore.ToString(format);
            newHigh.SetActive(true);
            leaderboard.SubmitScore(levelScore);
        }
        else if (levelScore < PlayerPrefs.GetInt("level3Score") & currentLevel == 3)
        {
            PlayerPrefs.SetInt("level3Score", levelScore);
            highscoreTxt.text = levelScore.ToString(format);
            newHigh.SetActive(true);
            leaderboard.SubmitScore(levelScore);
        }
        else
        {
            highscoreTxt.text = PlayerPrefs.GetInt("level" + currentLevel + "Score").ToString(format);
        }


        //disable player movement + pause menu
        playermov.moveEnable = false;
        pausemenu.escEnable = false;

        //Fade in Finish canvas
        finishCan.SetActive(true);
        HudCan.SetActive(false);
        finishCanGroup.alpha = 0;
        StartCoroutine("FinishFade");

    }

    IEnumerator FinishFade()
    {
        for (float j = 1; j <= 10; j++)
        {
            hudCanGrp.alpha -= 0.1f;
            finishCanGroup.alpha += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
        Cursor.lockState = CursorLockMode.None;
    }

}
