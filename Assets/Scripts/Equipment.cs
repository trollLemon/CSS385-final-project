using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class Equipment : MonoBehaviour
{

public GameObject[] items;
public int selectedItem;
public int numItems;
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
    }

    public void switchItems(){
        selectedItem = (selectedItem+1) % numItems;
        transform.position=items[selectedItem].transform.position;

    }

    
}
