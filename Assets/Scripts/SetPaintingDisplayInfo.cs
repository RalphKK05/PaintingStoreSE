using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SetPaintingDisplayInfo : MonoBehaviour
{

    public string title;
    public string pID;
    public string price;
    public string imageURL;
    public string rating;
    public string description;
    public string size;
    public string style;
    public string artistFName;
    public string artistLName;
    public string creationDate;
    public string uploadDate;
    public string artistID;
    public string qt;
    public Sprite paintingsprite;
    public Text titleText;
    public Text priceText;
    public Text artistNameText;
    public Text sizeText;
    public Text styleText;
    public Image paintingImage;
    public GameObject paintingstorePage;
    public GameObject sponsoredBar;
    GameObject appController;
    Queries queriesScript;
    PageManager pageManager;
    ImageHandler imageHandler;
    // Start is called before the first frame update
    void Start()
    {
        appController = GameObject.FindWithTag("AppController");
        queriesScript = appController.GetComponent<Queries>();
        pageManager = appController.GetComponent<PageManager>();
        imageHandler = appController.GetComponent<ImageHandler>();
        //CallGetImageResult();
      
        
    }

    // Update is called once per frame
    void Update()
    {
        titleText.text = title;
        priceText.text = price;
        paintingImage.sprite = paintingsprite;
        if (artistNameText != null)
        {
            artistNameText.text = "Artist name: " + artistFName + " " + artistLName;
        }
        if (sizeText != null)
        {
            sizeText.text = size;
        }
        if (styleText != null)
        {
            styleText.text = "Style: " + style;
        }
        if (paintingsprite == null)
        {
            Color temp = paintingImage.color;
            temp.a = 0;
            paintingImage.color = temp;
        }
        if (paintingsprite != null)
        {
            Color temp = paintingImage.color;
            temp.a = 1;
            paintingImage.color = temp;
        }

        GetImage();


    }

    public void RemovePainting()
    {
        CallGetQueryResultDELETE(queriesScript.WriteQuery_DELETEPAINTING(pID));

    }
    public void ClickEvent()
    {
        
        GameObject newPaintingPage = pageManager.OpenPAINTING_PAGE();
        newPaintingPage.GetComponent<PaintingPage>().pID = pID;
        newPaintingPage.GetComponent<PaintingPage>().title = title;
        newPaintingPage.GetComponent<PaintingPage>().price = price;
        newPaintingPage.GetComponent<PaintingPage>().imageURL = imageURL;
        newPaintingPage.GetComponent<PaintingPage>().description = description;
        newPaintingPage.GetComponent<PaintingPage>().rating = rating;
        newPaintingPage.GetComponent<PaintingPage>().size = size;
        newPaintingPage.GetComponent<PaintingPage>().style = style;
        newPaintingPage.GetComponent<PaintingPage>().artistFName = artistFName;
        newPaintingPage.GetComponent<PaintingPage>().artistLName = artistLName;
        newPaintingPage.GetComponent<PaintingPage>().creationDate = creationDate;
        newPaintingPage.GetComponent<PaintingPage>().uploadDate = uploadDate;
        newPaintingPage.GetComponent<PaintingPage>().quantity = qt;
        newPaintingPage.GetComponent<PaintingPage>().artistID = artistID;
        newPaintingPage.GetComponent<PaintingPage>().SetPaintingInfo();
    }

    public void CallGetImageResult()
    {
   
    }
    public void CallGetQueryResultDELETE(string query)
    {
        Coroutine coroutine = StartCoroutine(queriesScript.GetQueryResult(query, result =>
        {
         
            GameObject AdminSearch = GameObject.FindWithTag("AdminSearchPage");
            AdminSearch.GetComponent<SearchPage>().ExecuteSearch();
        }));
    }

    public void GetImage()
    {
        Sprite sprite = null;
        foreach (KeyValuePair<string, Sprite> kvp in imageHandler.paintings)
        {
            if (kvp.Key == pID)
            {
                sprite = kvp.Value;
                paintingsprite = sprite;
                break;
            }
            else
            {
                if (imageHandler.isFetchDone == false)
                {
                    sprite = null;
                    paintingsprite = sprite;
                }


            }
           
        }
        if (sprite == null)
        {
            if (!imageHandler.paintings.Exists(pair => pair.Key == pID))
            {
                if (imageHandler.isFetchDone)
                {
                    GetImagesQuery(queriesScript.WriteQuery_GETPAINTINGIMAGE(pID));
                }
            }
        }
      
       
        
        
    }


    public void GetImagesQuery(string query)
    {
        Coroutine coroutine = StartCoroutine(queriesScript.GetQueryResult(query, result =>
        {
            for (int i = 0; i < queriesScript.ExtractInfo(result).Count; i++)
            {
                byte[] bytes = Convert.FromBase64String(queriesScript.ExtractInfo(result)[i][1]);
                Texture2D texture = new Texture2D(1, 1);
                texture.LoadImage(bytes);
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
                paintingsprite = sprite;                           
                imageHandler.paintings.Add(new KeyValuePair<string, Sprite>(pID, sprite));
            }

        }));
    }

    public void OpenPaintingStorePage()
    {
        
        GameObject[] objectsWithTag = Resources.FindObjectsOfTypeAll<GameObject>();
        foreach (GameObject obj in objectsWithTag)
        {
            if (obj.CompareTag("PaintingStorePage"))
            {
                paintingstorePage = obj;
                break;
            }
        }
        paintingstorePage.SetActive(true);
        paintingstorePage.GetComponent<PaintingStore>().title = title;
        paintingstorePage.GetComponent<PaintingStore>().price = price;
        paintingstorePage.GetComponent<PaintingStore>().style = style;
        paintingstorePage.GetComponent<PaintingStore>().createDate = creationDate;
        paintingstorePage.GetComponent<PaintingStore>().uploadDate = uploadDate;
        paintingstorePage.GetComponent<PaintingStore>().description = description;
        paintingstorePage.GetComponent<PaintingStore>().size = size;
        paintingstorePage.GetComponent<PaintingStore>().rating = rating;
        paintingstorePage.GetComponent<PaintingStore>().quantity = qt;
        paintingstorePage.GetComponent<PaintingStore>().pID = pID;
        paintingstorePage.GetComponent<PaintingStore>().SetInfo();

    }
}
