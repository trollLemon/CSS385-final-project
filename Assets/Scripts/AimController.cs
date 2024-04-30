using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimController : MonoBehaviour
{

    public float rotationSpeed = 100.0f;

    private Camera mainCamera;

    GameObject player;

    void Start()
    {
        // Get the main camera in the scene
        mainCamera = Camera.main;

        player = GameObject.Find("Player");
    }

    private void Update()
    {

        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        worldPosition.z = 0.0f;

        // Calculate angle between current rotation and target direction
        Vector3 targetDirection = worldPosition - transform.position;
        float targetAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle - 90);

        Debug.Log("Target Angle: " + targetAngle);

        bool behindPlayer;
        if (targetAngle > 45 && targetAngle < 135)
        {
            behindPlayer = true;
        } else
        {
            behindPlayer = false;
        }

        // Smoothly rotate towards the target angle
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        Vector3 newPosition = player.transform.position;

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
