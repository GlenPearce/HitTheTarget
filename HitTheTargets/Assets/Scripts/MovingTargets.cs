using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTargets : MonoBehaviour
{
    [Header("move between points")]
    public Transform spotOne, spotTwo;
    public float moveSpeed;
    [Header("is this a moving target?")]
    public bool ismoving;
    private float interpolation;
    private bool direction;

    private void Start()
    {
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
        //ADD SOME SCORE HERE TO FOR GAME HANDLER

        Destroy(spotOne.gameObject);
        Destroy(spotTwo.gameObject);
        Destroy(transform.parent.gameObject);
        Destroy(gameObject);
    }


}
