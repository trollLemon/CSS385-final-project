using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Gold : MonoBehaviour
{


    public GameObject goldText;
    public TMP_Text text;
    public int gold;

    // Start is called before the first frame update
    void Start()
    {
        
        //text = goldText.GetComponent<TMP_Text>();
        text.text=""+gold;
    }


    public void Take(int amount){
            gold-=amount;
    }
    public void Return(int amount){
            gold+=amount;
    }
    

    // Update is called once per frame
    void Update()
    {
        text.text=""+gold;
    }
}
