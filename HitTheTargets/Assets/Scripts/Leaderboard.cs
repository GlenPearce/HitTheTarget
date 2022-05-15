using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using LootLocker.Requests;


public class Leaderboard : MonoBehaviour
{
    //Online Leaderboard using LootLocker and following their tutorial: https://www.youtube.com/watch?v=pp8Vl4cKLdc

    //Leaderboard
    string memberID;
    public int ID;
    int maxScores = 12;
    public Text[] entries;

    //UI
    public Text selectedLevel;
    int currentLevel;

    private void Start()
    {
        memberID = PlayerPrefs.GetString("PlayerName");

        LootLockerSDKManager.StartGuestSession("Player", (response) =>
        {
            if (response.success)
            {
                Debug.Log("Success");
            }
            else
            {
                Debug.Log("Failed");
            }
        });


        currentLevel = 1;
        
    }

    public void ShowScores()
    {
        LootLockerSDKManager.GetScoreList(ID, maxScores, (response) =>
        {
            if (response.success)
            {
                LootLockerLeaderboardMember[] scores = response.items;
                for (int i = 0; i < scores.Length; i++)
                {
                    entries[i].text = (scores[i].rank + ".   " + scores[i].score);
                }

                if (scores.Length < maxScores)
                {
                    for (int i = scores.Length; i < maxScores; i++)
                    {
                        entries[i].text = (i + 1).ToString() + ".   none";
                    }
                }

            }
            else
            {
                Debug.Log("Failed");
            }
        });
    }

    public void SubmitScore(int score)
    {
        LootLockerSDKManager.SubmitScore(memberID, score, ID, (response) =>
        {
            if (response.success)
            {
                Debug.Log("Success");
            }
            else
            {
                Debug.Log("Failed");
            }
        });
    }

    // Start is called before the first frame update
    public void NxtBtn()
    {
        currentLevel ++;
        if (currentLevel >= 4)
        {
            currentLevel = 1;
        }

        selectedLevel.text = "Level: " + currentLevel;

        //Leaderboard ID Change
        if (currentLevel == 1)
        {
            ID = 3103;
        }
        else if (currentLevel == 2)
        {
            ID = 3104;
        }
        else
        {
            ID = 3105;
        }

        ShowScores();       

    }
}
