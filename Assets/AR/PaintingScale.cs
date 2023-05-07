using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PaintingScale : MonoBehaviour
{
    //Input dimensions in cm and sprite image from gallery

    public Sprite gallerySprite;
    public string Width, Height;

    //Inputs that will be passed to the AR script
    public float Xvalue;
    public float Yvalue;
    public Sprite spriteImage;

    //In unity 1 meter = 1 unit so for 50 centimeters --> 0.5 meters so 0.5 unit in scale

    //Save all inputs from user
    public void SavePaintingDetails()
    {
        Xvalue = 0.01f * int.Parse(Width);
        Yvalue = 0.01f * int.Parse(Height);
        //spriteImage = gallerySprite;
    }
}
