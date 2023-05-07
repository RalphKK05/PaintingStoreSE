using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleColorFix : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<Toggle>().isOn)
        {
            GetComponent<Image>().color = GetComponent<Toggle>().colors.selectedColor;
        }

        if (GetComponent<Toggle>().isOn == false)
        {
            
            GetComponent<Image>().color = GetComponent<Toggle>().colors.normalColor;
        }
    }
}
