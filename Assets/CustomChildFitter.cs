using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

[Serializable]
public class ChildPositonCombo
{
    public GameObject child;
    public Vector3 newPosition;
    public Vector3 originalPosition;
}
public class CustomChildFitter : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public List<ChildPositonCombo> childPosCombo;

    private void Start()
    {
        foreach (ChildPositonCombo childCombo in childPosCombo)
        {
            childCombo.originalPosition = childCombo.child.GetComponent<RectTransform>().anchoredPosition;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        foreach (ChildPositonCombo childCombo in childPosCombo)
        {
            childCombo.child.GetComponent<RectTransform>().anchoredPosition = childCombo.newPosition;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        foreach (ChildPositonCombo childCombo in childPosCombo)
        {
            childCombo.child.GetComponent<RectTransform>().anchoredPosition = childCombo.originalPosition;
        }    }
}
