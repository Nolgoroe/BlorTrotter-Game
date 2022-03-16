using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPositionScrollView : MonoBehaviour
{
    private RectTransform rctTransform;
    private Vector3 initialPos;
    void Start()
    {
        rctTransform = GetComponent<RectTransform>();
        initialPos = rctTransform.localPosition;
    }

    //reset the position to avoid the fact that if we scroll the  base screen, click on an element and go back to the base, the screen is still scrolled 
    
    private void OnDisable()
    {
        rctTransform.localPosition = initialPos;
    }
}
