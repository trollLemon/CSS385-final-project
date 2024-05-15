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
    Sprinting,
    Blocking
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
    private float prevMoveSpeed;
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


    public Transform playerObject;
    public Transform axeObject;
    public float swingSpeed = 25f; // Speed at which the axeObject follows the arc
    public float arcWidth = 45.0f;
    public bool isAttacking = false;
    public bool isBlocking = false;
    public bool comboSwing1 = false;
    public bool comboSwing2 = false;
    public bool comboSwing3 = false;
    public bool superSwingPrimed = false;
    public bool comboSuperSwing = false;

    private bool endOfSwing = false;
    private float prevSwingSpeed;

    public GameObject axeBlockObject;
    private GameObject instantedAxeBlockObject;
    private Vector3 previousAxeBlockSize;
    private bool expandAxeBlock = false;
    private float prevHandReach;













    private Camera mainCamera;
    private Vector2 previousCameraPos;
    private float cameraReach;
    private GameObject hand;
    private GameObject heldObject;
    private Quaternion initialRotation;
    private bool isHoldingObject = false;
    private SpriteRenderer playerModel;
    private SpriteRenderer handModel;
    private SpriteRenderer axeModel;
    private Color originalAxeColor;
    private Vector3 mouseWorldPosition;
    private Equipment equipment;



    private List<Vector3> arcPoints = new List<Vector3>(); // List to store arc points
    private List<Vector3> arcRotation = new List<Vector3>(); // List to store arc points
    private int currentPointIndex = 0; // Index of the current point on the arc
    int nextIndex = 1;





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


        playerObject = GameObject.Find("Player").transform;
        axeObject = GameObject.Find("Hand").transform;
        prevSwingSpeed = swingSpeed;
        prevHandReach = handReach;

        previousAxeBlockSize = axeBlockObject.transform.localScale;

        prevMoveSpeed = moveSpeed;


        // Will clean up
        previousSelectedItem = equipment.selectedItem;
    }

    // Update is called once per frame
    void Update()
    {
        // Keep track of mouse position
        mouseWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        if (!isAttacking)
        {
            // Perform movement
            Movement();
            

            // Perform aiming
            Aiming();

            // Depending on aim direction, change sprite model to match
            ChangeModelDirection();
        }

        // Movement animation control
        MovementAnimation();


        // Camera positioning
        CameraPosition();


        // Attacking and blocking.
        if (heldObject != null)
        {
            if (heldObject.name == "Axe" || heldObject.name == "Axe(Clone)")
            {
                if (!isBlocking)
                {
                    AttackLogic();

                    // Attacking animation control
                    AttackingAnimation();
                }
                
                if (!isAttacking)
                {
                    BlockingLogic();
                }
            }

        }

        // Grab or drop objects when E is pressed
        if (Input.GetKeyDown(KeyCode.E) && heldObject == null)
        {
            
            CheckForGrab();
        
            
        } else if (Input.GetKeyDown(KeyCode.E) && heldObject != null)
        {
            if (heldObject.name != "Axe" && heldObject.name != "Axe(Clone)")
            {
                DropObject();
            }
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

        // change to axe.
        } else if (equipment.selectedItem == 1 && changeSelected) 
        {

            if (heldObject != null) {
                DropObject();
            }

            if(heldObject == null) {
                GameObject newAxe = Instantiate(axePrefab, hand.transform.position, hand.transform.rotation);
                heldObject = newAxe;
                

                axeModel = heldObject.GetComponent<SpriteRenderer>();

                // Get the initial rotation of the new object
                Quaternion initialRotation = heldObject.transform.rotation;

                Quaternion additionalRotation;

                if (lookDirection == LookDirection.Right)
                {
                    // Calculate the additional rotation by 15 degrees around the Z-axis
                    additionalRotation = Quaternion.Euler(0f, 0f, 25f);


                    Vector3 scale = heldObject.transform.localScale;
                    scale.x = -1;
                    heldObject.transform.localScale = scale;


                } else
                {
                    additionalRotation = Quaternion.Euler(0f, 0f, -25f);
                }
                

                // Combine the initial rotation with the additional rotation
                Quaternion finalRotation = initialRotation * additionalRotation;

                // Apply the final rotation to the new object
                heldObject.transform.rotation = finalRotation;



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

    private void MovementAnimation()
    {
        if(movementState == MovementState.Standing)
        {
            animator.SetFloat("speed", 0);

        } else if (movementState == MovementState.Walking)
        {
            animator.SetFloat("speed", 1);
        } else if (movementState == MovementState.Blocking)
        {
            animator.SetFloat("speed", 0.3f);
        }
    }

    private void AttackingAnimation()
    {
        if(isAttacking)
        {

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
            
        } else
        {
            movementState = MovementState.Walking;
            
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

        Quaternion targetRotation;
        if(isBlocking)
        {
            if (lookDirection == LookDirection.Right)
            {
                targetRotation = Quaternion.Euler(0, 0, targetAngle);
            } else
            {
                targetRotation = Quaternion.Euler(0, 0, targetAngle - 180);
            }
            
        } else
        {
            targetRotation = Quaternion.Euler(0, 0, targetAngle - 90);
        }
        
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
                // // Check if object is already held
                // if (heldObject == null)
                // {
                //     heldObject = col.gameObject;
                //     // Store initial local rotation relative to hand
                //     initialRotation = heldObject.transform.rotation;
                //     // Attach object to hand
                //     col.transform.SetParent(hand.transform);
                //     col.transform.localPosition = Vector3.zero;
                //     isHoldingObject = true;
                //     break;
                // }
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

    private void BlockingLogic()
    {
        if (Input.GetMouseButton(1))
        {
            
            if (!isBlocking)
            {
                prevMoveSpeed = moveSpeed;
                prevHandReach = handReach;
                instantedAxeBlockObject = Instantiate(axeBlockObject, hand.transform.position, Quaternion.identity, hand.transform);
                moveSpeed /= 4;
            }
            isBlocking = true;
            movementState = MovementState.Blocking;
            
        } else
        {
            if (isBlocking)
            {
                Destroy(instantedAxeBlockObject);
                moveSpeed = prevMoveSpeed;
            }
            isBlocking = false;
        }

        if (isBlocking && Input.GetMouseButtonDown(0) && !expandAxeBlock)
        {
            expandAxeBlock = true;
            Vector3 newScale = new Vector3(previousAxeBlockSize.x * 2, previousAxeBlockSize.y * 2, previousAxeBlockSize.z);
            instantedAxeBlockObject.transform.localScale = newScale;
            handReach *= 1.3f;
        } else if(isBlocking && expandAxeBlock)
        {
            StartCoroutine(RestoreBlockSize());
        }

    }

    IEnumerator RestoreBlockSize()
    {
        yield return new WaitForSeconds(0.2f);
        if (instantedAxeBlockObject != null)
        {
            instantedAxeBlockObject.transform.localScale = previousAxeBlockSize;
            expandAxeBlock = false;
            handReach = prevHandReach;
        }
        
    }

    private void AttackLogic()
    {
        if (Input.GetMouseButtonDown(0) && !isAttacking && !endOfSwing)
        {
            movementState = MovementState.Standing;
            prevSwingSpeed = swingSpeed;
            isAttacking = true;

            comboSwing1 = true;
            comboSwing2 = false;
            comboSwing3 = false;
            superSwingPrimed = false;
            comboSuperSwing = false;
            endOfSwing = false;

            CalculateArc();

            if (lookDirection == LookDirection.Right)
            {
                currentPointIndex = arcPoints.Count - 1;
                nextIndex = -1;

            } else
            {
                currentPointIndex = 0;
                nextIndex = 1;
            }
        } else if (Input.GetMouseButtonDown(0) && isAttacking && endOfSwing && comboSwing1 && !comboSwing2 && !comboSwing3 && !superSwingPrimed && !comboSuperSwing)
        {
            StopCoroutine(DelayIsAttackingFalse());

            comboSwing1 = false;
            comboSwing2 = true;
            comboSwing3 = false;
            superSwingPrimed = false;
            comboSuperSwing = false;
            endOfSwing = false;

            if (lookDirection == LookDirection.Right)
            {
                currentPointIndex = 0;
                nextIndex = 1;

                Vector3 scale = hand.transform.localScale;
                scale.x = 1;
                hand.transform.localScale = scale;

            } else
            {
                currentPointIndex = arcPoints.Count - 1;
                nextIndex = -1;

                Vector3 scale = hand.transform.localScale;
                scale.x = -1;
                hand.transform.localScale = scale;
            }
        } else if (Input.GetMouseButtonDown(0) && isAttacking && endOfSwing && !comboSwing1 && comboSwing2 && !comboSwing3 && !superSwingPrimed && !comboSuperSwing) 
        {
            StopCoroutine(DelayIsAttackingFalse());

            comboSwing1 = false;
            comboSwing2 = false;
            comboSwing3 = true;
            superSwingPrimed = false;
            comboSuperSwing = false;
            endOfSwing = false;

            if (lookDirection == LookDirection.Right)
            {
                currentPointIndex = arcPoints.Count - 1;
                nextIndex = -1;

                Vector3 scale = hand.transform.localScale;
                scale.x = -1;
                hand.transform.localScale = scale;

            } else
            {
                currentPointIndex = 0;
                nextIndex = 1;

                Vector3 scale = hand.transform.localScale;
                scale.x = 1;
                hand.transform.localScale = scale;
            }

        } else if (Input.GetMouseButtonDown(0) && isAttacking && endOfSwing && !comboSwing1 && !comboSwing2 && !comboSwing3 && superSwingPrimed && !comboSuperSwing)
        {
            StopCoroutine(DelayIsAttackingFalse());

            comboSwing1 = false;
            comboSwing2 = false;
            comboSwing3 = false;
            superSwingPrimed = false;
            comboSuperSwing = true;
            endOfSwing = false;

            axeModel.color = originalAxeColor;

            // Calculate the center of the arc
            Vector3 targetDirection = arcPoints[currentPointIndex] - transform.position;
            arcPoints.Clear();
            arcRotation.Clear();
            float targetAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;


            // Calculate the radius of the arc
            float radius = Vector3.Distance(axeObject.position, playerObject.position); // or axeObject.position

            // Calculate points along the arc
            float angleStep = 5f; // Angle step in degrees
            for (float angle = targetAngle; angle <= targetAngle + 360 + arcWidth; angle += angleStep)
            {
                float radianAngle = angle * Mathf.Deg2Rad;
                float x = radius * Mathf.Cos(radianAngle);
                float y = radius * Mathf.Sin(radianAngle);
                Vector3 arcPoint = new Vector3(x, y, 0f); // For 2D, assuming Z is zero
                arcPoints.Add(arcPoint);
                Vector3 arcRotPoint = new Vector3(2*x, 2*y, 0f);
                arcRotation.Add(arcRotPoint);
            }

            if (lookDirection == LookDirection.Right)
            {
                currentPointIndex = arcPoints.Count - 1;
                nextIndex = -1;

                Vector3 scale = hand.transform.localScale;
                scale.x = -1;
                hand.transform.localScale = scale;

            } else
            {
                currentPointIndex = 0;
                nextIndex = 1;

                Vector3 scale = hand.transform.localScale;
                scale.x = 1;
                hand.transform.localScale = scale;
            }

            prevSwingSpeed = swingSpeed;
            swingSpeed *= 2;

        }

        if (!endOfSwing && isAttacking)
        {
            
            // Move the axeObject towards the current arc point
            Vector3 targetPosition = arcPoints[currentPointIndex] + playerObject.transform.position;
            
            if(currentPointIndex == 0 || currentPointIndex == arcPoints.Count - 1)
            {
                axeObject.position = targetPosition;
            }

            // Position!
            float step = swingSpeed * Time.deltaTime;
            axeObject.position = Vector3.MoveTowards(axeObject.position, targetPosition, step);

            // Rotation!
            Vector3 targetDirection = (arcRotation[currentPointIndex] + playerObject.transform.position) - transform.position;
            float targetAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle - 90);
            axeObject.rotation = Quaternion.RotateTowards(axeObject.rotation, targetRotation, 1000 * Time.deltaTime);

            // Check if the axeObject has reached the current arc point
            if (axeObject.position == targetPosition)
            {

                // Move to the next arc point
                currentPointIndex = currentPointIndex + nextIndex;

                if (currentPointIndex == arcPoints.Count - 1 || currentPointIndex == 0)
                {
                    endOfSwing = true;
                    StartCoroutine(DelayIsAttackingFalse());
                }
               
            }
        }
    }

    IEnumerator DelayIsAttackingFalse()
    {
        if (comboSuperSwing || comboSwing3)
        {
            yield return new WaitForSeconds(0.2f);
        } else
        {
            yield return new WaitForSeconds(0.5f);
        }
        

        if(endOfSwing && !comboSwing2 && !superSwingPrimed && !comboSuperSwing)
        {
            endOfSwing = false;
            isAttacking = false;
            comboSwing1 = false;
            comboSwing2 = false;
            comboSwing3 = false;
            superSwingPrimed = false;
            comboSuperSwing = false;
            arcPoints.Clear();

        } else if (endOfSwing && !comboSwing1 && comboSwing2 && !comboSwing3 && !superSwingPrimed && !comboSuperSwing)
        {
            // Turn sprite white!
            yield return new WaitForSeconds(1.0f);
            if (endOfSwing && !comboSwing1 && comboSwing2 && !comboSwing3 && !superSwingPrimed && !comboSuperSwing)
            {
                comboSwing2 = false;
                superSwingPrimed = true;
                originalAxeColor = axeModel.color;
                axeModel.color = Color.red;
            }
            
            yield return new WaitForSeconds(0.5f);
            if(endOfSwing && !comboSuperSwing && superSwingPrimed && !comboSwing1 && !comboSwing2 && !comboSwing3)
            {
                axeModel.color = originalAxeColor;
                endOfSwing = false;
                isAttacking = false;
                comboSwing1 = false;
                comboSwing2 = false;
                comboSwing3 = false;
                superSwingPrimed = false;
                comboSuperSwing = false;
                arcPoints.Clear();
            }
        } else if (endOfSwing && !comboSwing1 && !comboSwing2 && !superSwingPrimed && comboSuperSwing || comboSwing3 )
        {
            endOfSwing = false;
            isAttacking = false;
            comboSwing1 = false;
            comboSwing2 = false;
            comboSwing3 = false;
            superSwingPrimed = false;
            comboSuperSwing = false;
            arcPoints.Clear();
            swingSpeed = prevSwingSpeed;
        }
    }

    void CalculateArc()
    {
        arcPoints.Clear();
        arcRotation.Clear();
        // Calculate the center of the arc

        Vector3 targetDirection = mouseWorldPosition - transform.position;
        float targetAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;


        // Calculate the radius of the arc
        float radius = Vector3.Distance(axeObject.position, playerObject.position); // or axeObject.position

        // Calculate points along the arc
        float angleStep = 5f; // Angle step in degrees
        for (float angle = targetAngle - arcWidth; angle <= targetAngle + arcWidth; angle += angleStep)
        {
            float radianAngle = angle * Mathf.Deg2Rad;
            float x = radius * Mathf.Cos(radianAngle);
            float y = radius * Mathf.Sin(radianAngle);
            Vector3 arcPoint = new Vector3(x, y, 0f); // For 2D, assuming Z is zero
            arcPoints.Add(arcPoint);
            Vector3 arcRotPoint = new Vector3(2*x, 2*y, 0f);
            arcRotation.Add(arcRotPoint);
        }


    }
}
