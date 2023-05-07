using UnityEngine;
using System.Collections;

public class PhotoPluginManager
{
    public static void PickPhoto() 
    {
        GameObject receiver = GameObject.Find("PluginHelper");
        if (receiver == null)
        {
            receiver = new GameObject("PluginHelper");
            GameObject.DontDestroyOnLoad(receiver);
            receiver.AddComponent<PluginReceiver>();
        }

        AndroidJavaClass PluginSender;
        PluginSender = new AndroidJavaClass("com.photocropper.code.PhotoHelper");
        PluginSender.CallStatic("LocalPhoto");
    }
}
