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

    bool endOfCycle = true;
    // Start is called before the first frame update
    void Start()
    {
        gameState=GameState.Day;
        GlobalLight = GameObject.Find("GlobalLight").GetComponent<Light2D>();
        //StartCoroutine(NightCycle());

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
        float startIntensity = 1f;
        float endIntensity = 0.004f;
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
        float startIntensity = 0.004f;
        float endIntensity = 1.0f;
        GlobalLight.intensity = startIntensity;
        while (elapsedTime < NightDuration)
        {
           
            
            float t = elapsedTime / NightDuration;
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
