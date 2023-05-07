using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageHandler : MonoBehaviour
{
    public Queries queries;
        public string phpURL = "https://onlinepaintingstore.000webhostapp.com/PHPFiles/sendimage.php";
        public List<KeyValuePair<string, Sprite>> paintings = new List<KeyValuePair<string, Sprite>>();
    public bool isFetchDone;
    public void Start()
    {
        GetImages();
    }
    public void GetImages()
    {
        GetImagesQuery(queries.WriteQuery_GETPAINTINGIMAGES());
    }
    public void GetImagesQuery(string query)
    {
        Coroutine coroutine = StartCoroutine(queries.GetQueryResult(query, result =>
        {
            if (result != null || result.Length > 2)
            {
                for (int i = 0; i < queries.ExtractInfo(result).Count; i++)
                {
                    byte[] bytes = null;
                    try
                    {
                        bytes = Convert.FromBase64String(queries.ExtractInfo(result)[i][1]);
                    }
                    catch(FormatException e)
                    {

                    }
                    Texture2D texture = new Texture2D(1, 1);
                    texture.LoadImage(bytes);
                    Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
                    paintings.Add(new KeyValuePair<string, Sprite>(queries.ExtractInfo(result)[i][0], sprite));
                    if (i == queries.ExtractInfo(result).Count - 1)
                    {
                        isFetchDone = true;
                    }
                }
                
            }
            if (result == null || result.Length < 2)
            {
                GetImages();
            }
           
        }));
    }
   
    }



