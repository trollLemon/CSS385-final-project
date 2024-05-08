using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private SpriteRenderer spr;
    public float maxSpeed = 10f;
    public float sensitivity = 1f;

    // Start is called before the first frame update
    void Start()
    {
        spr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // Get input from joystick axes
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");


        //if(horizontalInput>0){
        //    spr.flipX=true;
        //else{
        //    spr.flipX=false;
        //


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
