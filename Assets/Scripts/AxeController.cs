using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeController : MonoBehaviour
{
    private GameObject player;

    private PlayerMovementOld playerMovementOld;

    public bool isAttacking = false;

    public int damage = 15;

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
        if (other.CompareTag("ForestCreature")){

                Enemy ai = other.GetComponent<Enemy>();
                ai.Damage(damage);

        }

    }
}
