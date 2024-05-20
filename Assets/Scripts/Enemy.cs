using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    
    public Animator animator;
   // public Animator eye_animator;

    [SerializeField] Transform target;

    public GameObject[] targets;
    public GameObject exit;
    private int choice;
    public int goldHeld=0;

    public int health = 50;
    
    public bool attacking = false;
    NavMeshAgent agent;

    private SpriteRenderer spriteRenderer;
    // Original color of the sprite
    private Color originalColor;

    // Start is called before the first frame update
    void Start()
    {

    spriteRenderer = GetComponent<SpriteRenderer>();
        // Store the original color of the sprite
    if (spriteRenderer != null)
    {
        originalColor = spriteRenderer.color;
    }

    
     targets = new GameObject[3];
     
     targets[0] = GameObject.Find("Player");
     targets[1] = GameObject.Find("GoldPile");
     targets[2] = GameObject.FindWithTag("wall");
     exit = GameObject.Find("Exit");
     choice = Random.Range(0, 3);
     animator = GetComponent<Animator>();
     
     if(targets[choice]== null){
        target = targets[0].transform;
     }
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

        if(target==null){
            target = targets[0].transform;
            choice=0;
        } 
        agent.SetDestination(target.position);

        if(Vector2.Distance(transform.position, target.position)<4f){
            PerformNextAction();
        }

        animator.SetBool("isAttacking", attacking);
        //eye_animator.SetBool("isAttacking", attacking);
        Vector3 currentPosition = transform.position;
        currentPosition.z = currentPosition.y*0.01f;
        transform.position=currentPosition;
    }


    private void Die(){
        
        //TODO:Drop stuff: gold, crafting mats
        Destroy(gameObject);
    }

    public void Damage(int dmg){
        health-=dmg;
        StartCoroutine(ChangeColorTemporarily());
        if(health<=0) Die();
    }

    private IEnumerator ChangeColorTemporarily()
    {
        if (spriteRenderer != null)
        {
            // Change the color to red
            spriteRenderer.color = Color.red;
            // Wait for 1 second
            yield return new WaitForSeconds(1f);
            // Change the color back to the original color
            spriteRenderer.color = originalColor;
        }
    }

    void PerformNextAction(){

   

        // if we are at an exit, we should destory the object, this will only happen if the enemy was going to the exit
        // so we do not need to worry about if this statement executes when the enemy attacks the player. 

        if(Vector3.Distance(transform.position, exit.transform.position)<2f){
            Die();
            return;
        }
       
    
        if(choice==0){
                
                if(!attacking){
               
                Health hp = targets[choice].GetComponent<Health>();
                attacking=true;
                StartCoroutine(AttackPlayer(hp));
               
                }
        } 
        
        if( choice ==1){
         
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
             agent.speed = 2f;
        }

        if(choice == 2){
                
                //start breaking the wall
                
                if(!attacking){
                BarrierHealth bh = targets[choice].GetComponent<BarrierHealth>();
                attacking=true;
                StartCoroutine(AttackWall(bh, targets[choice]));
               
                }

        }

    }

    IEnumerator AttackPlayer(Health hp)
    {
       
        while (true)
        {
         
            
            if (Vector2.Distance(transform.position, target.transform.position)>4f) {
                attacking=false;
                break;
            }    
            Debug.Log( Vector3.Distance(transform.position, target.transform.position)) ;     
            Debug.Log("Damaging"); 
            hp.DamagePlayer(10);
            yield return new WaitForSeconds(2);
            
        }
    }

    IEnumerator AttackWall(BarrierHealth bh, GameObject targ)
    {
       
        while (true)
        {
            yield return new WaitForSeconds(2);
            // the barrier has been destroyed, attack the player next
            if(targ==null){
                attacking=false;
                target = targets[0].transform;
                choice=0;
                break;
            }    
            bh.Damage(10);
            
        }
    }
}
