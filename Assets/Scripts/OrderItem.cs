using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderItem : MonoBehaviour
{
    public GameObject ConfirmPage;
    public GameObject star1Full, star2Full, star3Full, star4Full, star5Full;
    bool clickOnce;
    public string quantity;
    public string size;
    public string style;
    public string orderID;
    public string orderDate;
    public string orderStatus;
    public Text sizeText;
    public Text styleText;
    public Text orderDateText;
    public Text orderStatusText;
    public Text quantityText;
    Queries queries;
    GameObject appController;
    UserInfo userinfo;
    float newRating;
    public GameObject confirmButton;
    public AccountPage accountpage;
    public GameObject accountPage;
    // Start is called before the first frame update
    void Start()
    {
        size = GetComponent<SetPaintingDisplayInfo>().size;
        style = GetComponent<SetPaintingDisplayInfo>().style;
        sizeText.text = size;
        styleText.text = style;
        quantityText.text = quantity;
        if (orderDateText != null)
        {
            orderDateText.text = "Order Date: " + orderDate;
        }
       
        orderStatusText.text = "Order Status: " + orderStatus;
        appController = GameObject.FindWithTag("AppController");
        accountPage = GameObject.FindWithTag("ACCOUNT_PAGE");
        accountpage = accountPage.GetComponent<AccountPage>();
        queries = appController.GetComponent<Queries>();
        userinfo = appController.GetComponent<UserInfo>();
        if (orderStatus == "confirmed")
        {
            confirmButton.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
       

    }
    public void Star1()
    {
        clickOnce = false;
        if (star1Full.activeInHierarchy)
        {
            clickOnce = true;
            star1Full.SetActive(false);
            star2Full.SetActive(false);
            star3Full.SetActive(false);
            star4Full.SetActive(false);
            star5Full.SetActive(false);

        }
        if (star1Full.activeInHierarchy == false && clickOnce == false)
        {
            star1Full.SetActive(true);
        }


    }

    public void Star2()
    {
        clickOnce = false;
        if (star2Full.activeInHierarchy)
        {
            clickOnce = true;
            star2Full.SetActive(false);
            star3Full.SetActive(false);
            star4Full.SetActive(false);
            star5Full.SetActive(false);

        }
        if (star2Full.activeInHierarchy == false && clickOnce == false)
        {
            star1Full.SetActive(true);
            star2Full.SetActive(true);
        }


    }

    public void Star3()
    {
        clickOnce = false;
        if (star3Full.activeInHierarchy)
        {
            clickOnce = true;
            star3Full.SetActive(false);
            star4Full.SetActive(false);
            star5Full.SetActive(false);

        }
        if (star3Full.activeInHierarchy == false && clickOnce == false)
        {
            star1Full.SetActive(true);
            star2Full.SetActive(true);
            star3Full.SetActive(true);
        }


    }

    public void Star4()
    {
        clickOnce = false;
        if (star4Full.activeInHierarchy)
        {
            clickOnce = true;
            star4Full.SetActive(false);
            star5Full.SetActive(false);

        }
        if (star4Full.activeInHierarchy == false && clickOnce == false)
        {
            star1Full.SetActive(true);
            star2Full.SetActive(true);
            star3Full.SetActive(true);
            star4Full.SetActive(true);
        }


    }

    public void Star5()
    {
        clickOnce = false;
        if (star5Full.activeInHierarchy)
        {
            clickOnce = true;
            star5Full.SetActive(false);

        }
        if (star5Full.activeInHierarchy == false && clickOnce == false)
        {
            star1Full.SetActive(true);
            star2Full.SetActive(true);
            star3Full.SetActive(true);
            star4Full.SetActive(true);
            star5Full.SetActive(true);
        }


    }
    public void ConfirmArrival()
    {
        OpenRatingPage();
    }

    public void OpenRatingPage()
    {
        ConfirmPage.SetActive(true);
    }
    public void CloseRatingPage()
    {
        ConfirmPage.SetActive(false);
    }
    public void ConfirmRating()
    {
        if (star1Full.activeInHierarchy == false)
        {
            newRating = 0;
        }

        if (star1Full.activeInHierarchy && star2Full.activeInHierarchy == false)
        {
            newRating = 1;
        }
        if (star2Full.activeInHierarchy && star3Full.activeInHierarchy == false)
        {
            newRating = 2;
        }
        if (star3Full.activeInHierarchy && star4Full.activeInHierarchy == false)
        {
            newRating = 3;
        }
        if (star4Full.activeInHierarchy && star5Full.activeInHierarchy == false)
        {
            newRating = 4;
        }
        if (star5Full.activeInHierarchy)
        {
            newRating = 5;
        }
      
        CallGetQueryResultUpdateOrder(queries.WriteQuery_UPDATEORDER(orderID));
    }

    public void CallGetQueryResultUpdateOrder(string query)
    {
        Coroutine coroutine = StartCoroutine(queries.GetQueryResult(query, result =>
        {
            CallGetQueryResultRating(queries.WriteQuery_RATEPAINTINGADD(userinfo.userID, GetComponent<SetPaintingDisplayInfo>().pID, newRating));
        }));
    }

    public void CallGetQueryResultRating(string query)
    {
        Coroutine coroutine = StartCoroutine(queries.GetQueryResult(query, result =>
        {
            CallGetQueryResultUpdateRating(queries.WriteQuery_UPDATEPAINTINGRATING(GetComponent<SetPaintingDisplayInfo>().pID));
        }));
    }

    public void CallGetQueryResultUpdateRating(string query)
    {
        Coroutine coroutine = StartCoroutine(queries.GetQueryResult(query, result =>
        {
            CloseRatingPage();
            accountpage.OpenOrders();
        }));
    }
}
