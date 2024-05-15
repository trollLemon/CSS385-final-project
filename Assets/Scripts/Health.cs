using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{

public int currHealth = -1;
public int maxHealth = 99;
public float shakeMag = 0.1f;
public float shakeDurr = 0.1f;
private Vector3 initialPosition;
    // Start is called before the first frame update
    void Start()
    {
        currHealth=maxHealth;       
    }

    // Update is called once per frame
    void Update()
    {
    

    }

    public void DamagePlayer(int dmg)
    {

        StartCoroutine(DecreaseHpOverTime(dmg));
    }

    IEnumerator DecreaseHpOverTime(int dmg)
    {
        Debug.Log("ouch");
        while(dmg>0)
        {
        currHealth--;
        dmg--;
        
        yield return new WaitForSeconds(0.02f);
        }
        
    }

}
