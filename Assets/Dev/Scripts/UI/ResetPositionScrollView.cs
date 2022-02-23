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

    // Update is called once per frame
    
    private void OnDisable()
    {
        rctTransform.localPosition = initialPos;
    }
}
