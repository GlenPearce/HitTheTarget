using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepActivate : MonoBehaviour
{
    new Audio audio;

    private void Start()
    {
        audio = GameObject.FindWithTag("Player").GetComponent<Audio>();
    }

    public void Step()
    {
        audio.Step();
    }
}
