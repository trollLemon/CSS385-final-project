using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootPrintController : MonoBehaviour
{
    public float duration;

    private float instantiationTime;
    private float currentTime;
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        instantiationTime = Time.time;
        currentTime = instantiationTime;

        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTime - instantiationTime < duration)
        {
            currentTime = Time.time;
            Color color = spriteRenderer.color;
            color.a = (duration - (currentTime - instantiationTime)) / duration;
            spriteRenderer.color = color;
        } else 
        {
            Destroy(gameObject);
        }
    }
}
