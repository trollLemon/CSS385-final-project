using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
public class GameManager : MonoBehaviour
{



    public enum GameState{
        Day,
        Night,
        GameOver

    };
    public float DayDuration; 
    public float NightDuration; 

    public Light2D GlobalLight;
    
    public GameState gameState;

    public float nightTimeScale;
    public float startLight;
    bool endOfCycle = true;
    // Start is called before the first frame update
    void Start()
    {
        gameState=GameState.Day;
        GlobalLight = GameObject.Find("GlobalLight").GetComponent<Light2D>();
        //StartCoroutine(NightCycle());
	startLight = GlobalLight.intensity;
    }

    // Update is called once per frame
    void Update()
    {
      
        if(endOfCycle){
            Debug.Log(gameState);
            if(gameState==GameState.Night){
             
                endOfCycle=false;
                
                StartCoroutine(NightCycle());
            }
            if(gameState == GameState.Day)
            {
                
                endOfCycle=false;
                
                StartCoroutine(DayCycle());
            }

        }
    }

    IEnumerator DayCycle()
    {
        float elapsedTime = 0f;
        float startIntensity = startLight;
        float endIntensity = 0f;
        GlobalLight.intensity = startIntensity;
        while (elapsedTime < DayDuration)
        {
            float t = elapsedTime / DayDuration;
            GlobalLight.intensity = Mathf.Lerp(startIntensity, endIntensity, t);
            elapsedTime += Time.deltaTime;
        
            yield return null;
        
        }

        // Ensure final intensity is set to end value
        GlobalLight.intensity = endIntensity;
        gameState = GameState.Night;
        endOfCycle = true;
        Debug.Log("End Day");
        
    }

    IEnumerator NightCycle()
    {
        float elapsedTime = 0f;
        float startIntensity = 0f;
        float endIntensity = startLight;
        GlobalLight.intensity = startIntensity;
        while (elapsedTime < NightDuration)
        {
           
            
            float t = elapsedTime / NightDuration * nightTimeScale;
            GlobalLight.intensity = Mathf.Lerp(startIntensity, endIntensity, t);
          
            elapsedTime += Time.deltaTime;
            yield return null;
       
        }

        // Ensure final intensity is set to end value
        GlobalLight.intensity = endIntensity;
        gameState = GameState.Day;
        endOfCycle = true;
        Debug.Log("End Night");
    }

}
