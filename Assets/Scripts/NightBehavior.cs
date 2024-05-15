using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] spawns;
    public GameObject enemy;

    public int maxEnemySpawn;
    public GameManager gm;

    public float spawnInterval = 3f;
    void Start()
    {
     gm = GetComponent<GameManager>();
     spawns =  GameObject.FindGameObjectsWithTag("spawner");   
    
     InvokeRepeating("SpawnObjectAtRandomIndex", spawnInterval, spawnInterval);
    }

    // Update is called once per frame
    void Update()
    {
            
    }


    void SpawnObjectAtRandomIndex()
    {
        if(gm.gameState != GameManager.GameState.Night) return;
        // Choose a random index from the arrayObjects array
        int randomIndex = Random.Range(0, spawns.Length);

        // Get the position of the chosen array object
        Vector3 spawnPosition = spawns[randomIndex].transform.position;

        int spawnCount = Random.Range(1, maxEnemySpawn + 1);
        
        for(int i =0; i<spawnCount; ++i)
            Instantiate(enemy, spawnPosition, Quaternion.identity);
      
    }

}
