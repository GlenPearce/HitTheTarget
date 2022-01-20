using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//Script for game score and game start and end

public class GameScore : MonoBehaviour
{
    int targetAmnt, milliS, second, minute, levelScore;
    float time;
    bool stopTime = false;
    string levelScoreStr;

    public Text targetCount, timer, levelScoreTxt, startCountdownTxt;
    public Playermov playermov;
    public CanvasGroup finishCanGroup, killFade;
    public GameObject finishCan, HudCan;

    

    void Start()
    {
        stopTime = true;
        playermov.moveEnable = false;
        targetAmnt = GameObject.FindGameObjectsWithTag("Target").Length;
        time = 0;
        targetCount.text = "Targets to get: " + targetAmnt;
        StartCoroutine("Countdown");
    }

    IEnumerator Countdown()
    {
        for (int i = 5; i >= 0; i--)
        {
            startCountdownTxt.text = (i - 2).ToString();
            if (i == 1)
            {
                startCountdownTxt.text = "GO!";
            }
            if (i == 0)
            {
                startCountdownTxt.text = "";
                stopTime = false;
                playermov.moveEnable = true;
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
            timer.text = minute.ToString() + ":" + second.ToString() + ":" + milliS.ToString("00");
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
        //sets the level score and ui
        levelScoreStr = timer.text = minute.ToString() + second.ToString() + milliS.ToString("00");
        levelScore = int.Parse(levelScoreStr);
        levelScoreTxt.text = "Score: " + timer.text;

        //Sets high score
        if (levelScore >= PlayerPrefs.GetInt("level1Score")){
            PlayerPrefs.SetInt("level1Score", levelScore);
        }

        //disable player movement
        playermov.moveEnable = false;

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
            finishCanGroup.alpha += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
    }

}
