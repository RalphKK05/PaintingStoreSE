using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SetPaintingMainDisplayInfo : MonoBehaviour
{
    public Queries queriesScript;
    public ImageHandler imageHandler;
    public string pID;
    public string title;
    public string description;
    public string price;
    public string size;
    public string rating;
    public string imageURL;
    public Sprite paintingImage;
    public Image paintingImageDisplay;

    // Start is called before the first frame update
    void Start()
    {
        CallGetQueryResult(queriesScript.WriteQuery_FEED("5"));
        CallGetImageResult();
    }

    // Update is called once per frame
    void Update()
    {
    
        GetImage();
        if (paintingImage == null)
        {
            Color temp = paintingImageDisplay.color;
            temp.a = 0;
            paintingImageDisplay.color = temp;
        }
        if (paintingImage != null)
        {
            Color temp = paintingImageDisplay.color;
            temp.a = 1;
            paintingImageDisplay.color = temp;
        }

    }

    public void CallGetImageResult()
    {

    }

    //SimilarStyle and Artist query and display them under main painting
    public void CallGetQueryResult(string query)
    {
        Coroutine coroutine = StartCoroutine(queriesScript.GetQueryResult(query, result =>
        {
            //DisplayPaintings(queries.ExtractInfo(result));
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

                sprite = null;

                paintingImage = sprite;

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
                paintingImage = sprite;
            }

        }));
    }
}


