using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdminOrder : MonoBehaviour
{
    public GameObject orderscrollParent;
    public GameObject paintingOrderDisplay;
    public Queries queries;
    List<GameObject> paintingOrderList = new List<GameObject>();
    public string userID;
    public InputField userIDInput;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadOrders()
    {
        userID = userIDInput.text;
        foreach (GameObject painting in paintingOrderList)
        {
            Destroy(painting);
        }
        paintingOrderList.Clear();
        GetOrders(queries.WriteQuery_GETORDERS(userID));
    }

    public void UpdateStatus(string orderID, string status)
    {
        CallGetQueryResult(queries.WriteQuery_UPDATEORDER(orderID, status));
    }

    public void DeleteOrder(string orderID)
    {
        CallGetQueryResult(queries.WriteQuery_DELETEORDER(orderID));
    }

    public void GetOrders(string query)
    {
        Coroutine coroutine = StartCoroutine(queries.GetQueryResult(query, result =>
        {

            DisplayOrders(queries.ExtractInfo(result));
        }));
    }

    void DisplayOrders(List<List<string>> feedpaintings)
    {

        for (int i = 0; i < feedpaintings.Count; i++)
        {


            GameObject painting = Instantiate(paintingOrderDisplay, transform.position, transform.rotation);
            painting.transform.parent = orderscrollParent.transform;
            painting.GetComponent<OrderItem>().orderID = feedpaintings[i][0];
            painting.GetComponent<SetPaintingDisplayInfo>().pID = feedpaintings[i][1];
            painting.GetComponent<OrderItem>().orderDate = feedpaintings[i][2];
            painting.GetComponent<OrderItem>().orderStatus = feedpaintings[i][3];
            painting.GetComponent<OrderItem>().quantity = feedpaintings[i][4];
            painting.GetComponent<SetPaintingDisplayInfo>().title = feedpaintings[i][5];
            painting.GetComponent<SetPaintingDisplayInfo>().style = feedpaintings[i][6];
            painting.GetComponent<SetPaintingDisplayInfo>().price = feedpaintings[i][7];
            painting.GetComponent<SetPaintingDisplayInfo>().size = feedpaintings[i][8];

            painting.GetComponent<SetPaintingDisplayInfo>().imageURL = feedpaintings[i][9];
            painting.GetComponent<SetPaintingDisplayInfo>().description = feedpaintings[i][10];
            painting.GetComponent<SetPaintingDisplayInfo>().rating = feedpaintings[i][11];
            painting.GetComponent<SetPaintingDisplayInfo>().artistFName = feedpaintings[i][12];
            painting.GetComponent<SetPaintingDisplayInfo>().artistLName = feedpaintings[i][13];
            painting.GetComponent<SetPaintingDisplayInfo>().creationDate = feedpaintings[i][14];
            painting.GetComponent<SetPaintingDisplayInfo>().uploadDate = feedpaintings[i][15];
            painting.GetComponent<SetPaintingDisplayInfo>().qt = feedpaintings[i][16];
            painting.GetComponent<SetPaintingDisplayInfo>().artistID = feedpaintings[i][17];
            paintingOrderList.Add(painting);



        }




    }

    public void CallGetQueryResult(string query)
    {
        Coroutine coroutine = StartCoroutine(queries.GetQueryResult(query, result =>
        {
            LoadOrders();

        }));
    }


}
