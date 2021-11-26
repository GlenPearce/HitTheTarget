using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameScore : MonoBehaviour
{
    int targetAmnt, milliS, second, minute;
    float time;
    bool stopTime = false;

    public Text targetCount, timer;

    void Start()
    {
        targetAmnt = GameObject.FindGameObjectsWithTag("Target").Length;
        time = 0;
        targetCount.text = "Targets: 0/" + targetAmnt;
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

    // Update is called once per frame
    public void TargetHit()
    {
        targetAmnt--;
        targetCount.text = "Targets: " + targetAmnt + "/" + targetAmnt;
        if (targetAmnt == 0)
        {
            //Code in finish
        }
    }
}
