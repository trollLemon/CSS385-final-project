using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeController : MonoBehaviour
{
    private GameObject player;

    private PlayerMovementOld playerMovementOld;

    public bool isAttacking = false;

    public float damage = 15.0f;

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
}
