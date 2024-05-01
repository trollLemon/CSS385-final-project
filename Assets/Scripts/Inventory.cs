using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{

    public int torches;
    public int coal;

    public int sticks;
    
    public TMP_Text tmp;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        tmp.text = "Coal:" + coal + "\nSticks:" + sticks +"\nTorches:" + torches + " \nCraft: C" +coal+ "/1, S" + sticks +"/1" ;  
    }

    public int Craft(){


        if (coal>=1 && sticks >= 1){
            torches++;           
            coal--;
            sticks--; 

            if(coal<0) coal=0;
            if(torches<0) torches=0;
            return 0;
        }

        return 1;
    }
}
