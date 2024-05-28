using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBehavior : MonoBehaviour
{

    public int itemId;
    public InventoryManager inv;
    public bool pickedUp = false;
    public SoundAPI sapi;
    public GameObject goldPile;
    bool destroyed = false;
    private Gold gld;
    // Start is called before the first frame update
    void Start()
    {
        inv = GameObject.Find("Equipment").GetComponent<InventoryManager>();
        goldPile =  GameObject.Find("GoldPile");
        gld = goldPile.GetComponent<Gold>();
    	sapi = GetComponent<SoundAPI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(destroyed) return;
        if (other.CompareTag("Player"))
        {
                
                if(itemId==79 /*gold!!!*/)
                {
                    gld.Return(1);
                    pickedUp=true;
                }

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
	    sapi.OneShot();
	    destroyed=true;
            Destroy(gameObject,0.15f);
        }
    }
}
