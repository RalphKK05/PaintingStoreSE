using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CartPage : MonoBehaviour
{
    public Button checkoutButton;
    public Queries queries;
    public UserInfo userinfo;
    public Toggle AllToggle;
    public GameObject CartEmpty;
    public GameObject OrderPopUp;
    public GameObject cartscrollParent;
    public GameObject orderscrollParent;
    public GameObject paintingDisplay, paintingOrderDisplay;
    public float totalprice;
    public Text totalPriceText;
    public Text totalPriceOrderText;
    public List<GameObject> cartItemList = new List<GameObject>();
    public List<GameObject> orderList = new List<GameObject>();
    public List<GameObject> finalOrderList = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
       
    }

    public void CloseCartTopPage()
    {
        this.gameObject.SetActive(false);
    }
    public void LoadCart()
    {
        foreach (GameObject cartitem in cartItemList)
        {
            Destroy(cartitem);
        }
        cartItemList.Clear();

        CallGetQueryResult(queries.WriteQuery_LOADCART(userinfo.userID));
    }

    public void AllToggleChanged()
    {
        if (AllToggle.isOn)
        {
            foreach (GameObject cartitem in cartItemList)
            {
                cartitem.GetComponent<CartItem>().toggle.isOn = true;

            }
        }
        if (AllToggle.isOn == false)
        {
            foreach (GameObject cartitem in cartItemList)
            {
                cartitem.GetComponent<CartItem>().toggle.isOn = false;

            }
        }
    }
    // Update is called once per frame
    void Update()
    {
      
        
        totalprice = 0;
        foreach (GameObject cartitem in cartItemList)
        {
            if (cartitem.GetComponent<CartItem>().toggle.isOn)
            {
                totalprice += float.Parse(cartitem.GetComponent<SetPaintingDisplayInfo>().price);
                
            }
        }

        totalPriceText.text = "$" + totalprice.ToString();
        totalPriceOrderText.text = "$" + totalprice.ToString();
    }

    
    public void DeleteItems()
    {
        foreach (GameObject cartitem in cartItemList)
        {
            if (cartitem.GetComponent<CartItem>().toggle.isOn)
            {
                CallGetQueryDeleteItem(queries.WriteQuery_DELETEFROMCART(userinfo.userID, cartitem.GetComponent<SetPaintingDisplayInfo>().pID));
            }
        }
        
    }

    public void CheckOutItems()
    {
        foreach (GameObject orderlist in orderList)
        {
            //Destroy(orderlist);
        }
        orderList.Clear();

        foreach (GameObject orderlist in finalOrderList)
        {
            Destroy(orderlist);
        }
        finalOrderList.Clear();

        OrderPopUp.SetActive(true);
        foreach (GameObject cartitem in cartItemList)
        {
            if (cartitem.GetComponent<CartItem>().toggle.isOn)
            {
                orderList.Add(cartitem);
                
            }
        }

        foreach (GameObject orderlist in orderList)
        {
            
            GameObject painting = Instantiate(paintingOrderDisplay, transform.position, transform.rotation);
            painting.transform.parent = orderscrollParent.transform;
            painting.GetComponent<SetPaintingDisplayInfo>().pID = orderlist.GetComponent<SetPaintingDisplayInfo>().pID;
            painting.GetComponent<SetPaintingDisplayInfo>().title = orderlist.GetComponent<SetPaintingDisplayInfo>().title;
            painting.GetComponent<SetPaintingDisplayInfo>().price = orderlist.GetComponent<SetPaintingDisplayInfo>().price;
            painting.GetComponent<SetPaintingDisplayInfo>().imageURL = orderlist.GetComponent<SetPaintingDisplayInfo>().imageURL;
            painting.GetComponent<SetPaintingDisplayInfo>().description = orderlist.GetComponent<SetPaintingDisplayInfo>().description;
            painting.GetComponent<SetPaintingDisplayInfo>().rating = orderlist.GetComponent<SetPaintingDisplayInfo>().rating;
            painting.GetComponent<SetPaintingDisplayInfo>().size = orderlist.GetComponent<SetPaintingDisplayInfo>().size;
            painting.GetComponent<SetPaintingDisplayInfo>().style = orderlist.GetComponent<SetPaintingDisplayInfo>().style;
            painting.GetComponent<SetPaintingDisplayInfo>().artistFName = orderlist.GetComponent<SetPaintingDisplayInfo>().artistFName;
            painting.GetComponent<SetPaintingDisplayInfo>().artistLName = orderlist.GetComponent<SetPaintingDisplayInfo>().artistLName;
            painting.GetComponent<SetPaintingDisplayInfo>().creationDate = orderlist.GetComponent<SetPaintingDisplayInfo>().creationDate;
            painting.GetComponent<SetPaintingDisplayInfo>().uploadDate = orderlist.GetComponent<SetPaintingDisplayInfo>().uploadDate;
            painting.GetComponent<OrderCartItem>().size = orderlist.GetComponent<SetPaintingDisplayInfo>().size;
            painting.GetComponent<OrderCartItem>().style = orderlist.GetComponent<SetPaintingDisplayInfo>().style;
            painting.GetComponent<OrderCartItem>().quantity = orderlist.GetComponent<CartItem>().counterValue.ToString();
            finalOrderList.Add(painting);

        }

        
    }

    public void ConfirmOrder()
    {
        foreach (GameObject orderlist in finalOrderList)
        {
            CallGetQueryResultADDORDER(queries.WriteQuery_ORDERFROMCART(userinfo.userID, orderlist.GetComponent<SetPaintingDisplayInfo>().pID, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "being prepared", orderlist.GetComponent<OrderCartItem>().quantity));
        }
        OrderPopUp.SetActive(false);

        
    }
    void DisplayPaintings(List<List<string>> feedpaintings)
    {

        for (int i = 0; i < feedpaintings.Count; i++)
        {
         
            GameObject painting = Instantiate(paintingDisplay, transform.position, transform.rotation);
            painting.transform.parent = cartscrollParent.transform;
            painting.GetComponent<SetPaintingDisplayInfo>().pID = feedpaintings[i][0];
            painting.GetComponent<SetPaintingDisplayInfo>().title = feedpaintings[i][1];
            painting.GetComponent<SetPaintingDisplayInfo>().price = feedpaintings[i][2];
            painting.GetComponent<SetPaintingDisplayInfo>().imageURL = feedpaintings[i][3];
            painting.GetComponent<SetPaintingDisplayInfo>().description = feedpaintings[i][4];
            painting.GetComponent<SetPaintingDisplayInfo>().rating = feedpaintings[i][5];
            painting.GetComponent<SetPaintingDisplayInfo>().size = feedpaintings[i][6];
            painting.GetComponent<SetPaintingDisplayInfo>().style = feedpaintings[i][7];
            painting.GetComponent<SetPaintingDisplayInfo>().artistFName = feedpaintings[i][8];
            painting.GetComponent<SetPaintingDisplayInfo>().artistLName = feedpaintings[i][9];

            painting.GetComponent<SetPaintingDisplayInfo>().creationDate = feedpaintings[i][10];
            painting.GetComponent<SetPaintingDisplayInfo>().uploadDate = feedpaintings[i][11];
            painting.GetComponent<SetPaintingDisplayInfo>().qt = feedpaintings[i][12];
            painting.GetComponent<SetPaintingDisplayInfo>().artistID = feedpaintings[i][13];

            cartItemList.Add(painting);



        }



    }

    void DisplayPaintingsOrders(List<List<string>> feedpaintings)
    {

        for (int i = 0; i < feedpaintings.Count; i++)
        {

            GameObject painting = Instantiate(paintingOrderDisplay, transform.position, transform.rotation);
            painting.transform.parent = orderscrollParent.transform;
            painting.GetComponent<SetPaintingDisplayInfo>().pID = feedpaintings[i][0];
            painting.GetComponent<SetPaintingDisplayInfo>().title = feedpaintings[i][1];
            painting.GetComponent<SetPaintingDisplayInfo>().price = feedpaintings[i][2];
            painting.GetComponent<SetPaintingDisplayInfo>().imageURL = feedpaintings[i][3];
            painting.GetComponent<SetPaintingDisplayInfo>().description = feedpaintings[i][4];
            painting.GetComponent<SetPaintingDisplayInfo>().rating = feedpaintings[i][5];
            painting.GetComponent<SetPaintingDisplayInfo>().size = feedpaintings[i][6];
            painting.GetComponent<SetPaintingDisplayInfo>().style = feedpaintings[i][7];
            painting.GetComponent<SetPaintingDisplayInfo>().artistFName = feedpaintings[i][8];
            painting.GetComponent<SetPaintingDisplayInfo>().artistLName = feedpaintings[i][9];

            painting.GetComponent<SetPaintingDisplayInfo>().creationDate = feedpaintings[i][10];
            painting.GetComponent<SetPaintingDisplayInfo>().uploadDate = feedpaintings[i][11];





        }



    }
    public void CallGetQueryDeleteItem(string query)
    {
        Coroutine coroutine = StartCoroutine(queries.GetQueryResult(query, result =>
        {
            Invoke("LoadCart", 0.5f);
        }));
    }
    public void CallGetQueryResult(string query)
    {
        Coroutine coroutine = StartCoroutine(queries.GetQueryResult(query, result =>
        {
            if (result == "0 results")
            {
                CartEmpty.SetActive(true);
                checkoutButton.interactable = false;
            }
            else
            {
                CartEmpty.SetActive(false);
                DisplayPaintings(queries.ExtractInfo(result));
                checkoutButton.interactable = true;

            }
            

          
        }));
    }
    public void CallGetQueryResultADDORDER(string query)
    {
        Coroutine coroutine = StartCoroutine(queries.GetQueryResult(query, result =>
        {

        }));
    }
}
