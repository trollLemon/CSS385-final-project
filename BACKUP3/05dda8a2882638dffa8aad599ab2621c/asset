using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierHealth : MonoBehaviour
{

    public int health = 100;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void Damage(int dmg)
    {

        health-=dmg;

        if(health<=0){
            Destroy(gameObject);
        }
    }
}
