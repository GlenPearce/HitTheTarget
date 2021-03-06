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
    Transform player;


    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        gameScore = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameScore>();
        ping = gameObject.transform.GetChild(0).gameObject.GetComponent<Canvas>();
        mainCam = Camera.main;
        ping.worldCamera = mainCam;
    }

    void Update()
    {
        var lookPos = player.position - transform.position;
        lookPos.y = 10000;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.05f);

        interpolation = moveSpeed * Time.deltaTime;

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
        float dist = Vector3.Distance(gameObject.transform.position, mainCam.transform.position);

        //shrinks the pinp on distance
        if (dist < 50)
        {
            ping.transform.localScale = new Vector3(1*(dist/30),1 * (dist / 30),1 * (dist / 30));
        }
        //hides the ping if too far
        else
        {
            ping.transform.localScale = new Vector3(0,0,0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {

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
