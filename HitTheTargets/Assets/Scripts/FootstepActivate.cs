using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepActivate : MonoBehaviour
{
    Audio audio;
    string currentFloor = "ConcFloor";

    private void Start()
    {

        audio = GameObject.FindWithTag("Player").GetComponent<Audio>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ConcFloor") | other.CompareTag("WoodFloor") | other.CompareTag("TileFloor") | other.CompareTag("MetalFloor") | other.CompareTag("CarpetFloor"))
        {
            currentFloor = other.name;
        }
    }

    public void Step()
    {
        audio.Step(currentFloor);
    }
}
