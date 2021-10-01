using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Playermov : MonoBehaviour
{
    //Public setting
    [Header("Player cam and capsule")]
    public GameObject player;
    public Camera camera;
    [Header("Player speed settings")]
    public float speed;
    public float maxSpeed;
    public float dashSpeed;
    public float dashCooldown;
    public float jumpForce;
    public float doubleJumpHeight;
    public float doubleJumpFpower;
    public float airMoveMultiply;
    public float maxHover;
    public float hoverPower;
    public float airMaxSpeed;
    [Header("Player slide settings")]
    public float slideFricton;
    [Header("Player settings")]
    public float mouseSens;
    [Header("Camera settings")]
    public float camLerpSpeed;

    /// <summary>
    /// settings that are used for the various mechanics
    /// </summary>
    Rigidbody rb;
    Collider playerColl;

    Vector3 movementVec, movementDash, rbVelocity;
    Vector3 yChange = new Vector3(0, -0.5f, 0);
    Vector2 camRotation;
    bool  grounded, sliding, dashing;
    bool doubleJump = false;
    float horizontal, vertical, mouseVertical, mouseHorizontal, dashTimer, inAirSpeed, interpolation;
    float hoverAmount,tempMaxSpeed;

    void Start()
    {
        //init the variables here
        rb = GetComponent<Rigidbody>();
        playerColl = GetComponent<Collider>();
        tempMaxSpeed = maxSpeed;
    }
    //-----------------------------------------------------------------------------------
    /// <summary>
    /// most of the stuff in update needs cleaning, via being made into methods and adding controller support aswell as organising the heirarchy and order of things are done.
    /// </summary>
    void Update()
    {
        ///camera movement at top of update----------------------------------------------------------------------------------------
        SmoothCamera();
        cameraMov();

        ///Grounded Check and physics changes----------------------------------------------------------------------------------------

        grounded = Physics.Raycast(player.transform.position, Vector3.down, 1.1f);

        ///stats on ground and ground resets
        if (grounded&&!sliding)
        {
            inAirSpeed = 1;
            playerColl.material.dynamicFriction = 1;
            playerColl.material.staticFriction = 1;
            hoverAmount = 0;
            maxSpeed = tempMaxSpeed;
        }
        /// air movement stats
        else
        {
            inAirSpeed = airMoveMultiply;
            playerColl.material.dynamicFriction = 0;
            playerColl.material.staticFriction = 0;
            maxSpeed = airMaxSpeed;
        }

        ///take inputs for ground movement---------------------------------------------------------------------------------------------
        if (!sliding&&!dashing)
        {
            inputs();
        }

        ///dash--------------------------------------------------------------------------------------------------------------------
        dashTimer += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.LeftShift)&& dashTimer >= dashCooldown)
        {
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

        ///Jump--------------------------------------------------------------------------------------------------------------------
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (grounded == true)
            {
                jump();
                doubleJump = true;
            }
            else if (grounded == false && doubleJump == true)
            {
                DoubleJump();
                doubleJump = false;
            }
        }

        if (Input.GetKey(KeyCode.Space) && hoverAmount<maxHover)
        {
            rb.AddForce(0, hoverPower, 0);
            Debug.Log("charging" + hoverAmount);
            hoverAmount += Time.deltaTime;
        }
        //Slide-------------------------------------------------------------------------------------------------------------------
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

        // enable and disable mouse-----------------------------------------------------------------------------------------------
        if (Input.GetKey(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    //-----------------------------------------------------------------------------------------------------------------------------
    void FixedUpdate()
    {
        grounded = (Physics.Raycast(player.transform.position, Vector3.down, 1.1f, LayerMask.NameToLayer("ground")));
        if (!sliding && !dashing)
        {
            movement();
        }

    }
    //-----------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// rotates the camera based on mouse movement, multiplying the input by fixed delta time because its supposed to reduce the stuttering but not really noticed any effect
    /// </summary>
    void cameraMov()
    {
        //grab mouse inputs and clamp the y
        mouseHorizontal += (Input.GetAxis("Mouse X")*Time.fixedDeltaTime) * mouseSens;
        mouseVertical -= (Input.GetAxis("Mouse Y")* Time.fixedDeltaTime) * mouseSens;
        mouseVertical = Mathf.Clamp(mouseVertical, -80, 80);
        //make into a vector2
        camRotation.y = mouseHorizontal;
        camRotation.x = mouseVertical;
        //apply the rotation
        camera.transform.eulerAngles = (Vector2)camRotation;
        transform.eulerAngles = new Vector2(0, mouseHorizontal);
    }
    //-----------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// grabs the axis values of wasd and converts into a vector 3
    /// </summary>
    void inputs()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        movementVec = new Vector3(horizontal, 0.0f, vertical);
    }

    //-----------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// basic add force in the direction of the inputs from the input method on update, there is a speed cap that ignores the y axis as not to disturb the way jumps work
    /// </summary>
    void movement()
    {
         rb.AddRelativeForce(movementVec * speed*inAirSpeed);

         //speed limit ignores limits on Y axis
         float tempY = rb.velocity.y;
         
        if (rb.velocity.magnitude > maxSpeed)
        {
            rbVelocity = rb.velocity.normalized * maxSpeed;
            rb.velocity = new Vector3(rbVelocity.x, tempY, rbVelocity.z);
        }
         
    }

    //----------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Jumps just adding impulse force to the character, the hover amount is reset upon activating the double jump.
    /// also has the option to add frontal force to the character in the direction of the character but this didnt feel as good as it could.
    /// </summary>
    void jump()
    {
        rb.AddForce(0, jumpForce, 0, ForceMode.Impulse);
    }
    void DoubleJump()
    {
        //reset the y velocity so that the double jump is applied evenly even if the player is already falling.
        rbVelocity = rb.velocity.normalized * maxSpeed;
        rb.velocity = new Vector3(rbVelocity.x, 0, rbVelocity.z);
        hoverAmount = 0;
        rb.AddForce(0, doubleJumpHeight, 0, ForceMode.Impulse);
        rb.AddForce(camera.transform.forward * doubleJumpFpower, ForceMode.Impulse);
    }
    //----------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// lerping the camera to give a smooth more "human" like movement to the camera
    /// </summary>
    void SmoothCamera()
    {
        interpolation = camLerpSpeed * Time.deltaTime;

        Vector3 position = this.transform.position;
        position.y = Mathf.Lerp(camera.transform.position.y, this.transform.position.y+0.5f, interpolation);
        position.x = Mathf.Lerp(camera.transform.position.x, this.transform.position.x, interpolation);

        camera.transform.position = position;
    }
}

