using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class title : MonoBehaviour
{
    public Image logo;
    public Button startButton;

    // Start is called before the first frame update
    void Start()
    {
        startButton.onClick.AddListener(StartGame);
        // StartCoroutine(FadeTitle());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator FadeTitle() {

        logo.color = new Color(1, 1, 1, logo.color.a + 0.1f);
        yield return new WaitForSeconds(0.1f);

    }

    void StartGame() {
        SceneManager.LoadScene("BetaLevel");
    }
}
