using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CartItem : MonoBehaviour
{

    public Toggle toggle;
    public Text counterText;
    public int counterValue;
    public int quantity;
    public string size;
    public string style;
    public Text sizeText;
    public Text styleText;
    int maxQt;
    // Start is called before the first frame update
    void Start()
    {
        counterValue = 1;
        size = GetComponent<SetPaintingDisplayInfo>().size;
        style = GetComponent<SetPaintingDisplayInfo>().style;
        sizeText.text = size;
        styleText.text = style;
        maxQt = int.Parse(GetComponent<SetPaintingDisplayInfo>().qt);
    }

    // Update is called once per frame
    void Update()
    {
        counterText.text = counterValue.ToString();
        quantity = counterValue;
    }

    public void Increment()
    {
        ///coun
        if (counterValue != maxQt)
        {
            counterValue = int.Parse(counterText.text);
            counterValue++;
        }
       
       


    }
    public void Decrement()
    {
        ///coun
        counterValue = int.Parse(counterText.text);
        counterValue--;

    }
}
