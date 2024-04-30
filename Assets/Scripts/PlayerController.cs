using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    public float maxSpeed = 10f;
    public float sensitivity = 1f;
    float horizontalInput;
    float verticalInput = 0.0f;


    private void OnHorizontalMovement(InputValue axis)
    {
        horizontalInput = axis.Get<float>();
    }

    private void OnVerticalMovement(InputValue axis)
    {
        verticalInput = axis.Get<float>();
    }
    
    // Update is called once per frame
    void Update()
    {
        // Calculate movement direction
        Vector3 moveDirection = new Vector3(horizontalInput, verticalInput, 0f);

        // Calculate speed based on joystick tilt
        float speed = moveDirection.magnitude * maxSpeed * sensitivity;

        // Normalize the moveDirection if it's not zero to keep the same direction but adjust speed
        if (moveDirection != Vector3.zero)
        {
            moveDirection.Normalize();
        }

        // Move the cube
        transform.Translate(moveDirection * speed * Time.deltaTime);
    }
}
