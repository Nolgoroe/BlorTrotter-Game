using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlphaHitTestThreshhold : MonoBehaviour
{
    public Image theButton;

    public float threshhold;

    void Start()
    {
        theButton = GetComponent<Image>();

    }

    private void Update()
    {
        theButton.alphaHitTestMinimumThreshold = threshhold;
    }
}
