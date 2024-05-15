using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour
{

   
    public InventoryManager items;
    // Start is called before the first frame update
    void Start()
    {
     items = GameObject.Find("Equipment").GetComponent<InventoryManager>();   
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            items.switchItems();
        }

        if(Input.GetKeyDown(KeyCode.R)){
            Debug.Log("Craft Torch");
            items.CraftTorch();
        }
    }
}
