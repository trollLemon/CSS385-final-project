using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapZtoY : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;

        pos.z=pos.y*0.01f;

        transform.position=pos;
        
    }
}
