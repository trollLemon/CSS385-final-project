using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HP : MonoBehaviour
{

    private SpriteRenderer spriteRenderer;
    // Original color of the sprite
    private Color originalColor;

    public float hp;
    public int minItems;
    public float spawnOffset;
    public int maxItemSpawns;
    public GameObject[] items; //items to drop on descruction

    // Start is called before the first frame update
    void Start()
    {
    spriteRenderer = GetComponent<SpriteRenderer>();
        // Store the original color of the sprite
    if (spriteRenderer != null)
    {
        originalColor = spriteRenderer.color;
    }
    }



    void DropItems()
    {
        int numItemTypesToSpawn = Random.Range(minItems, items.Length+1);

        for(int i = 0; i<numItemTypesToSpawn; ++i)
        {
            int numItemSpawns = Random.Range(minItems, maxItemSpawns+1);
            for(int j = 0; j<numItemSpawns; ++j)
            {
                GameObject item = items[i];
                Vector3 spawnPosition = transform.position;
                float randomOffsetX = Random.Range(-spawnOffset, spawnOffset);
                float randomOffsetY = Random.Range(-spawnOffset, spawnOffset);
                spawnPosition.x+= randomOffsetX;
                spawnPosition.y+=randomOffsetY;
                spawnPosition.z=spawnPosition.y;
                Debug.Log(spawnPosition);
                Instantiate(item, spawnPosition, Quaternion.identity);
            }

        }

    }

    void Die()
    {
        DropItems();
        Destroy(gameObject);
    }

    public void Damage(float dmg)
    {
        hp-=dmg;
        StartCoroutine(ChangeColorTemporarily());
    }

        private IEnumerator ChangeColorTemporarily()
    {
        if (spriteRenderer != null)
        {
            // Change the color to red
            spriteRenderer.color = Color.red;
            // Wait for 1 second
            yield return new WaitForSeconds(0.2f);
            // Change the color back to the original color
            spriteRenderer.color = originalColor;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        if(hp<=0){
            Die();
        }

    }
}
