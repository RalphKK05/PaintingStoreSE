using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleManage : MonoBehaviour
{
    public GameObject Feed, Search, Cart, Account;
    public Color normalColor;
    public Color selectedColor;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Feed_Clicked()
    {
        Feed.GetComponent<Image>().color = selectedColor;
        Search.GetComponent<Image>().color = normalColor;
        Cart.GetComponent<Image>().color = normalColor;
        Account.GetComponent<Image>().color = normalColor;

    }
    public void Search_Clicked()
    {
        Feed.GetComponent<Image>().color = normalColor;
        Search.GetComponent<Image>().color = selectedColor;
        Cart.GetComponent<Image>().color = normalColor;
        Account.GetComponent<Image>().color = normalColor;

    }
    public void Cart_Clicked()
    {
        Feed.GetComponent<Image>().color = normalColor;
        Search.GetComponent<Image>().color = normalColor;
        Cart.GetComponent<Image>().color = selectedColor;
        Account.GetComponent<Image>().color = normalColor;

    }
    public void Account_Clicked()
    {
        Feed.GetComponent<Image>().color = normalColor;
        Search.GetComponent<Image>().color = normalColor;
        Cart.GetComponent<Image>().color = normalColor;
        Account.GetComponent<Image>().color = selectedColor;

    }
    // Update is called once per frame
    void Update()
    {
       
    }
}
