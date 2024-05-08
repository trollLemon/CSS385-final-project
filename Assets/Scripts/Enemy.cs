using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    
    [SerializeField] Transform target;

    public GameObject[] targets;
    public GameObject exit;
    private int choice;
    public int goldHeld=0;
    
    NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
     targets = new GameObject[2];
     
     targets[0] = GameObject.Find("Player");
     targets[1] = GameObject.Find("GoldPile");
     exit = GameObject.Find("Exit");
     choice = Random.Range(0, 2);
     target=targets[choice].transform;

     Gold gold = targets[1].GetComponent<Gold>();
     if(gold.gold == 0) target=targets[0].transform;
     agent=GetComponent<NavMeshAgent>();
     agent.updateRotation=false;
     agent.updateUpAxis=false;   
    
    
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(target.position);

        if(transform.position.x==target.position.x && target.position.y==target.position.y){
            PerformNextAction();
        }
    }


    void PerformNextAction(){

        

        // if we are at an exit, we should destory the object, this will only happen if the enemy was going to the exit
        // so we do not need to worry about if this statement executes when the enemy attacks the player. 

        if(Vector3.Distance(transform.position, exit.transform.position)<2f){
            Destroy(gameObject);
            return;
        }
       
    
        if(choice==0){

        } 
        
        if( choice ==1){
            Debug.Log("go to exit");
            //pick up gold
            Gold gold = targets[choice].GetComponent<Gold>();
            
            // if the enemy reaches the gold pile, but there is no gold, rage against the player
            if(gold.gold==0){
                target=targets[0].transform;
                return;
            }
            gold.Take(1);
            goldHeld++;
            // set the target so the agent can run to exit (off screen)
            target=exit.transform;
        }

    }
}
