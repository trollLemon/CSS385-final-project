using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class title : MonoBehaviour
{
    public Image logo;
    public Button startButton;

    // Chris Code
    // private SpriteRenderer logoSpriteRenderer;
    Color color;
    private float startTime;

    // Start is called before the first frame update
    void Start()
    {
        startButton.onClick.AddListener(StartGame);
        // StartCoroutine(FadeTitle());


        // Chris Code
        // logoSpriteRenderer = logo.GetComponent<SpriteRenderer>();
        color = logo.color;
        color.a = 0.0f;
        logo.color = color;
        startTime = Time.time;

    }

    // Update is called once per frame
    void Update()
    {
        if (color.a != 1.0f)
        {
            color.a = Time.time - startTime;
            logo.color = color;
        }
        
    }

    IEnumerator FadeTitle() {

        logo.color = new Color(1, 1, 1, logo.color.a + 0.1f);
        yield return new WaitForSeconds(0.1f);

    }

    void StartGame() {
        SceneManager.LoadScene("BetaLevel");
    }
}
