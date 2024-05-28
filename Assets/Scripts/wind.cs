using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wind : MonoBehaviour
{
    public GameObject windPrefab;
    public GameObject PlayBoundary;

    public float spawnTimer;

    // Start is called before the first frame update
    void Start()
    {
        spawnTimer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnTimer > 0f) {
            spawnTimer -= Time.deltaTime;
        } else {
            spawnWind();
            spawnTimer = Random.Range(30f, 60f);
        }

    }

    void spawnWind() {

        // varied spawn location within playarea bounds (i hope)
        BoxCollider2D boundaryCol = PlayBoundary.GetComponent<BoxCollider2D>();
        Bounds playarea = boundaryCol.bounds;
        Vector2 temp = new Vector2(Random.Range(playarea.min.x, playarea.max.x), Random.Range(playarea.min.y, playarea.max.y));
        GameObject thisWind = Instantiate(windPrefab, temp, transform.rotation);
        temp = new Vector2(Random.Range(playarea.min.x, playarea.max.x), Random.Range(playarea.min.y, playarea.max.y));
        GameObject thisWind2 = Instantiate(windPrefab, temp, transform.rotation);
        
        // slight variation in size
        float randomScale = Random.Range(0.5f, 1.5f);
        thisWind.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
        randomScale = Random.Range(1.5f, 2.5f);
        thisWind2.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
        
        // get anim clip length
        AnimatorClipInfo[] windclip = thisWind.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0);
        float animTime = windclip[0].clip.length;
        // auto destroy object on animation completion
        Destroy(thisWind, animTime);
        Destroy(thisWind2, animTime);

    }
}
