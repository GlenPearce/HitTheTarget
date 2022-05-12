using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShiftOrb : MonoBehaviour
{

    AudioSource pickup;
    Vector3 pos;

    private void Start()
    {
        pos = transform.position;
        pickup = transform.GetChild(0).GetComponent<AudioSource>();
        if (PlayerPrefs.GetInt(gameObject.name) == 1)
        {
            Destroy(gameObject);
        }
        
    }

    public void Update()
    {
        float y = Mathf.PingPong(Time.time * 1, 1) * 0.5f;
        transform.position = new Vector3(pos.x, y + pos.y, pos.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerPrefs.SetInt(gameObject.name, 1);
            pickup.Play();
            Destroy(gameObject, 1);
        }
    }
}
