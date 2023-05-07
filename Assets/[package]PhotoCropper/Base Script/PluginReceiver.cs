using UnityEngine;
using System.Collections;

public class PluginReceiver : MonoBehaviour {

    public void LoadPhoto(string imgPath)
    {
        MessageManager.SendMessage("Photo Loaded", imgPath);

    }
}
