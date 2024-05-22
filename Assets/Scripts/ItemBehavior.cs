using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBehavior : MonoBehaviour
{

    public int itemId;
    public InventoryManager inv;
    public bool pickedUp = false;
    // Start is called before the first frame update
    void Start()
    {
        inv = GameObject.Find("Equipment").GetComponent<InventoryManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
       
        if (other.CompareTag("Player"))
        {
             Debug.Log("Hit");
                if(itemId == 6){
                    if (inv.PickUpLog() == 0)
                    {
                        pickedUp=true;

                    }
                }
                if(itemId == 5){
                    if (inv.PickUpStick() == 0)
                    {
                        pickedUp=true;

                    }
                }

                if(itemId == 4) {
                    if (inv.PickUpCoal() == 0)
                    {
                        pickedUp=true;

                    }

                }
        
        }

        if(pickedUp){
            Destroy(gameObject);
        }
    }
}
