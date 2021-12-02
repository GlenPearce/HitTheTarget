using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovingTargets : MonoBehaviour
{
    [Header("move between points")]
    public Transform spotOne, spotTwo;
    public float moveSpeed;
    [Header("is this a moving target?")]
    public bool ismoving;
    private float interpolation;
    private bool direction;

    Canvas ping;
    Camera mainCam;
    GameScore gameScore;


    private void Start()
    {
        gameScore = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameScore>();
        ping = gameObject.transform.GetChild(0).gameObject.GetComponent<Canvas>();
        mainCam = Camera.main;
        ping.worldCamera = mainCam;
    }

    void Update()
    {
        
        interpolation = moveSpeed * Time.deltaTime;
        Debug.Log(direction);
        if (direction == true && ismoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, spotOne.position, interpolation);
        }
        else if(direction == false && ismoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, spotTwo.position, interpolation);
        }

        //Canvas on child of this object always faces player
        ping.transform.LookAt(mainCam.transform.position);

    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        if(other.name == "spotOne")
        {
            direction = false;
        }
        else if (other.name == "spotTwo")
        {
            direction = true;
        }

    }

    public void Die()
    {
        gameScore.TargetHit();

        Destroy(spotOne.gameObject);
        Destroy(spotTwo.gameObject);
        Destroy(transform.parent.gameObject);
        Destroy(gameObject);
    }


}
