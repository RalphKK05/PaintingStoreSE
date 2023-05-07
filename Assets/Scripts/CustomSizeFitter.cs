using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomSizeFitter : MonoBehaviour
{
    public RectTransform rectTransform;
    public float sizeMultiplier;
       // Start is called before the first frame update
       void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {       
        Vector2 sizeDelta = rectTransform.sizeDelta;
        sizeDelta.y = transform.childCount * sizeMultiplier;
        rectTransform.sizeDelta = sizeDelta;
    }
}
