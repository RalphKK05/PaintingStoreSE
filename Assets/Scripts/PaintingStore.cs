using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PaintingStore : MonoBehaviour
{
    public AccountPage accPage;
    public Image paintingimage;
    public Sprite paintingImage;
    public ImageHandler imageHandler;
    public GameObject PromoteButton, UnPromoteButton;
    public Queries queries;
    public InputField sizeWInput, sizeHInput, priceInput, quantityInput;
    public Text soldText;
    public Text descriptionText;
    public Text createDateText;
    public Text uploadDateText;
    public Text sizeText;
    public Text priceText;
    public Text quantityText;
    public Text titleText;
    public Text styleText;
    public Image star1, star2, star3, star4, star5;
    
    public string pID;
    public string sold;
    public string description;
    public string createDate;
    public string uploadDate;
    public string size;
    public string price;
    public string quantity;
    public string title;
    public string style;
    public string rating;

    // Start is called before the first frame update
    void Start()
    {
        
    }
   
    public void SetInfo()
    {

        CallGetQuerySOLD(queries.WriteQuery_SOLD(pID));
        titleText.text = title;
        priceText.text = "$" + price;
        styleText.text = "Style: " + style;
        createDateText.text = "Created on: " + createDate;
        uploadDateText.text = "Created on: " + uploadDate;
        descriptionText.text = "Description: " + description;
        quantityText.text = "Quantity: " + quantity;
        sizeText.text = size;
        SetRating();

    }
    public void CallGetQuerySOLD(string query)
    {
        Coroutine coroutine = StartCoroutine(queries.GetQueryResult(query, result =>
        {
            sold = queries.ExtractInfo(result)[0][0];
        }));
    }

    void SetRating()
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
    // Update is called once per frame
    void Update()
    {
        soldText.text = "Sold: " + sold + " units";
        GetImage();
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
    }

    public void ConfirmChanges()
    {
        price = priceInput.text;
        quantity = quantityInput.text;
        size = sizeWInput.text + "x" + sizeHInput.text;
        CallGetQueryUPDATEPAINTING(queries.WriteQuery_UPDATEPAINTING(pID, price, quantity, size));
    }

    public void Promote()
    {
        CallGetQueryPROMOTE(queries.WriteQuery_PROMOTE(pID, "1"));
        PromoteButton.SetActive(false);
        UnPromoteButton.SetActive(true);
    }

    public void UnPromote()
    {
        CallGetQueryPROMOTE(queries.WriteQuery_PROMOTE(pID, "0"));
        PromoteButton.SetActive(true);
        UnPromoteButton.SetActive(false);
    }
    public void DeletePainting()
    {
        CallGetQueryDELETE(queries.WriteQuery_DELETEPAINTING(pID));
    }
    public void CallGetQueryUPDATEPAINTING(string query)
    {
        Coroutine coroutine = StartCoroutine(queries.GetQueryResult(query, result =>
        {
            SetInfo();
        }));
    }

    public void CallGetQueryPROMOTE(string query)
    {
        Coroutine coroutine = StartCoroutine(queries.GetQueryResult(query, result =>
        {
           
        }));
    }

    public void CallGetQueryDELETE(string query)
    {
        Coroutine coroutine = StartCoroutine(queries.GetQueryResult(query, result =>
        {
            accPage.LoadStore();
            this.gameObject.SetActive(false);
            
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
