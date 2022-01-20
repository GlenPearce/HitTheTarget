using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Killbox : MonoBehaviour
{
    GameObject player;
    bool fullAlpha = false;
    public CanvasGroup killFade;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine("KillReset");
        }
    }

    IEnumerator KillReset()
    {
        for (int i = 0; i <= 19; i++){
            if (fullAlpha)
            {
                Debug.Log("fade out");
                yield return new WaitForSeconds(0.05f);
                killFade.alpha -= 0.1f;
            }
            else
            {
                Debug.Log("fade in");
                yield return new WaitForSeconds(0.05f);
                killFade.alpha += 0.1f;
            }

            if (killFade.alpha >= 1)
            {
                fullAlpha = true;
                player.transform.position = new Vector3(0,0,0);
            }
        }
        fullAlpha = false;
    }
}
