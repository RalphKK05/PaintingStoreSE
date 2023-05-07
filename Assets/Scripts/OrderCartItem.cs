using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderCartItem : MonoBehaviour
{
    public string quantity;
    public string size;
    public string style;
    public Text sizeText;
    public Text styleText;
    public Text Quantity;

    // Start is called before the first frame update
    void Start()
    {
        size = GetComponent<SetPaintingDisplayInfo>().size;
        style = GetComponent<SetPaintingDisplayInfo>().style;
        sizeText.text = size;
        styleText.text = style;
        Quantity.text = quantity;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
