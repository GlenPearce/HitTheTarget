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
    public AudioSource metalStep;
    public AudioSource woodStep;
    public AudioSource tileStep1;
    public AudioSource tileStep2;
    public AudioSource exteriorStep1;
    public AudioSource exteriorStep2;
    public AudioSource carpetStep1;
    public AudioSource carpetStep2;

    private void Start()
    {
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

}

