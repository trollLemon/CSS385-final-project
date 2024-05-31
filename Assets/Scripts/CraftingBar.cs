using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingBar : MonoBehaviour
{

    public Slider craftingBar;

    public Items items;

    // Start is called before the first frame update
    void Start()
    {
        craftingBar = GetComponent<Slider>();
        items = GameObject.Find("Player").GetComponent<Items>();
        craftingBar.maxValue = items.craftingTime;
        craftingBar.value = 0;
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        craftingBar.value = items.currentCraftingTime;
    }
}
