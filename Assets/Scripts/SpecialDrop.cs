using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialDrop : MonoBehaviour
{
    // Start is called before the first frame update
    
    public GameObject specialItem;
    public GameObject specialItemRef;
    public bool on = false;
    
    void Start()
    {
    }


    public void Enable()
    {
        on=true;
        Vector3 position = transform.position;
        position.y+=4f;
        specialItemRef = Instantiate(specialItem, position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        if(!on) return;
        Vector3 position = transform.position;
        position.y+=4f;
        specialItemRef.transform.position=position; 


    }
}
