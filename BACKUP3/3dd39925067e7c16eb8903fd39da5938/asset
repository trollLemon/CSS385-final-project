using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class InventoryManager : MonoBehaviour
{

    public GameObject Torch;
    public GameObject Coal;
    public GameObject Stick;
    public GameObject Logs;
   


    public GameObject Hand;
    public GameObject Barrier;
    public GameObject Axe; 
    public GameObject Selected;

    public int torches;
    public int coal;
    public int sticks;
    
    public int logs;

    public int barriers;

    public int MAXITEMS = 10;

    private TMP_Text torch_text;
    private TMP_Text coal_text;
    private TMP_Text stick_text;

    private TMP_Text log_text;

    private TMP_Text barrier_text;

    public GameObject[] items;
    public int selectedItem;
    public int numItems;

    // Start is called before the first frame update
    void Start()
    {
        Torch    = GameObject.Find("TorchIcon");
        Coal     = GameObject.Find("CoalIcon");
        Stick    = GameObject.Find("StickIcon");
        Selected = GameObject.Find("Selected");
        Logs     = GameObject.Find("LogsIcon");
        Barrier  = GameObject.Find("BarrierIcon");

        torch_text   = GameObject.Find("TorchNum").GetComponent<TMP_Text>();
        coal_text    = GameObject.Find("CoalNum").GetComponent<TMP_Text>();
        stick_text   = GameObject.Find("StickNum").GetComponent<TMP_Text>();
        log_text     = GameObject.Find("LogNum").GetComponent<TMP_Text>();
        barrier_text = GameObject.Find("BarrierNum").GetComponent<TMP_Text>();

        Hand = GameObject.Find("HandIcon");
        Axe  = GameObject.Find("AxeIcon");

        selectedItem = 0;
        items = new GameObject[4];
        items[0] = Hand;
        items[1] = Axe;
        items[2] = Torch;
        items[3] = Barrier;
        numItems = items.Length;

    }

    // Update is called once per frame
    void Update()
    {
        PinSelected();
        UpdateItemNum();
        DimEmptyItems();
    }

    
    void PinSelected()
    {
        Selected.transform.position = items[selectedItem].transform.position;
    }

    void UpdateItemNum()
    {

        torch_text.text   =""+torches;
        coal_text.text    =""+coal;
        stick_text.text   = ""+sticks;
        log_text.text     = ""+logs;
        barrier_text.text = ""+barriers;


    }
    void DimEmptyItems()
    {
        Image image = Torch.GetComponent<Image>();
        _dimEmpty(image, torches);
        image = Coal.GetComponent<Image>();
        _dimEmpty(image, coal);
        image = Stick.GetComponent<Image>();
        _dimEmpty(image, sticks);
        image = Logs.GetComponent<Image>();
        _dimEmpty(image, logs);
        image = Barrier.GetComponent<Image>();
        _dimEmpty(image, barriers);

    }

    void _dimEmpty(Image image, int count){
        Color currentColor = image.color;
        if(count<=0){
            currentColor.a = 0.5f;
        } 
        else{
             currentColor.a = 1f;
        }
        image.color = currentColor;
    }


    public void switchItems(){

            selectedItem = (selectedItem++) % numItems;

    }

    public void UseTorch(){
        torches--;
    }
   public void UseBarrier(){
        barriers--;
    }

    public int CraftBarrier()
    {
        if(barriers == MAXITEMS) return 1;
        if(logs <=0) return 1;
        barriers++;
        UseLog();
        return 0;
    }

    public int CraftTorch()
    {
        if(torches == MAXITEMS) return 1;
        if(coal <=0 && sticks <= 0) return 1;
        torches++;
        UseCoal();
        UseSticks();
        return 0;
    }

    public int PickUpCoal()
    {
        if(coal == MAXITEMS) return 1;
        coal++;
        return 0;
    }
    public int PickUpStick()
    {
        if(sticks == MAXITEMS) return 1;
        sticks++;
        return 0;
    }

   public int PickUpLog()
    {
        if(logs == MAXITEMS) return 1;
        logs++;
        return 0;
    }

    void UseCoal()
    {
        coal--;
    }
    void UseSticks()
    {
        sticks--;
    }
    void UseLog()
    {
        logs--;
    }

}
