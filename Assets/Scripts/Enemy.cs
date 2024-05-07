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
    
    NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
     targets = new GameObject[2];
     
     targets[0] = GameObject.Find("Player");
     targets[1] = GameObject.Find("Gold");
     choice = Random.Range(0, 2);
     target=targets[choice].transform;
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

        Debug.Log("Choice");
        
        if(transform.position.x==exit.transform.position.x && transform.position.y==exit.transform.position.y){
            Destroy(gameObject);
            return;
        }
        
        if(choice==0){

        } 
        
        if( choice ==1){
            Debug.Log("go to exit");
            //pick up gold

            // set the target so the agent can run to exit (off screen)
            target=exit.transform;
        }

    }
}
