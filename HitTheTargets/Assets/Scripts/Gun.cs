using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class Gun : MonoBehaviour
{
    [Header("Gun Stats")]
    public float fireRate = 1f;
    public int gunRange;
    public float lineDuration;
    public int ammoMax;

    [Header("Initalise variables")]
    public Camera camera;
    public Transform gunTip;

    private float nextFire;
    private int currentAmmo;
    private bool shooting;
    private LineRenderer shotLine;
    Playermov playerMov;

    GameObject[] ammoCountTxt;
    public Slider ammoSlide;

    public Animator m_animator;

    void Start()
    {
        //initalise things here
        shotLine = GetComponent<LineRenderer>();
        playerMov = GetComponent<Playermov>();
        //start with full clip
        currentAmmo = ammoMax;
        ammoCountTxt = GameObject.FindGameObjectsWithTag("AmmoCounter");
        foreach(GameObject i in ammoCountTxt)
        {
            i.GetComponent<Text>().text = (currentAmmo).ToString();
        }
        ammoSlide.value = currentAmmo;
    }

    private void Update()
    {
        //timer for firerate
        nextFire += Time.deltaTime;
        //shoot is held, and can fire
        if (shooting && nextFire > fireRate &&currentAmmo>0)
        {
            Shoot();


            //shoot animation
            m_animator.SetTrigger("Shoot");

            currentAmmo -= 1;
            Debug.Log(currentAmmo);
        }

    }
    //'input for shooting
    public void Fire(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            shooting = true;

        }
        else if (context.canceled)
        {
            shooting = false;
        }
    }

    void Shoot()
    {
        //ray start at camera
        Vector3 origin = camera.transform.position;
        RaycastHit hit;
        //set the line to gun tip
        shotLine.SetPosition(0, gunTip.position);
        Debug.Log("Shoot");
        //set the line active and then deactivate on timer
        StartCoroutine(ShotEffect());
        //reset firerate time
        nextFire = 0;

        //add the recoil to the player mov script
        playerMov.recoil += Random.Range(1f, 2f);

        

        //if hit 
        if (Physics.Raycast(origin, camera.transform.forward, out hit, gunRange))
        {
            shotLine.SetPosition(1, hit.point);
            Debug.Log(hit.collider.gameObject.name);
            if(hit.collider.gameObject.tag == "Target")
            {
                hit.collider.gameObject.GetComponent<MovingTargets>().Die();
            }
        }
        //if miss
        else
        {
            //set line some units infront of the gun
            shotLine.SetPosition(1, gunTip.position + (camera.transform.forward * gunRange));
        }

        //Ammo count ui update
        foreach (GameObject i in ammoCountTxt)
        {
            i.GetComponent<Text>().text = (currentAmmo).ToString();
        }
        ammoSlide.value = currentAmmo;
    }
    //play audio here aswell 
    private IEnumerator ShotEffect()
    {
        shotLine.enabled = true;
        yield return lineDuration;
        shotLine.enabled = false;
    }
    //reload but needs time from animation to delay it
    public void Reload(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            currentAmmo = ammoMax;

            m_animator.SetTrigger("Reload");

            //Update UI
            foreach (GameObject i in ammoCountTxt)
            {
                i.GetComponent<Text>().text = (currentAmmo).ToString();
            }
            ammoSlide.value = currentAmmo;
        }
    }
}
