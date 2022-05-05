using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioMusic : MonoBehaviour
{
    [Header("Music")]
    AudioSource currentMusic;
    public AudioClip mainMenu, song1, song2, song3;

    void Start()
    {

        currentMusic = gameObject.transform.GetChild(0).GetComponent<AudioSource>();

        int chosenSong = Random.Range(0, 2);
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            chosenSong = 3;
        }
        switch (chosenSong)
        {
            case 0:
                currentMusic.clip = song1;
                break;
            case 1:
                currentMusic.clip = song2;
                break;
            case 2:
                currentMusic.clip = song3;
                break;
            case 3:
                currentMusic.clip = mainMenu;
                break;
            default:
                currentMusic.clip = song1;
                break;
        }

        currentMusic.Play();
    }

}
