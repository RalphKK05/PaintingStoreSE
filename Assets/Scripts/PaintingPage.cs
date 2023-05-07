using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PaintingPage : MonoBehaviour
{
    public GameObject cartButton, cartExists;
    public ImageHandler imageHandler;
    public PaintingScale paintingscale;
    public UserInfo userinfo;
    public GameObject MainPanel;
    public GameObject CartPageTop;
    public GameObject similarStyleScrollParent, similarArtistScrollParent;
    public GameObject paintingDisplay;
    public string pID, title, artistFName, artistLName, style, size, price, rating, description, imageURL, creationDate, uploadDate, quantity, artistID;
    public Image star1, star2, star3, star4, star5;
    public Text titleText;
    public Text artistText;
    public Text styleText;
    public Text sizeText;
    public Text priceText;
    public Text similarArtistText;
    public Text descriptionText;
    public Text createDateText;
    public Text uploadDateText;
    public Text quantityText;
    public Sprite paintingImage;
    public Image paintingimage;
    public Queries queries;
    float viewTimer;

    // Start is called before the first frame update
    void Start()
    {


    }

    public void StartAR()
    {
        UpdateUserPreferenceAR(queries.WriteQuery_UPDATEPREFERENCE(userinfo.userID, style, 15));

        
    }

    public void OpenCart()
    {
        GameObject newCart = Instantiate(CartPageTop, CartPageTop.transform.position, CartPageTop.transform.rotation);
        newCart.SetActive(true);
        newCart.GetComponent<CartPage>().LoadCart();
    }

    public void AddToCart()
    {
        CallGetQueryAddCart(queries.WriteQuery_ADDTOCART(userinfo.userID, pID));
       
    }
    public void SetPaintingInfo()
    {
        viewTimer = 0;
        paintingImage = null;
        Vector3 pos = MainPanel.GetComponent<RectTransform>().position;
        pos.y = 32;
        MainPanel.GetComponent<RectTransform>().position = pos;
        titleText.text = title;
        artistText.text = "Artist: " + artistFName + " " + artistLName;
        if (styleText != null)
        {
            styleText.text = "Style: " + style;
        }
        if (sizeText != null)
        {
            sizeText.text = "Size: " + size;
        }

        descriptionText.text = "Description: " + description;
        priceText.text = "$" + price;
        createDateText.text = "Creation Date: " + creationDate;
        uploadDateText.text = "Upload Date: " + uploadDate;
        similarArtistText.text = "Some of " + artistFName + " " + artistLName + "'s art";
        quantityText.text = "Quantity: " + quantity;
        SetRating();
   
        CallGetQueryResultSIMILARSTYLE(queries.WriteQuery_SIMILARSTYLE(style, pID));
        CallGetQueryResultSIMILARARTIST(queries.WriteQuery_SIMILARARTIST(artistFName, artistLName, pID, artistID));
        CallGetQueryResultCARTEXISTS(queries.WriteQuery_CHECKCART(pID, userinfo.userID));

    }
    void SetRating()
    {
        if (rating != "")
        {
            int integerPart = Mathf.FloorToInt(float.Parse(rating));
            float decimalPart = float.Parse(rating) - integerPart;
            if (integerPart == 0)
            {
                star1.fillAmount = decimalPart;
                star2.fillAmount = 0;
                star3.fillAmount = 0;
                star4.fillAmount = 0;
                star5.fillAmount = 0;
            }
            if (integerPart == 1)
            {
                star1.fillAmount = 1;
                star2.fillAmount = decimalPart;
                star3.fillAmount = 0;
                star4.fillAmount = 0;
                star5.fillAmount = 0;
            }
            if (integerPart == 2)
            {
                star1.fillAmount = 1;
                star2.fillAmount = 1;
                star3.fillAmount = decimalPart;
                star4.fillAmount = 0;
                star5.fillAmount = 0;
            }
            if (integerPart == 3)
            {
                star1.fillAmount = 1;
                star2.fillAmount = 1;
                star3.fillAmount = 1;
                star4.fillAmount = decimalPart;
                star5.fillAmount = 0;
            }
            if (integerPart == 4)
            {
                star1.fillAmount = 1;
                star2.fillAmount = 1;
                star3.fillAmount = 1;
                star4.fillAmount = 1;
                star5.fillAmount = decimalPart;
            }
            if (integerPart == 5)
            {
                star1.fillAmount = 1;
                star2.fillAmount = 1;
                star3.fillAmount = 1;
                star4.fillAmount = 1;
                star5.fillAmount = 1;
            }
        }
        if (rating == "")
        {
            star1.fillAmount = 0;
            star2.fillAmount = 0;
            star3.fillAmount = 0;
            star4.fillAmount = 0;
            star5.fillAmount = 0;
        }


    }
    // Update is called once per frame
    void Update()
    {
        viewTimer += 1 * Time.deltaTime;
        paintingimage.sprite = paintingImage;
        if (paintingImage == null)
        {
            Color temp = paintingimage.color;
            temp.a = 0;
            paintingimage.color = temp;
        }
        if (paintingImage != null)
        {
            Color temp = paintingimage.color;
            temp.a = 1;
            paintingimage.color = temp;
        }
        GetImage();
    }

    public void CallGetQueryResultSIMILARSTYLE(string query)
    {
        Coroutine coroutine = StartCoroutine(queries.GetQueryResult(query, result =>
        {
            DisplayPaintings(queries.ExtractInfo(result), similarStyleScrollParent);
            //CallGetQueryResultFEED(queries.WriteQuery_FEED("5"));
        }));
    }
    public void CallGetQueryResultCARTEXISTS(string query)
    {
        Coroutine coroutine = StartCoroutine(queries.GetQueryResult(query, result =>
        {
            if (queries.ExtractInfo(result)[0][0] == "1")
            {
                cartButton.SetActive(false);
                cartExists.SetActive(true);
            }
            else
            {
                cartButton.SetActive(true);
                cartExists.SetActive(false);
            }
        }));
    }
    public void CallGetQueryResultSIMILARARTIST(string query)
    {
        Coroutine coroutine = StartCoroutine(queries.GetQueryResult(query, result =>
        {
            DisplayPaintings(queries.ExtractInfo(result), similarArtistScrollParent);
            //CallGetQueryResultFEED(queries.WriteQuery_FEED("5"));
        }));
    }

    void DisplayPaintings(List<List<string>> feedpaintings, GameObject parentPanelObject)
    {


        for (int i = 0; i < feedpaintings.Count; i++)
        {


            GameObject painting = Instantiate(paintingDisplay, transform.position, transform.rotation);
            painting.transform.parent = parentPanelObject.transform;
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

            //paintingList.Add(painting);



        }




    }

    public void BackButton()
    {
        Vector3 pos = MainPanel.GetComponent<RectTransform>().position;
        pos.y = -5000;
        MainPanel.GetComponent<RectTransform>().position = pos;
        if (viewTimer >= 10)
        {
            UpdateUserPreference(queries.WriteQuery_UPDATEPREFERENCE(userinfo.userID, style, 5));
            viewTimer = 0;
        }
        Destroy(this.gameObject);
    }
  
    public void UpdateUserPreference(string query)
    {
        Coroutine coroutine = StartCoroutine(queries.GetQueryResult(query, result =>
        {

        }));
    }

    public void UpdateUserPreferenceAR(string query)
    {
        Coroutine coroutine = StartCoroutine(queries.GetQueryResult(query, result =>
        {
            paintingscale.Width = "10";
            paintingscale.Height = "20";
            paintingscale.spriteImage = paintingImage;
            paintingscale.SavePaintingDetails();
            GameObject canvas = GameObject.FindWithTag("Canvas");
            canvas.SetActive(false);

            GameObject[] objectsWithTag = Resources.FindObjectsOfTypeAll<GameObject>();
            foreach (GameObject obj in objectsWithTag)
            {
                if (obj.CompareTag("AR"))
                {
                    GameObject ar = obj;
                    ar.SetActive(true);
                    break;
                }
            }
            
        }));
    }
    public void CallGetQueryAddCart(string query)
    {
        Coroutine coroutine = StartCoroutine(queries.GetQueryResult(query, result =>
        {
           
            UpdateUserPreference(queries.WriteQuery_UPDATEPREFERENCE(userinfo.userID, style, 10));
            CallGetQueryResultCARTEXISTS(queries.WriteQuery_CHECKCART(pID, userinfo.userID));
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
                paintingImage = sprite;
                break;
            }
            else
            {
                if (imageHandler.isFetchDone == false)
                {
                    sprite = null;
                    paintingImage = sprite;
                }


            }

        }
        if (sprite == null)
        {
            if (!imageHandler.paintings.Exists(pair => pair.Key == pID))
            {
                if (imageHandler.isFetchDone)
                {
                    GetImagesQuery(queries.WriteQuery_GETPAINTINGIMAGE(pID));
                }
            }
        }




    }


    public void GetImagesQuery(string query)
    {
        Coroutine coroutine = StartCoroutine(queries.GetQueryResult(query, result =>
        {
            for (int i = 0; i < queries.ExtractInfo(result).Count; i++)
            {
                byte[] bytes = Convert.FromBase64String(queries.ExtractInfo(result)[i][1]);
                Texture2D texture = new Texture2D(1, 1);
                texture.LoadImage(bytes);
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
                paintingImage = sprite;
            }

        }));

    }
}
