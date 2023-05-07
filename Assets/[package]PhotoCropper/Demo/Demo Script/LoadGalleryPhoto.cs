using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadGalleryPhoto : MonoBehaviour {
    public Image img;
 
    void Start() 
    {
        //Register photo events
        MessageManager.RegisterMessage("Photo Loaded", LoadPhoto);
	}

    void OnDestroy()
    {
        //Remove photo events
        MessageManager.RemoveMessage("Photo Loaded", LoadPhoto);
    }

    //button event: start picking photo from the gallery
    public void PickPhoto()
    {
        PhotoPluginManager.PickPhoto();
    }

    
    void LoadPhoto(object obj)
    {
        StartCoroutine(ReloadImg(obj.ToString()));
    }


    //Load photo
    IEnumerator ReloadImg(string imgPath)
    {
        string path = "file:///" + Application.persistentDataPath + "/" + imgPath;
        WWW www = new WWW(path);
        yield return www;
        Debug.Log(www.error);
        Texture2D texture = (Texture2D)www.texture;
        img.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }


}
