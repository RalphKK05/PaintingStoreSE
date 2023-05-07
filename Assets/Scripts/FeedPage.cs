using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FeedPage : MonoBehaviour
{
    public UserInfo userinfo;
    public Queries queries;
    public GameObject paintingDisplay;
    public string userID;
    public int nbPaintings;
    public GameObject feedscrollParent;
    public List<Sprite> paintingImagesList = new List<Sprite>();
    public List<GameObject> paintingList = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
      
      
        
     
    }

    public void LoadFeed()
    {
        foreach (GameObject painting in paintingList)
        {
            Destroy(painting);
        }
        paintingList.Clear();
        CallGetQueryResultFEEDPROMOTED(queries.WriteQuery_FEEDPROMOTED(userinfo.userID));
    }

    // Update is called once per frame
    void Update()
    {
      
        
    }

    public void CallGetQueryResultFEED(string query)
    {
        Coroutine coroutine = StartCoroutine(queries.GetQueryResult(query, result =>
        {
            DisplayPaintings(queries.ExtractInfo(result), 9999999);
            
        }));
    }

    public void CallGetQueryResultFEEDPROMOTED(string query)
    {
        Coroutine coroutine = StartCoroutine(queries.GetQueryResult(query, result =>
        {
            DisplayPaintings(queries.ExtractInfo(result), 2);

            CallGetQueryResultFEED(queries.WriteQuery_FEED(userinfo.userID));
        }));
    }



    void DisplayPaintings(List<List<string>> feedpaintings, int maxnbPaintings)
    {

        nbPaintings = feedpaintings.Count;
        for (int i = 0; i < nbPaintings; i++)
        {
            if (i == maxnbPaintings)
            {
                break;
            }
            
            GameObject painting = Instantiate(paintingDisplay, transform.position, transform.rotation);
            painting.transform.parent = feedscrollParent.transform;
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
            if (maxnbPaintings == 2)
            {
                painting.GetComponent<SetPaintingDisplayInfo>().sponsoredBar.SetActive(true);
            }
            paintingList.Add(painting);
         


        }
     

        

    }
}
