using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Playermov : MonoBehaviour
{
    //Public setting
    [Header("Player cam and capsule")]
    public GameObject player;
    public Transform camera;
    [Header("Player speed settings")]
    public float speed;
    public float maxSpeed;
    public float dashSpeed;
    public float jumpForce;
    public float doubleJumpHeight;
    public float doubleJumpFpower;
    [Header("Player slide settings")]
    public float slideFricton;
    [Header("Player settings")]
    public float mouseSens;


    Rigidbody rb;
    Collider playerColl;

    Vector3 movementVec, movementDash;
    Vector3 yChange = new Vector3(0, -0.5f, 0);

    bool  grounded, sliding, dashing;
    bool doubleJump = false;

    float horizontal, vertical, mouseVertical, mouseHorizontal, dashTimer;

    void Start()
    {
        //init the variables here
        rb = GetComponent<Rigidbody>();
        playerColl = GetComponent<Collider>(); 
    }
    //-----------------------------------------------------------------------------------
    void Update()
    {
        ///camera movement at top of update----------------------------------------------------------------------------------------
        cameraMov();

        ///Only Allow movement on the floor----------------------------------------------------------------------------------------
        grounded = (Physics.Raycast(player.transform.position, Vector3.down, 1.2f, LayerMask.NameToLayer("ground")));
        if (!sliding&&!dashing)
        {
            inputs();
        }

        ///dash--------------------------------------------------------------------------------------------------------------------
        dashTimer += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.LeftShift)&& dashTimer >= 2f){
            if (grounded)
            {
                dashing = true;
                movementDash = new Vector3(horizontal, 0.1f, vertical);
                rb.AddRelativeForce(movementDash * dashSpeed*2, ForceMode.Impulse);
                dashTimer = 0;
            }
            else
            {
                dashing = true;
                movementDash = new Vector3(horizontal, 0.1f, vertical);
                rb.AddRelativeForce(movementDash * dashSpeed, ForceMode.Impulse);
                dashTimer = 0;
            }
        }
        if (dashTimer >= 0.5f)
        {
            dashing = false;
        }


        ///jump--------------------------------------------------------------------------------------------------------------------
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log(grounded);
            if (grounded == true)
            {
                jump();
                doubleJump = true;
            }
            if (grounded == false && doubleJump == true)
            {
                DoubleJump();
                doubleJump = false;
            }
        }
        ///Slide-------------------------------------------------------------------------------------------------------------------
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            sliding = true;
            rb.AddForce(0,-1, 0, ForceMode.Impulse);
            player.transform.localScale += yChange;
            playerColl.material.dynamicFriction = slideFricton;
            playerColl.material.staticFriction = slideFricton;
        }
        if ((Input.GetKeyUp(KeyCode.LeftControl)))
        {
            sliding = false;
            player.transform.localScale -= yChange;
            playerColl.material.dynamicFriction = 1;
            playerColl.material.staticFriction = 1;
        }

        /// enable and disable mouse-----------------------------------------------------------------------------------------------
        if (Input.GetKey(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    //-----------------------------------------------------------------------------------
    ///fixed update for consistancy of movement feel
    void FixedUpdate()
    {
        grounded = (Physics.Raycast(player.transform.position, Vector3.down, 1.1f, LayerMask.NameToLayer("ground")));
        if (grounded && !sliding && !dashing)
        {
            movement();
        }
      
    }
    //-----------------------------------------------------------------------------------

    void cameraMov()
    {
        //camera
        mouseHorizontal = Input.GetAxis("Mouse X") * mouseSens;
        transform.Rotate(0, mouseHorizontal, 0);

        mouseVertical -= Input.GetAxis("Mouse Y") * mouseSens;
        mouseVertical = Mathf.Clamp(mouseVertical, -80, 80);

        Camera.main.transform.localRotation = Quaternion.Euler(mouseVertical, 0, 0);
    }
    //-------------------------------------------------------------------------------------
    void inputs()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        movementVec = new Vector3(horizontal, 0.0f, vertical);
    }

    //----------------------------------------------------------------------------

    void movement()
    {
        rb.AddRelativeForce(movementVec * speed);
        //speed limit ignores limits on Y axis
        float tempY = rb.velocity.y;
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
        rb.velocity = new Vector3(rb.velocity.x, tempY, rb.velocity.z);
    }

    //---------------------------------------------------------------------------

    void jump()
    {
        rb.AddForce(0, jumpForce, 0, ForceMode.Impulse);
    }
    void DoubleJump()
    {
        rb.AddForce(0, doubleJumpHeight, 0, ForceMode.Impulse);
        rb.AddForce(camera.forward * doubleJumpFpower, ForceMode.Impulse);
    }
    //-----------------------------------------------------------------------
}

