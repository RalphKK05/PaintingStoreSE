using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class ScriptTesting : MonoBehaviour
{

    public List<List<string>> ListOfRows = new List<List<string>>();
    string responseText;

    // Start is called before the first frame update
    void Start()
    {
        //Browsing Page Queries
        string query1 = WriteQuery_SEARCH("'%o%'", 2, 10, 0, 0, "1 YEAR", "IN ('Abstract', 'Renaissance')", "1 YEAR", "price");

        //Personal Feed Page Queries
        //-show paintings according to user preferences
        string query2 = WriteQuery_FEED("5");
        //-show sponsored paintings according to prefs (2 ways??)
        string query = WriteQuery_FEEDPROMOTED("5");


        //Cart Page Queries
        //-look at this user's cart
        string query3 = WriteQuery_LOADCART("5");
        //-delete painting from cart
        string query4 = WriteQuery_DELETEFROMCART("5", "2");
        //-order selected paintings
        string query5 = WriteQuery_ORDERFROMCART("9", "5", "2", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "being prepared");
        string query6 = WriteQuery_DELETEFROMCART("5", "2");

        //Painting Page Queries
        //-Get all painting info
        string query7 = WriteQuery_GETPAINTINGINFO("2");
        //-add this painting to cart
        string query8 = WriteQuery_ADDTOCART("5", "2");
        //-order this painting directly by choosing quantity also
        string query9 = WriteQuery_ORDERFROMCART("9", "5", "2", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "being prepared");
        //-show under the painting similar paintings according to style (More abstract, or More by this artist)
        string query10 = WriteQuery_SIMILARSTYLE("Renaissance");
        string query11 = WriteQuery_SIMILARARTIST("Van Gogh");

        //Account Page Queries
        //-look at list of orders and see their status
        //-Confirm order arrived (deletes it from order list and sends notif to seller that order arrived at buyer).
        //-always check if a specific order in seller store has been confirmed by buyer that it arrived. When it does (status arrived), push notif in seller's phone app that this order was confirmed.
        //-In buyer app also, when his order status is changed by seller as shipping, or in country of destination, receive notif that it changed.
        //-Rate order once it arrives
        //-see account information and change account information

        //User Your Store Page Queries
        //-see uploaded paintings
        //-upload a painting
        //-remove an uploaded painting
        //-promote/sponsor an uploaded painting
        //-See your paintings that have been ordered, change their status, shipping w hek tracking,
        //-Receive notif that painting arrived. When order confirmed by buyer, it gets deleted from order tables and thus doesnt show in seller ordered paintings.

        //Updating Prefs Queries
        string query12 = WriteQuery_UPDATEPREFERENCE("5", "Renaissance", 25);

        //Updating Ratings Queries
        //-When user rates painting X:
        string query13 = WriteQuery_RATEPAINTINGADD("5", "1", 5f);
        string query14 = WriteQuery_RATEPAINTINGADD("6", "1", 7f);
        //Small delay to let time for rating to be filled in rating table before calculating avg. If not, rating will be null in painting table, or not updated at all.
        string query15 = WriteQuery_UPDATEPAINTINGRATING("1");

      
        StartCoroutine(SendQuery(query2));
       
    }

    // Update is called once per frame
    void Update()
    {

    }

    List<List<string>> ExtractInfo(string result)
    {
        string[] rowSeparator = new string[] { "ROW = " };
        char[] elementSeparator = new char[] { '|' };

        // Split the result string into rows
        string[] rows = result.Split(rowSeparator, System.StringSplitOptions.RemoveEmptyEntries);
     
      
        // Create a list to hold the lists of elements
        List<List<string>> elementLists = new List<List<string>>();

        //Loop over the rows and split each one into elements
        foreach (string row in rows)
        {
            string[] elements = row.Split(elementSeparator, System.StringSplitOptions.RemoveEmptyEntries);
            List<string> elementList = new List<string>(elements);
            elementLists.Add(elementList);
        }
        elementLists.RemoveAt(0);

        return elementLists;
        //Can access results of query through: elementLists[rowNumber][columnNumber]
        
       
    
    }

    

    string WriteQuery_SIMILARSTYLE(string style)
    {
        return "SELECT * FROM Painting WHERE style = " + "'" + style + "'" + ";";
    }

    string WriteQuery_SIMILARARTIST(string artist)
    {
        return "SELECT * FROM Painting WHERE ??artist = " + "'" + artist + "'" + ";";
    }
    string WriteQuery_ADDTOCART(string userID, string pID)
    {
        return "INSERT INTO Cart(userID, pID)  VALUES(" + userID + ", " + pID + ");";
    }

    string WriteQuery_GETPAINTINGINFO(string pID)
    {
        return "SELECT * FROM Painting WHERE pID = " + pID + ";";
    }

    string WriteQuery_UPDATEPAINTINGRATING(string pID)
    {
        return "UPDATE Painting SET rating = (SELECT AVG(rating) FROM RatePainting WHERE pID = " + pID + ") WHERE pID = " + pID + ";";
    }
    string WriteQuery_RATEPAINTINGADD(string userID, string pID, float rating)
    {
        return "INSERT INTO RatePainting(userID, pID, rating)  VALUES(" + userID + ", " + pID + ", " + rating + ");";
    }
    string WriteQuery_UPDATEPREFERENCE(string userID, string preference, int incrementValue)
    {
        return "UPDATE Preferences SET prefWeight = prefWeight + " + incrementValue + " WHERE userID = " + userID + " AND preference = " + "'" + preference + "'" + ";";
    }
     

    string WriteQuery_ORDERFROMCART(string orderID, string userID, string pID, string orderDate, string orderStatus)
    {   
        return "INSERT INTO Orders(orderID, userID, pID, orderDate, orderStatus) VALUES("+ orderID + ", " + userID + ", " + pID + ", " + "'" + orderDate + "', " + "'" + orderStatus + "'" + ");";
    }
    
    string WriteQuery_DELETEFROMCART(string userID, string pID)
    {
        return "DELETE FROM Cart WHERE userID = " + userID + " AND pID = " + pID + ";";
    }
    
    string WriteQuery_LOADCART(string userID)
    {
        return "SELECT * FROM Painting WHERE pID = (SELECT pID FROM Cart WHERE userID = " + userID + ");";
    }


    string WriteQuery_SEARCH(string title, float minPrice, float maxPrice, int minQuantity, int minRating, string uploadDateInterval, string styleParameter, string creationDateInterval, string orderParameter)
    {
        return "SELECT * FROM Painting WHERE title LIKE " + title + " AND price >= " + minPrice + " AND price <= " + maxPrice + " AND quantity >= " + minQuantity + " AND rating >= " + minRating + " AND uploadDate >= DATE_SUB(NOW(), INTERVAL " + uploadDateInterval + ") AND style " + styleParameter + " AND creationDate >= DATE_SUB(NOW(), INTERVAL " + creationDateInterval + ") ORDER BY " + orderParameter + ";";
    }

    string WriteQuery_FEED(string userID)
    {
        return "SELECT * FROM Painting p JOIN Preferences pf ON p.style = pf.preference WHERE pf.userID = " + userID + " ORDER BY pf.prefWeight DESC;";
    }
    string WriteQuery_FEEDPROMOTED(string userID)
    {
        return "SELECT Painting.pID FROM Painting JOIN (SELECT preference FROM Preferences WHERE userID = " + userID + " ORDER BY prefWeight DESC LIMIT 3) AS top_preferences ON Painting.style = top_preferences.preference WHERE sponsored = 1;";
    }
    

    IEnumerator SendQuery(string mysqlQuery)
    {       

        // The URL of the PHP file to send data to and receive data from
        string url = "https://online-painting-store.000webhostapp.com/PHPFiles/sendquery.php";

        // Create a form object to hold the data we want to send to the PHP file
        WWWForm form = new WWWForm();
        form.AddField("query", mysqlQuery);

        // Create a UnityWebRequest object to make the POST request
        UnityWebRequest www = UnityWebRequest.Post(url, form);

        // Send the request and wait for a response
        yield return www.SendWebRequest();

        // Check for errors
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Error: " + www.error);
            yield break;
        }

        // Extract the response text
        responseText = www.downloadHandler.text;
      
        // Do something with the response text
        Debug.Log("Response: " + responseText);
    }

    public List<List<string>> GET_PAINTINGS_FEED(string userID)
    {
        string feedpaintings = WriteQuery_FEED(userID);
        StartCoroutine(SendQuery(feedpaintings));
        return ExtractInfo(responseText);


    }
    public List<List<string>> GET_PAINTINGS_FEED_SPONSORED(string userID)
    {
        string sponsoredpaintings = WriteQuery_FEEDPROMOTED(userID);
        StartCoroutine(SendQuery(sponsoredpaintings));
        return ExtractInfo(responseText);


    }
}
