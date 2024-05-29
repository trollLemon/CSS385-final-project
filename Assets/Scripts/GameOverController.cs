using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class GameOverController : MonoBehaviour
{
    public TMP_Text dayTimerText;
    public TMP_Text gameOverText;
    public Health health;
    public Gold gold;
    public GameManager gameManager;
    private float currentTime;

    // Start is called before the first frame update
    void Start()
    {
        currentTime = Time.time;
        gameOverText.text = "";

        health = GameObject.Find("Player").GetComponent<Health>();
        gold = GameObject.Find("GoldPile").GetComponent<Gold>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
        string dayTimerString = "Day ";

        dayTimerText.text = dayTimerString + (Mathf.Floor(currentTime / (gameManager.DayDuration + gameManager.NightDuration)) + 1);

        currentTime = Time.time;

        if (health.currHealth <= 0 || gold.gold <= 0)
        {

            gameOverText.text = "Game Over!";
            #if UNITY_EDITOR
                    // Application.Quit() does not work in the editor so
                    // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
                    UnityEditor.EditorApplication.isPlaying = false;
            #else
                        Application.Quit();
            #endif
        }
    }
}
