using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Script for audio triggers for ambience
//Gun audio is in the gun script
//Colliders are on Audio GameObject

public class Audio : MonoBehaviour
{
    [Header("AtmosSounds")]
    public AudioSource exterior;
    public AudioSource interior;
    public float transitionRate;

    [Header("FootstepSounds")]
    public AudioClip metalStep;
    public AudioClip woodStep;
    public AudioClip tileStep1;
    //public AudioClip tileStep2;
    public AudioClip exteriorStep1;
    //public AudioClip exteriorStep2;
    public AudioClip carpetStep1;
    //public AudioClip carpetStep2;
    AudioClip currentClip;

    AudioSource footstep;
    string currentFloor;

    private void Start()
    {
        currentFloor = "ConcFloor";
        footstep = gameObject.GetComponent<AudioSource>();
        interior.volume = 0;
        exterior.volume = 1;

    }
    //Using exit to ensure the last collider left is the one to start the audio
    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Exterior")
        {
            StopAllCoroutines();
            StartCoroutine(Transition("Exit"));
        }
        else if (other.name == "Interior")
        {
            StopAllCoroutines();
            StartCoroutine(Transition("Enter"));
        }
    }

    //Transition between inside and otuside atmos sounds
    IEnumerator Transition(string location)
    {
        if (location == "Enter")
        {
            while (exterior.volume >= 0)
            {
                exterior.volume -= transitionRate;
                interior.volume += transitionRate;
                yield return new WaitForSeconds(0.1f);
            }
            exterior.volume = 0;
            interior.volume = 1;
        }

        if (location == "Exit")
        {
            while (interior.volume >= 0)
            {
                interior.volume -= transitionRate;
                exterior.volume += transitionRate;
                yield return new WaitForSeconds(0.1f);
            }
            interior.volume = 0;
            exterior.volume = 1;
        }

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ConcFloor") | other.CompareTag("WoodFloor") | other.CompareTag("TileFloor") | other.CompareTag("MetalFloor") | other.CompareTag("CarpetFloor"))
        {
            currentFloor = other.name;
        }
        Debug.Log(currentFloor);
    }

    public void Step()
    {
        if (currentFloor == "ConcFloor")
        {
            currentClip = exteriorStep1;
        }
        else if (currentFloor == "WoodFloor")
        {
            currentClip = woodStep;
        }
        else if (currentFloor == "TileFloor")
        {
            currentClip = tileStep1;
        }
        else if (currentFloor == "MetalFloor")
        {
            currentClip = metalStep;
        }
        else if (currentFloor == "CarpetFloor")
        {
            currentClip = carpetStep1;
        }
        footstep.PlayOneShot(currentClip);
    }

}

