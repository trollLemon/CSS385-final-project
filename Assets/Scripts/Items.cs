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

        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if(Input.GetKeyDown(KeyCode.Q) || scroll < 0f)
        {
            items.switchItems();
        } else if (scroll > 0f)
        {
            items.switchItemsUp();
        }

        if(Input.GetKeyDown(KeyCode.R) && items.selectedItem == 2){
            Debug.Log("Craft Torch");
            items.CraftTorch();
        } else if (Input.GetKeyDown(KeyCode.R) && items.selectedItem == 3)
        {
            Debug.Log("Craft Barrier");
            items.CraftBarrier();
        }
    }
}
