using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeController : MonoBehaviour
{
    private GameObject player;

    private PlayerMovementOld playerMovementOld;

    public bool isAttacking = false;

    public int damage = 15;

    public int comboEndDamage = 20;
    public int superSwingDamage = 40;

    private Color originalColor;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        playerMovementOld = player.GetComponent<PlayerMovementOld>();
    }

    // Update is called once per frame
    void Update()
    {
        isAttacking = playerMovementOld.isAttacking;
    }

    void OnTriggerEnter2D(Collider2D other){

        Debug.Log("Hit Something");

        if(!isAttacking) return;
        
            if (other.name == "Tree" || other.name == "Tree(Clone)" || other.name == "Enemy" || other.name == "Enemy(Clone)") 
            {

                if (playerMovementOld.comboSwing3 == true)
                {
                    HP obj = other.GetComponent<HP>();
                    obj.Damage((float)damage);
                } else if (playerMovementOld.comboSuperSwing == true)
                {
                    HP obj = other.GetComponent<HP>();
                    obj.Damage((float)comboEndDamage);
                } else
                {
                    HP obj = other.GetComponent<HP>();
                    obj.Damage((float)superSwingDamage);
                }
                
            }

    }
}
