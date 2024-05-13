using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.InputSystem;

public enum MovementState
{
    Standing,
    Walking,
    Sprinting
}

public enum LookDirection
{
    Up,
    Left,
    Right,
    Down
}

public class PlayerMovementOld : MonoBehaviour
{
    public Animator animator;
    public float moveSpeed;
    public float sprintSpeed;
    public float handReach;
    public float grabDistance;
    public float cameraReachFactor;
    public float cameraExtendedReachFactor;
    public float cameraMoveFactor;
    public float cameraMoveSpeed;
    public MovementState movementState;
    public LookDirection lookDirection;
    public GameObject axePrefab;
    public GameObject torchPrefab;



    private Camera mainCamera;
    private Vector2 previousCameraPos;
    private float cameraReach;
    private GameObject hand;
    private GameObject heldObject;
    private Quaternion initialRotation;
    private bool isHoldingObject = false;
    private SpriteRenderer playerModel;
    private SpriteRenderer handModel;
    private Vector3 mouseWorldPosition;
    private Equipment equipment;

    // Will clean up
    private int previousSelectedItem;
    private bool changeSelected = false;

   

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        hand = transform.Find("Hand").gameObject;
        playerModel = GetComponent<SpriteRenderer>();
        handModel = hand.GetComponent<SpriteRenderer>();
        mouseWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        movementState = MovementState.Standing;
        lookDirection = LookDirection.Up;
        equipment = GameObject.Find("Equipment").GetComponent<Equipment>();

        animator = GetComponent<Animator>();

        // Will clean up
        previousSelectedItem = equipment.selectedItem;
    }

    // Update is called once per frame
    void Update()
    {
        // Keep track of mouse position
        mouseWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        // Perform movement
        Movement();

        // Perform aiming
        Aiming();

        // Depending on aim direction, change sprite model to match
        ChangeModelDirection();

        // Camera positioning
        CameraPosition();

        // Grab or drop objects when E is pressed
        if (Input.GetKeyDown(KeyCode.E) && heldObject == null)
        {
            CheckForGrab();
        } else if (Input.GetKeyDown(KeyCode.E) && heldObject != null)
        {
            DropObject();
        }
        UpdateHeldObjectRotation();

        // Extend Camera View
        if (Input.GetKey(KeyCode.LeftControl))
        {
            cameraReach = cameraExtendedReachFactor;
        } else
        {
            cameraReach = cameraReachFactor;
        }

        if (previousSelectedItem != equipment.selectedItem)
        {
            changeSelected = true;
            previousSelectedItem = equipment.selectedItem;
        }

        if(equipment.selectedItem == 0 && changeSelected) {

            Destroy(heldObject);
            changeSelected = false;
        } else if (equipment.selectedItem == 1 && changeSelected) 
        {

            if (heldObject != null) {
                DropObject();
            }

            if(heldObject == null) {
                GameObject newAxe = Instantiate(axePrefab, hand.transform.position, transform.rotation);
                heldObject = newAxe;
                // Attach object to hand
                newAxe.transform.SetParent(hand.transform);
                newAxe.transform.localPosition = Vector3.zero;
            }
            changeSelected = false;

        } else if (equipment.selectedItem == 2 && changeSelected)
        {
            if(heldObject.name == "Axe" || heldObject.name == "Axe(Clone)"){
                Destroy(heldObject);
            }

            if (heldObject != null) {
                DropObject();
            }

            if(heldObject == null)
            {
                
                GameObject newTorch = Instantiate(torchPrefab, hand.transform.position, transform.rotation);
                heldObject = newTorch;
                // Store initial local rotation relative to hand
                initialRotation = heldObject.transform.rotation;
              
                // Attach object to hand
                newTorch.transform.SetParent(hand.transform);
                newTorch.transform.localPosition = Vector3.zero;
                isHoldingObject = true;
            }
            changeSelected = false;
        }
        
    }

    private void Movement()
    {
        // Get input from keyboard
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        if (horizontalInput == 0 && verticalInput == 0)
        {
            movementState = MovementState.Standing;
            animator.SetFloat("speed", 0);
        } else
        {
            movementState = MovementState.Walking;
            animator.SetFloat("speed", 1);
        }


        // Calculate movement direction
        Vector3 moveDirection = new Vector3(horizontalInput, verticalInput, 0f);

        // Calculate speed based input
        float speed = 0;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = moveDirection.magnitude * sprintSpeed;
            movementState = MovementState.Sprinting;
        } else
        {
            speed = moveDirection.magnitude * moveSpeed;
        }
        

        // Normalize the moveDirection if it's not zero to keep the same direction but adjust speed
        if (moveDirection != Vector3.zero)
        {
            moveDirection.Normalize();
        }

        // Move the Player
        transform.Translate(moveDirection * speed * Time.deltaTime);
        
    }

    private void Aiming()
    {
        Vector3 targetDirection = mouseWorldPosition - transform.position;
        Vector2 handUnitCircle = new Vector2(targetDirection.x, targetDirection.y);
        if (handUnitCircle.magnitude > handReach)
        {
            handUnitCircle.Normalize();
            handUnitCircle *= handReach;
        }
        float targetAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;

        if (Mathf.Abs(targetAngle) < 90)
        {
            lookDirection = LookDirection.Right;
        } else
        {
            lookDirection = LookDirection.Left;
        }

        Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle - 90);
        hand.transform.rotation = Quaternion.RotateTowards(hand.transform.rotation, targetRotation, 1000 * Time.deltaTime);
        hand.transform.position = new Vector3(handUnitCircle.x + transform.position.x, handUnitCircle.y + transform.position.y, transform.position.z - 1);
    }

    private void ChangeModelDirection()
    {
        if (lookDirection == LookDirection.Left)
        {
            playerModel.flipX = false;
            Vector3 scale = hand.transform.localScale;
            scale.x = 1;
            hand.transform.localScale = scale;
        }

        if (lookDirection == LookDirection.Right)
        {
            playerModel.flipX = true;
            Vector3 scale = hand.transform.localScale;
            scale.x = -1;
            hand.transform.localScale = scale;
        }
    }

    private void CameraPosition()
    {
        Vector3 targetDirection = mouseWorldPosition - transform.position;
        Vector2 cameraUnitCircle = new Vector2(targetDirection.x, targetDirection.y);
        if (cameraUnitCircle.magnitude < 1 * cameraMoveFactor)
        {
            cameraUnitCircle = new Vector2(0.0f, 0.0f);
        } else if (cameraUnitCircle.magnitude < 2 * cameraMoveFactor)
        {
            cameraUnitCircle.Normalize();
            cameraUnitCircle *= 1.0f * cameraReach;
        } else if (cameraUnitCircle.magnitude < 3 * cameraMoveFactor)
        {
            cameraUnitCircle.Normalize();
            cameraUnitCircle *= 2.0f * cameraReach;
        } else if (cameraUnitCircle.magnitude < 4 * cameraMoveFactor)
        {
            cameraUnitCircle.Normalize();
            cameraUnitCircle *= 3.0f * cameraReach;
        } else if (cameraUnitCircle.magnitude < 5 * cameraMoveFactor)
        {
            cameraUnitCircle.Normalize();
            cameraUnitCircle *= 4.0f * cameraReach;
        } else if (cameraUnitCircle.magnitude < 6 * cameraMoveFactor)
        {
            cameraUnitCircle.Normalize();
            cameraUnitCircle *= 5.0f * cameraReach;
        } else
        {
            cameraUnitCircle.Normalize();
            cameraUnitCircle *= 6.0f * cameraReach;
        }


        Vector3 followPlayer = new Vector3(previousCameraPos.x + transform.position.x, previousCameraPos.y + transform.position.y, mainCamera.transform.position.z);
        mainCamera.transform.position = followPlayer;


        Vector3 targetCamPos = new Vector3(cameraUnitCircle.x + transform.position.x, cameraUnitCircle.y + transform.position.y, mainCamera.transform.position.z);
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetCamPos, cameraMoveSpeed * Time.deltaTime);

        previousCameraPos = new Vector2(mainCamera.transform.position.x - transform.position.x, mainCamera.transform.position.y - transform.position.y);
    }

    void CheckForGrab()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(hand.transform.position, grabDistance);      

        foreach (Collider2D col in colliders)
        {
            if (col.name == "Torch" || col.name == "Torch(Clone)")
            {
                // Check if object is already held
                if (heldObject == null)
                {
                    heldObject = col.gameObject;
                    // Store initial local rotation relative to hand
                    initialRotation = heldObject.transform.rotation;
                    // Attach object to hand
                    col.transform.SetParent(hand.transform);
                    col.transform.localPosition = Vector3.zero;
                    isHoldingObject = true;
                    break;
                }
            } else if (col.name == "Axe" || col.name == "Axe(Clone)")
            {
                // Check if object is already held
                if (heldObject == null)
                {
                    heldObject = col.gameObject;
                    // Attach object to hand
                    col.transform.SetParent(hand.transform);
                    col.transform.localPosition = Vector3.zero;
                    break;
                }
            }
        }
    }

    void UpdateHeldObjectRotation()
    {
        if (heldObject != null && isHoldingObject)
        {
            heldObject.transform.rotation = initialRotation;
        }
    }

    void DropObject()
    {
        if (heldObject != null)
        {
            if (heldObject.name == "Torch" || heldObject.name == "Torch(Clone)")
            {
                GameObject.Find("Inventory").GetComponent<Inventory>().torches--;
            }
            heldObject.transform.SetParent(null);
            heldObject = null;
            isHoldingObject = false;
        }
    }
}
