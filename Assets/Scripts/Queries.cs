using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;

public class Queries : MonoBehaviour
{

    public List<List<string>> ListOfRows = new List<List<string>>();
    public string responseText;
    public Sprite responseSprite;
    public List<List<string>> responseList = new List<List<string>>();
    public bool queryDone, getImageDone;
    int queryStatus;
    public GameObject errorPage, loadingPage;

    // Start is called before the first frame update
    void Start()
    {
     
        

    }

    // Update is called once per frame
    void Update()
    {
        if (queryStatus == 1)
        {
            loadingPage.SetActive(true);
        }
        if (queryStatus == 0)
        {
            loadingPage.SetActive(false);
        }
        if (queryStatus == 2)
        {
            loadingPage.SetActive(false);
            errorPage.SetActive(true);
            Invoke("DisableError", 1f);
        }
    }
    void DisableError()
    {
        errorPage.SetActive(false);
        queryStatus = 0;
    }

    public List<List<string>> ExtractInfo(string result)
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
       
            //elementLists.RemoveAt(0);
            for (int i = 0; i <elementLists.Count; i++)
            {
                for (int j = 0; j < elementLists[i].Count; j++)
                {
                    elementLists[i][j] = elementLists[i][j].Trim();
                }
                
            }
      
        
        return elementLists;
        //Can access results of query through: elementLists[rowNumber][columnNumber]



    }



  
    

    string WriteQuery_GETPAINTINGINFO(string pID)
    {
        return "SELECT * FROM Painting WHERE pID = " + pID + ";";
    }



    public string WriteQuery_ADDTOCART(string userID, string pID)
    {
        return "INSERT INTO Cart(userID, pID)  VALUES(" + userID + ", " + pID + ");";
    }

    public string WriteQuery_ORDERFROMCART(string userID, string pID, string orderDate, string orderStatus, string quantity)
    {
        return "INSERT INTO Orders(userID, pID, orderDate, orderStatus, quantity) VALUES(" + userID + ", " + pID + ", " + "'" + orderDate + "', " + "'" + orderStatus + "', '" + quantity + "' );";
    }

    public string WriteQuery_DELETEFROMCART(string userID, string pID)
    {
        return "DELETE FROM Cart WHERE userID = " + userID + " AND pID = " + pID + ";";
    }



    public string WriteQuery_LOADCART(string userID)
    {
        return "SELECT pID, title, price, image, description, rating, size, style, artist_FName, artist_LName, creationDate, uploadDate, quantity, artistID FROM Painting WHERE pID IN (SELECT pID FROM Cart WHERE userID = " + userID + ");";
    }


   

    public string WriteQuery_FEED(string userID)
    {
        return "SELECT pID, title, price, image, description, rating, size, style, artist_FName, artist_LName, creationDate, uploadDate, quantity, artistID FROM Painting p JOIN Preferences pf ON p.style = pf.preference WHERE pf.userID = " + userID + " ORDER BY pf.prefWeight DESC;";
    }
  
    public string WriteQuery_FEEDPROMOTED(string userID)
    {
        return "SELECT Painting.pID, Painting.title, Painting.price, Painting.image, Painting.description, Painting.rating, Painting.size, Painting.style, Painting.artist_FName, Painting.artist_LName, Painting.creationDate, Painting.uploadDate, Painting.quantity, Painting.artistID FROM Painting JOIN (SELECT preference FROM Preferences WHERE userID = " + userID + " ORDER BY prefWeight DESC LIMIT 3) AS top_preferences ON Painting.style = top_preferences.preference WHERE sponsored = 1 ORDER BY RAND();";
    }

    public string WriteQuery_GETALLUSERNAMES()
    {
        return "SELECT username FROM User;";
    }

    public string WriteQuery_GETALLUSERIDS()
    {
        return "SELECT userID FROM User;";
    }
    public string WriteQuery_GETUSERINFO(string username)
    {
        return "SELECT userID, username, fName, lName, birthDate, email, password, admin FROM User WHERE username = '" + username + "';";
    }

    public string WriteQuery_GETUSERPASSWORD(string username)
    {
        return "SELECT password FROM User WHERE username = '" + username + "';";
    }

    public string WriteQuery_ADDUSER(string username, string fName, string lName, string birthDate, string email, string password)
    {
        return "INSERT INTO User (username, fName, lName, birthDate, email, password, admin) VALUES('" + username + "', '" + fName + "', '" + lName + "', DATE('" + birthDate+ "'), '" + email + "', '" + password + "', '0');"; 
    }
    public string WriteQuery_ADDPREFERENCE(string userID, string preference, string prefWeight)
    {
        return "INSERT INTO Preferences (userID, preference, prefWeight) VALUES(" + userID + ", '" + preference + "', '" + prefWeight + "');";
    }

    public string WriteQuery_SIMILARSTYLE(string style, string currentPID)
    {
        return "SELECT pID, title, price, image, description, rating, size, style, artist_FName, artist_LName FROM Painting WHERE style = " + "'" + style + "'" + " AND pID != '" + currentPID + "' ;";
    }

    public string WriteQuery_SIMILARARTIST(string artistFName, string artistLName, string currentPID, string artistID)
    {
        return "SELECT pID, title, price, image, description, rating, size, style, artist_FName, artist_LName FROM Painting WHERE artistID = " + "'" + artistID + "' AND pID != '" + currentPID + "' ;";
    }

    public string WriteQuery_UPDATEPREFERENCE(string userID, string preference, int incrementValue)
    {
        return "UPDATE Preferences SET prefWeight = prefWeight + " + incrementValue + " WHERE userID = " + userID + " AND preference = " + "'" + preference + "'" + ";";
    }

    public string WriteQuery_SEARCH(string title, string minPrice, string maxPrice, string minRating, string uploadDateInterval, string styleParameter, string creationDateInterval, string artistParameter, string orderParameter)
    {
        return "SELECT pID, title, price, image, description, rating, size, style, artist_fName, artist_lName, creationDate, uploadDate, quantity, artistID FROM Painting WHERE title LIKE " + title + " AND price >= " + minPrice + " AND price <= " + maxPrice + " AND rating >= " + minRating + creationDateInterval + uploadDateInterval + " AND style " + styleParameter  + artistParameter + orderParameter + " ;";
    }

    public string WriteQuery_ADDPAINTING(string userID, string title, string description, string style, string price, string quantity, string artistfName, string artistlName, string size, string imageURL, string creationDate, string sponsored, string uploadDate)
    {
        return "INSERT INTO Painting(title, description, style, quantity, size, price, artist_fName, artist_lName, artistID, image, creationDate, sponsored, uploadDate, rating)  VALUES('" + title + "', '" + description + "', '" + style + "', '" + quantity + "', '" + size + "', '" + price + "', '" + artistfName + "', '" + artistlName + "', '" + userID + "', '"+ imageURL + "', DATE('" + creationDate + "'), '" + sponsored + "', '" + uploadDate + "', '0');"; 
    }

    public string WriteQuery_GETPAINTINGSOFSELLER(string userID)
    {
        return "SELECT pID, title, price, image, description, rating, size, style, artist_FName, artist_LName, creationDate, uploadDate, quantity, artistID FROM Painting WHERE artistID = " + userID + ";";
    }

    public string WriteQuery_GETPAINTINGIMAGES()
    {
        return "SELECT pID, image FROM Painting;";
    }
    public string WriteQuery_GETPAINTINGIMAGE(string pID)
    {
        return "SELECT pID, image FROM Painting WHERE pID = '" + pID + "';";
    }

    public string WriteQuery_UPDATEUSER(string username, string fName, string lName, string email, string birthDate, string password, string userID)
    {
        return "UPDATE User SET username = '" + username + "' , fName = '" + fName + "' , lName = '" + lName + "' , email = '" + email + "' , birthDate = '" + birthDate + "' , password = '" + password + "' WHERE userID = '" + userID + "';";

    }
    public string WriteQuery_GETORDERS(string userID)
    {
        return "SELECT o.orderID, o.pID, o.orderDate, o.orderStatus, o.quantity, p.title, p.style, p.price, p.size, p.image, p.description, p.rating, p.artist_fName, p.artist_lName, p.creationDate, p.uploadDate, p.quantity, p.artistID FROM Orders o JOIN Painting p ON o.pID = p.pID WHERE o.userID = '" + userID + "' ORDER BY CASE WHEN o.orderStatus = 'confirmed' THEN 1 ELSE 0 END, o.orderDate DESC;";
 

    }

    public string WriteQuery_CHECKCART(string pID, string userID)
    {
        return "SELECT EXISTS(SELECT * FROM Cart WHERE pID = '" + pID + "' AND userID = '" + userID + "') as field_exists;";
    }

    public string WriteQuery_UPDATEORDER(string orderID)
    {
        return "UPDATE Orders SET orderStatus = 'confirmed' WHERE orderID = '" + orderID + "';";
    }
    public string WriteQuery_RATEPAINTINGADD(string userID, string pID, float rating)
    {
        return "INSERT INTO RatePainting(userID, pID, rating)  VALUES(" + userID + ", " + pID + ", " + rating + ");";
    }
    public string WriteQuery_UPDATEPAINTINGRATING(string pID)
    {
        return "UPDATE Painting SET rating = (SELECT AVG(rating) FROM RatePainting WHERE pID = " + pID + ") WHERE pID = " + pID + ";";
    }
  

    public string WriteQuery_SOLD(string pID)
    {
        return "SELECT COUNT(*) AS count FROM Orders WHERE pId = '" + pID + "';";
    }

    public string WriteQuery_UPDATEPAINTING(string pID, string price, string quantity, string size)
    {
        return "UPDATE Painting SET price = '" + price + "', quantity = '" + quantity + "', size = '" + size + "' WHERE pID = '" + pID + "';";

    }

    public string WriteQuery_PROMOTE(string pID, string promoteBool)
    {
        return "UPDATE Painting SET sponsored = '" + promoteBool + "';";

    }

    public string WriteQuery_DELETEPAINTING(string pID)
    {
        return "DELETE FROM Painting WHERE pID = '" + pID + "';";

    }

    public string WriteQuery_UPDATEORDER(string orderID, string orderStatus)
    {
        return "UPDATE Orders SET orderStatus = '" + orderStatus + "' WHERE orderID = '" + orderID + "';";
    }

    public string WriteQuery_DELETEORDER(string orderID)
    {
        return "DELETE FROM Orders WHERE orderID = '" + orderID + "';";
    }

   



    public IEnumerator GetQueryResult(string query, Action<string> resultCallback)
    {
        queryStatus = 1;
        print("Query Sent: " + query);
        string url = "https://onlinepaintingstore0.000webhostapp.com/PHPFiles/sendquery.php";
        WWWForm form = new WWWForm();
        form.AddField("query", query);

        UnityWebRequest www = UnityWebRequest.Post(url, form);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError)
        {
            queryStatus = 2;
        }
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Error: " + www.error);
            resultCallback(null);
            queryStatus = 2;
        }
        else
        {
            string result = www.downloadHandler.text;
            print("Query Result: " + result);
            resultCallback(result);
            queryStatus = 0;
        }
    }

    public void CallGetQueryResult(string query)
    {
     
        Coroutine coroutine = StartCoroutine(GetQueryResult(query, result =>
        {
            // Do something with the query result
            Debug.Log(result);
            

        }));
        
    }

   











}
