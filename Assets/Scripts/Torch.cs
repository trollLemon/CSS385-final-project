using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Torch : MonoBehaviour
{

    public Light2D gameLight;
    public GameObject fire;
    public float intensity;
    public float outer_rad;
    public float decay = 0.0001f;
    public float rad_decay = 0.001f;
    public float ocil = 40;
    // Start is called before the first frame update
    

    private float _getIntensityFromTimeStep(float time, float intensity)
    {   

        float delta = Mathf.Sin(ocil*Mathf.Sin(ocil*time)) * decay*10;
        intensity += delta;
        return intensity;
    }
    
    void Start()
    {
    gameLight= GetComponent<Light2D>(); 
    fire = transform.parent.Find("torch_fire_sheet_0").gameObject;
    fire.GetComponent<SpriteRenderer>().sortingOrder += 1;
    intensity=gameLight.intensity;


    }

    // Update is called once per frame
    void Update()
    {


        if(intensity > 0.0) intensity = _getIntensityFromTimeStep(Time.time, intensity);
        intensity-=decay;
        intensity = Mathf.Max(0,intensity);
        gameLight.intensity=intensity;
        fire.transform.localScale = new Vector3(intensity, intensity, intensity);
        if(intensity ==0){
            Destroy(gameObject.transform.parent.gameObject);
            Destroy(gameObject);
        }
     

        
        
    }
}
