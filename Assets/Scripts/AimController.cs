using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AimController : MonoBehaviour
{

    public float rotationSpeed = 100.0f;

    private Camera mainCamera;

    GameObject player;

    PlayerController playerController;


    void Start()
    {
        // Get the main camera in the scene
        mainCamera = Camera.main;

        player = GameObject.Find("Player");
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }


    private void Update()
    {

        // Calculate angle between current rotation and target direction
        Vector3 targetDirection = playerController.getAimInput();
        float targetAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle - 90);

        if (playerController.getIsController())
        {
            // Handle Deadzone on controller
            if (targetDirection.x >= 0.01 && targetDirection.y >= 0.01 || 
                targetDirection.x >= 0.01 && targetDirection.y <= -0.01 ||
                targetDirection.x <= -0.01 && targetDirection.y >= 0.01 || 
                targetDirection.x <= -0.01 && targetDirection.y <= -0.01)
                {
                    // Smoothly rotate towards the target angle
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                }
        } else
        {
            // Smoothly rotate towards the target angle
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        
        // Calculate Aim objects new position based on player.
        Vector3 newPosition = player.transform.position;

        if (playerController.getIsController())
        {
            newPosition = newPosition + (targetDirection);
        }

        //// Is the Aim "behind" the player?
        bool behindPlayer;
        if (targetAngle > 45 && targetAngle < 135)
        {
            behindPlayer = true;
        } else
        {
            behindPlayer = false;
        }

        // If behind player then move behind player.
        if (behindPlayer)
        {
            newPosition.z = 1.0f;
        } else
        {
            newPosition.z = -1.0f;
        }
        
        transform.position = newPosition;
    }
}
