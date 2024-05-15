using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    // animator
    public Animator animator;

    // Movement Settings
    public float maxSpeed = 10f;
    public float sensitivity = 1f;

    // Movement
    private float horizontalInput;
    private float verticalInput;

    // Aiming
    private Vector2 aimInputRaw;
    private Vector2 aimInput;

    // Primary and Secondary Flags
    private bool primaryInput;
    private bool secondaryInput;

    // Is Contoller or Mouse/Keyboard in use?
    private bool controllerInUse;

    private SpriteRenderer spr;

    private void OnHorizontalMovement(InputValue axis)
    {
        horizontalInput = axis.Get<float>();
    }

    private void OnVerticalMovement(InputValue axis)
    {
        verticalInput = axis.Get<float>();
    }

    private void OnAim(InputValue axis)
    {
        aimInputRaw = axis.Get<Vector2>();

        if (!isController(aimInputRaw))
        {
            aimInput = Camera.main.ScreenToWorldPoint(aimInputRaw);
            aimInput.x = aimInput.x - transform.position.x;
            aimInput.y = aimInput.y - transform.position.y;
            aimInput.Normalize();
            controllerInUse = false;
        } else 
        {
            aimInput = aimInputRaw;
            controllerInUse = true;
        }
    }
    
    void Start(){

        spr=GetComponent<SpriteRenderer>();

    }
    // Update is called once per frame
    void Update()
    {

        if (!isController(aimInputRaw))
        {
            aimInput = Camera.main.ScreenToWorldPoint(aimInputRaw);
            aimInput.x = aimInput.x - transform.position.x;
            aimInput.y = aimInput.y - transform.position.y;
            aimInput.Normalize();
            controllerInUse = false;
        } else 
        {
            aimInput = aimInputRaw;
            controllerInUse = true;
        }

        // Calculate movement direction
        Vector3 moveDirection = new Vector3(horizontalInput, verticalInput, 0f);

       

        if(horizontalInput>0){
            spr.flipX=true;
        }

        if(horizontalInput<0){
            spr.flipX=false;

        }

        float speed = moveDirection.magnitude * maxSpeed * sensitivity;
        animator.SetFloat("player_speed", Mathf.Abs(speed));

        // Calculate speed based on joystick tilt

        // Normalize the moveDirection if it's not zero to keep the same direction but adjust speed
        if (moveDirection != Vector3.zero)
        {
            moveDirection.Normalize();
        }

        // Move the cube
        transform.Translate(moveDirection * speed * Time.deltaTime);
    }

    private bool isController(Vector2 aimInput)
    {
        Vector2 mousePosition = Input.mousePosition;
        if (aimInput == mousePosition)
        {
            return false;
        }
        return true;
    }

    public Vector2 getAimInput()
    {
        return aimInput;
    }

    public bool getPrimaryInput()
    {
        return primaryInput;
    }

    public bool getSecondaryInput()
    {
        return secondaryInput;
    }

    public bool getIsController()
    {
        return controllerInUse;
    }
}
