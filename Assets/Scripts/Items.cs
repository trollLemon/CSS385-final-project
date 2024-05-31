using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour
{

   
    public InventoryManager items;

    public CraftingBar craftingBar;
    public GameObject craftingGameObject;
    public InventoryManager inv;

    public float craftingTime = 3f;

    public float currentCraftingTime;

    private float startTime;

    private float currentTime;

    private bool startCrafting = false;

    // Start is called before the first frame update
    void Start()
    {
        items = GameObject.Find("Equipment").GetComponent<InventoryManager>(); 
        currentCraftingTime = 0f; 
        craftingBar = GameObject.Find("CraftingBar").GetComponent<CraftingBar>();
        craftingGameObject = GameObject.Find("CraftingBar");
        inv = GameObject.Find("Equipment").GetComponent<InventoryManager>();
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

        if(Input.GetKey(KeyCode.R) && ((items.selectedItem == 2  && inv.coal > 0 && inv.sticks > 0) || (items.selectedItem == 3&& inv.logs > 0 )) )
        {
            if (!startCrafting)
            {
                startTime = Time.time;
                currentTime = startTime;
                startCrafting = true;
                craftingGameObject.SetActive(true);
            } else
            {
                currentCraftingTime = currentTime - startTime;
                currentTime = Time.time;
                if (currentTime - startTime > craftingTime)
                {
                    if (items.selectedItem == 2)
                    {
                        Debug.Log("Craft Torch");
                        items.CraftTorch();
                        startCrafting = false;
                        currentCraftingTime = 0;
                    } else if (items.selectedItem == 3)
                    {
                        Debug.Log("Craft Barrier");
                        items.CraftBarrier();
                        startCrafting = false;
                        currentCraftingTime = 0;
                    }
                }
            }

        } else
        {
            startCrafting = false;
            currentCraftingTime = 0;
            craftingGameObject.SetActive(false);
            
        }

        // if(Input.GetKeyDown(KeyCode.R) && items.selectedItem == 2){
        //     Debug.Log("Craft Torch");
        //     items.CraftTorch();
        // } else if (Input.GetKeyDown(KeyCode.R) && items.selectedItem == 3)
        // {
        //     Debug.Log("Craft Barrier");
        //     items.CraftBarrier();
        // }
    }
}
