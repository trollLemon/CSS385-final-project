using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayBehavior : MonoBehaviour
{
    public GameManager gm;

    public bool daylock = true;

  
    public GameObject playArea;
    private Bounds bounds;

    public float treePadding;
    public GameObject Tree;

    public float numTrees;

    void Start()
    {
    gm = GetComponent<GameManager>();

    playArea = GameObject.Find("PlayArea");
    bounds = playArea.GetComponent<Collider2D>().bounds;
    for(int i =0 ; i< numTrees; ++i)
        {
           SpawnTree();
        }
    }


    private void SpawnTree()
    {
        float randomY = Random.Range(bounds.min.y+treePadding, bounds.max.y-treePadding);
        float randomX = Random.Range(bounds.min.x+treePadding, bounds.max.x-treePadding);
        Vector3 spawnPosition = new Vector3(randomX,randomY,randomY); //map z to y
        Instantiate(Tree, spawnPosition, Quaternion.identity);
    }
    // Update is called once per frame
    void Update()
    {
        if(gm.gameState == GameManager.GameState.Night){
            daylock = false;
        }
        if(daylock) return;
        if(gm.gameState == GameManager.GameState.Day){
            daylock = true;

            //spawn some trees
            for(int i =0 ; i< numTrees; ++i)
            {
                SpawnTree();
            }
        }
    }
}
