using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Shifts : MonoBehaviour
{
    //contains the code for intiating selected modifiers at the start of a game + buttons for the menu choice

    string grav, plSpeed, plSize, dash;
    Playermov player;
    public Text gravTxt, speedTxt, sizeTxt, dashTxt;

    private void Start()
    {
        //loads the player prefs of chosen modifiers
        grav = PlayerPrefs.GetString("ShiftGrav");
        plSpeed = PlayerPrefs.GetString("ShiftSpeed");
        plSize = PlayerPrefs.GetString("ShiftSize");
        dash = PlayerPrefs.GetString("ShiftDash");

        //initiates modifiers at start of scene, as long as it isn't the main menu
        if (SceneManager.GetActiveScene().buildIndex > 0)
        {
            player = GameObject.FindWithTag("Player").GetComponent<Playermov>();

            //Gravity
            if (grav == "low")
            {
                Physics.gravity = new Vector3(0, -5.0F, 0);
            }
            else if (grav == "high")
            {
                Physics.gravity = new Vector3(0, -30.0F, 0);
            }
            //Player speed
            if (plSpeed == "fast")
            {
                player.speed = 200;
                player.maxSpeed = 10;
            }

            //Player size
            {
                if(plSize == "small")
                {
                    GameObject.FindWithTag("Player").transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
                }
                else if (plSize == "big")
                {
                    GameObject.FindWithTag("Player").transform.localScale = new Vector3(1.8f, 1.8f, 1.8f);
                }
            }

            //dash cooldown
            {
                if(dash == "none")
                {
                    player.dashCooldown = 0.01f;
                }
            }

        }

        else
        {
            //sets to default if no prefs are found
            if (PlayerPrefs.GetString("ShiftGrav") == "")
            {
                Defaults();
            }
            else
            {
                UpdateNames();
            }

        }


    }

    //Button to activate and deactivate shifts
    public void Modifiers(int i)
    {
        //low Grav
        if (i == 1)
        {
            if (grav == "normal")
            {
                grav = "low";
            }
            else if (grav == "low")
            {
                grav = "high";
            }
            else
            {
                grav = "normal";
            }
            UpdateNames();
            PlayerPrefs.SetString("ShiftGrav", grav);
        }
        //player speed
        if (i == 2)
        {
            if (plSpeed == "normal")
            {
                plSpeed = "fast";
            }
            else
            {
                plSpeed = "normal";
            }
            UpdateNames();
            PlayerPrefs.SetString("ShiftSpeed", plSpeed);
        }
        //player size
        if (i == 3)
        {
            if (plSize == "normal")
            {
                plSize = "small";
            }
            else if (plSize == "small")
            {
                plSize = "big";
            }
            else
            {
                plSize = "normal";
            }
            UpdateNames();
            PlayerPrefs.SetString("ShiftSize", plSize);
        }
        //dash cooldown
        if (i == 4)
        {
            if (dash == "normal")
            {
                dash = "none";
            }
            else
            {
                dash = "normal";
            }
            UpdateNames();
            PlayerPrefs.SetString("ShiftDash", dash);
        }
    }
    /*list of modifiers
    1 = high/low grav
    2 = fast speed
    3 = player size
    4 = dash cooldown
    */

    public void Defaults()
    {
        grav = "normal";
        plSpeed = "normal";
        plSize = "normal";
        dash = "normal";
        UpdateNames();
    }
    public void UpdateNames()
    {
        gravTxt.text = "Gravity = " + grav;
        speedTxt.text = "Player speed = " + plSpeed;
        sizeTxt.text = "Player size = " + plSize;
        dashTxt.text = "Dash cooldown = " + dash;
    }

}
