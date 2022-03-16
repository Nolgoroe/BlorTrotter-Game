using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeArea : MonoBehaviour
{
    public RectTransform rectTransform;
    public Rect safeArea;
    public Vector2 minAnchor;
    public Vector2 maxAnchor;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        safeArea = Screen.safeArea;
        minAnchor = safeArea.position;
        maxAnchor = minAnchor + safeArea.size;

        minAnchor.x /= Display.main.systemWidth;
        minAnchor.y /= Display.main.systemHeight;

        maxAnchor.x /= Display.main.systemWidth;
        maxAnchor.y /= Display.main.systemHeight;

        rectTransform.anchorMin = minAnchor;
        rectTransform.anchorMax = maxAnchor;
    }
}
