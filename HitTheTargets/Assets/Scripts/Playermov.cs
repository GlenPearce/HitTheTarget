using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class Playermov : MonoBehaviour
{
    //Public setting
    [Header("Player cam and capsule")]
    public GameObject player;
    public Camera playerCam;
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
    Vector2 camRotation, movementInput, pointer;
    bool grounded, sliding, dashing, hovering;
    bool doubleJump = false;
    float horizontal, vertical, mouseVertical, mouseHorizontal, dashTimer, inAirSpeed, interpolation;
    float hoverAmount, tempMaxSpeed, lookX, lookY;

    PlayerInput playerInput;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        InputActionMap playerActionMap = playerInput.actions.FindActionMap("Gameplay");
        playerActionMap.Enable();
    }
    void Start()
    {
        Debug.Log("Started");
        //init the variables here
        rb = GetComponent<Rigidbody>();
        playerColl = GetComponent<Collider>();
        tempMaxSpeed = maxSpeed;
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Update()
    {
        Debug.Log("x" + lookX);
        Debug.Log("y" + lookY);
        SmoothCamera();
        /*
        lookY = Mathf.Clamp(lookY, -90, 90);
        playerCam.transform.localEulerAngles = new Vector3(0, lookX, 0);
        player.transform.localEulerAngles = new Vector3(0, lookX, 0);
        playerCam.transform.localEulerAngles = new Vector3(lookY, 0, 0);
        */

        //camera
        player.transform.localEulerAngles = new Vector3(0, lookX, 0);
        mouseVertical = Mathf.Clamp(lookY, -80, 80);
        playerCam.transform.localEulerAngles = new Vector3(mouseVertical, lookX, 0);

        //Grounded Check and physics changes
        grounded = Physics.Raycast(player.transform.position, Vector3.down, 1.1f);
        //Check for on ground
        if (grounded && !sliding)
        {
            GroundPhysics();
        }
        //check for in Air
        else
        {
            AirPhysics();
        }
        //movement
        if (!sliding && !dashing)
        {
            //get input and make into vector 3

            horizontal = movementInput.x;
            vertical = movementInput.y;
            movementVec = new Vector3(horizontal, 0.0f, vertical);

            rb.AddRelativeForce(movementVec * speed * inAirSpeed);

            //speed limit ignores limits on Y axis
            float tempY = rb.velocity.y;

            if (rb.velocity.magnitude > maxSpeed)
            {
                rbVelocity = rb.velocity.normalized * maxSpeed;
                rb.velocity = new Vector3(rbVelocity.x, tempY, rbVelocity.z);
            }
        }
    }

    void FixedUpdate()
    {

        if (grounded && !sliding)
        {
            GroundPhysics();
        }
        //check for in Air
        else
        {
            AirPhysics();
        }
        //movement
        if (!sliding && !dashing)
        {
            //get input and make into vector 3
            horizontal = movementInput.x;
            vertical = movementInput.y;
            movementVec = new Vector3(horizontal, 0.0f, vertical);
            rb.AddRelativeForce(movementVec * speed * inAirSpeed);
            //speed limit ignores limits on Y axis
            float tempY = rb.velocity.y;
            if (rb.velocity.magnitude > maxSpeed)
            {
                rbVelocity = rb.velocity.normalized * maxSpeed;
                rb.velocity = new Vector3(rbVelocity.x, tempY, rbVelocity.z);
            }
        }
        //hovering
        if (hoverAmount < maxHover && hovering)
        {
            rb.AddForce(0, hoverPower, 0);
            Debug.Log("charging" + hoverAmount);
            hoverAmount += Time.deltaTime;
        }

        //dash timer
        dashTimer += Time.deltaTime;
        //dash duration
        if (dashTimer >= 0.5f)
        {
            dashing = false;
        }
    }
    /// <summary>
    /// lerping the camera to give a smooth more "human" like movement to the camera
    /// </summary>
    void SmoothCamera()
    {
        interpolation = camLerpSpeed * Time.deltaTime;

        Vector3 position = this.transform.position;
        position.y = Mathf.Lerp(playerCam.transform.position.y, this.transform.position.y + 0.5f, interpolation);
        position.x = Mathf.Lerp(playerCam.transform.position.x, this.transform.position.x, interpolation);

        playerCam.transform.position = position;
    }
    /// <summary>
    /// stats on ground and ground resets
    /// </summary>
    void GroundPhysics()
    {
        inAirSpeed = 1;
        playerColl.material.dynamicFriction = 1;
        playerColl.material.staticFriction = 1;
        hoverAmount = 0;
        maxSpeed = tempMaxSpeed;
    }
    /// <summary>
    /// air movement stats
    /// </summary>
    void AirPhysics()
    {
        inAirSpeed = airMoveMultiply;
        playerColl.material.dynamicFriction = 0;
        playerColl.material.staticFriction = 0;
        maxSpeed = airMaxSpeed;
    }

    /// <summary>
    /// rotates the camera based on mouse movement, multiplying the input by fixed delta time because its supposed to reduce the stuttering but not really noticed any effect
    /// </summary>
    public void cameraMov(InputAction.CallbackContext context)
    {
        lookX += context.ReadValue<Vector2>().x * mouseSens * Time.fixedDeltaTime;
        lookY -= context.ReadValue<Vector2>().y * mouseSens * Time.fixedDeltaTime;
    }
    //-----------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// basic add force in the direction of the inputs, there is a speed cap that ignores the y axis as not to disturb the way jumps work
    /// </summary>
    public void movement(InputAction.CallbackContext context)
    {
       movementInput = context.ReadValue<Vector2>();
    }
    /// <summary>
    /// Dash goes towards player keyboard inputs 
    /// </summary>
    public void Dash()
    {
        Debug.Log("PRESSED");
        if (dashTimer >= dashCooldown)
        {
            if (grounded)
            {
                dashing = true;
                movementDash = new Vector3(horizontal, 0.1f, vertical);
                rb.AddRelativeForce(movementDash * dashSpeed * 2, ForceMode.Impulse);
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
    }
    /// <summary>
    /// hover adds force and adds to the hoveramount based on time
    /// </summary>
    public void hover()
    {
        Debug.Log("PRESSED");
        if (hoverAmount < maxHover&& hovering)
        {
            rb.AddForce(0, hoverPower, 0);
            Debug.Log("charging" + hoverAmount);
            hoverAmount += Time.deltaTime;
        }
    }
    /// <summary>
    /// use for activating the slide
    /// </summary>
    public void Slide(InputAction.CallbackContext context)
    {
        Debug.Log("PRESSED");
        if (context.performed)
        {
            sliding = true;
            rb.AddForce(0, -1, 0, ForceMode.Impulse);
            player.transform.localScale += yChange;
            //change the player material to the sliding values
            playerColl.material.dynamicFriction = slideFricton;
            playerColl.material.staticFriction = slideFricton;
        }
        else if (context.canceled)
        {
            sliding = false;
            player.transform.localScale -= yChange;
            //changes the values back to the original of 1, probably should use a value thats stored on start incase of changes
            playerColl.material.dynamicFriction = 1;
            playerColl.material.staticFriction = 1;
        }
    }
    /// <summary>
    /// enable and disable mouse 
    /// </summary>
    public void MouseLock(InputAction.CallbackContext context)
    {
        Debug.Log("PRESSED");
        if (context.performed)
        {
            if( Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

    }
    /// <summary>
    /// Jumps just adding impulse force to the character, the hover amount is reset upon activating the double jump.
    /// also has the option to add frontal force to the character in the direction of the character but this didnt feel as good as it could.
    /// </summary>
    public void jump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            hovering = true;

            Debug.Log("PRESSED");
            if (grounded == true)
            {
                rb.AddForce(0, jumpForce, 0, ForceMode.Impulse);
                //allow double jump
                doubleJump = true;
                Debug.Log(doubleJump);
            }
            else if (grounded == false && doubleJump == true)
            {
                //reset the y velocity so that the double jump is applied evenly even if the player is already falling.
                rbVelocity = rb.velocity.normalized * maxSpeed;
                rb.velocity = new Vector3(rbVelocity.x, 0, rbVelocity.z);
                hoverAmount = 0;
                rb.AddForce(0, doubleJumpHeight, 0, ForceMode.Impulse);
                rb.AddForce(playerCam.transform.forward * doubleJumpFpower, ForceMode.Impulse);
                //has doublejumped
                doubleJump = false;
            }
        }
        if (context.canceled)
        {
            hovering = false;
        }
    }
}

