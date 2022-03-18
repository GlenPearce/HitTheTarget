using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
    [Header("Gun Stats")]
    float fireRate;
    int gunRange;
    float lineDuration;
    int ammoMax;
    public GameObject pistol;
    public GameObject m4;
    public GameObject railgun;

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

    [Header("Audio")]
    public AudioSource dryFire;
    public AudioSource fire;
    public AudioSource reload;

    void Start()
    {
        //initalise things here
        shotLine = GetComponent<LineRenderer>();
        playerMov = GetComponent<Playermov>();

        //Change gun specs depending on active gun
        int selectedWeapon;
        selectedWeapon = PlayerPrefs.GetInt("SelectedWeapon");
        if (selectedWeapon == 1)
        {
            pistol.SetActive(true);
            fireRate = 0.5f;
            gunRange = 100;
            lineDuration = 1;
            ammoMax = 9;

            m_animator = pistol.GetComponent<Animator>();
        }
        else if (selectedWeapon == 2)
        {
            m4.SetActive(true);
            fireRate = 0.1f;
            gunRange = 100;
            lineDuration = 1;
            ammoMax = 30;

            m_animator = m4.GetComponent<Animator>();
        }
        if (selectedWeapon == 3)
        {
            railgun.SetActive(true);
            fireRate = 2;
            gunRange = 400;
            lineDuration = 1;
            ammoMax = 4;

            m_animator = railgun.GetComponent<Animator>();
        }


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
        if (shooting && nextFire > fireRate && currentAmmo > 0)
        {
            Shoot();
            fire.Play();


            //shoot animation
            m_animator.SetTrigger("Shoot");

            currentAmmo -= 1;
            Debug.Log(currentAmmo);
        }
        else if (shooting && nextFire > fireRate && currentAmmo <= 0)
        {
            dryFire.Play();
            nextFire = 0;
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
            reload.Play();

            //Update UI
            foreach (GameObject i in ammoCountTxt)
            {
                i.GetComponent<Text>().text = (currentAmmo).ToString();
            }
            ammoSlide.value = currentAmmo;
        }
    }
}
