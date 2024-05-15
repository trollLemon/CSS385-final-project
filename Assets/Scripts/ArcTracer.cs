using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Collections;
using TMPro;

public class ArcTracer : MonoBehaviour
{
    public Transform playerObject; // The object around which the arc is traced
    public Transform axeObject;
    public float swingSpeed = 25f; // Speed at which the axeObject follows the arc

    public float arcWidth = 45.0f;

    public float handReach = 1;

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

    public LookDirection lookDirection;

    private Vector3 mouseWorldPosition;
    private Camera mainCamera;

    private List<Vector3> arcPoints = new List<Vector3>(); // List to store arc points
    private List<Vector3> arcRotation = new List<Vector3>(); // List to store arc points
    private int currentPointIndex = 0; // Index of the current point on the arc
    int nextIndex = 1;

    private GameObject hand;

    public GameObject dot1;
    public GameObject dot2;
    public GameObject dot3;
    public GameObject dot4;

    void Start()
    {

        playerObject = GameObject.Find("Player").transform;
        axeObject = GameObject.Find("Axe").transform;

        dot1 = GameObject.Find("dot1");
        dot2 = GameObject.Find("dot2");
        dot3 = GameObject.Find("dot3");

        hand = GameObject.Find("Axe");

        mainCamera = Camera.main;
        mouseWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        prevSwingSpeed = swingSpeed;
        prevHandReach = handReach;

        previousAxeBlockSize = axeBlockObject.transform.localScale;
    }

    void Update()
    {

        mouseWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        if (!isAttacking)
        {
            ChangeModelDirection();
            Aiming();
        }
        
        if (!isBlocking)
        {
            AttackLogic();
        }
        
        if (!isAttacking)
        {
            BlockingLogic();
        }
    }

    private void BlockingLogic()
    {
        if (Input.GetMouseButton(1))
        {
            
            if (!isBlocking)
            {
                prevHandReach = handReach;
                instantedAxeBlockObject = Instantiate(axeBlockObject, hand.transform.position, Quaternion.identity, hand.transform);
            }
            isBlocking = true;
            
        } else
        {
            if (isBlocking)
            {
                Destroy(instantedAxeBlockObject);
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
            Vector3 targetDirection = arcRotation[currentPointIndex] - transform.position;
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
            }
            
            yield return new WaitForSeconds(0.5f);
            if(endOfSwing && !comboSuperSwing && superSwingPrimed && !comboSwing1 && !comboSwing2 && !comboSwing3)
            {
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


    private void ChangeModelDirection()
    {
        if (lookDirection == LookDirection.Left)
        {

            Vector3 scale = hand.transform.localScale;
            scale.x = 1;
            hand.transform.localScale = scale;
        }

        if (lookDirection == LookDirection.Right)
        {

            Vector3 scale = hand.transform.localScale;
            scale.x = -1;
            hand.transform.localScale = scale;
        }
    }
}
