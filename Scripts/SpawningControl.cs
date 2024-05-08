using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpawningControl : MonoBehaviour
{
public GameObject torch;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            SpawnTorchAtMousePosition();
        }
        
    }

    void SpawnTorchAtMousePosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 10; // Distance from the camera to the game world

        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Instantiate(torch, worldPosition, Quaternion.identity);
    }


}

