using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;




public class Equipment : MonoBehaviour
{

public GameObject[] items;
public int selectedItem;
public int numItems;

public Inventory inventory;
    // Start is called before the first frame update
    void Start()
    {
        transform.position=items[0].transform.position;
        selectedItem=0;
        numItems=items.Length;
    }

    // Update is called once per frame
    void Update()
    {
       transform.position=items[selectedItem].transform.position;

        if(Input.GetKeyDown(KeyCode.Q))
        {
            switchItems();
        }

        if(Input.GetKeyDown(KeyCode.R)){
            craftTorch();
        }
    }

    public void switchItems(){

        if(inventory.torches == 0)
        {
            selectedItem = (selectedItem+1) % (numItems - 1);
            
        } else
        {
            selectedItem = (selectedItem+1) % numItems;
        }
        
        transform.position=items[selectedItem].transform.position;

    }

    public void craftTorch(){
        
           inventory.Craft(); 

    }

    
}
