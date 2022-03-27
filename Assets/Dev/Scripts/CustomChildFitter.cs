using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

[Serializable]
public class ChildPositonCombo
{
    public GameObject child;
    public Vector3 newPosition;
    public Vector3 originalPosition;

    [ColorUsage(true, true)]
    public Color colorPressedFace;
    [ColorUsage(true, true)]
    public Color colorPressedOutline;
    public Color colorOriginalFace;
    public Color colorOriginalOutline;
    public bool changeColor;
}
public class CustomChildFitter : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public List<ChildPositonCombo> childPosCombo;

    public bool fitChild;

    private void Start()
    {
        foreach (ChildPositonCombo childCombo in childPosCombo)
        {
            childCombo.originalPosition = childCombo.child.GetComponent<RectTransform>().anchoredPosition;

            if (childCombo.changeColor)
            {
                childCombo.colorOriginalFace = childCombo.child.GetComponent<TMP_Text>().faceColor;
                childCombo.colorOriginalOutline = childCombo.child.GetComponent<TMP_Text>().outlineColor;
            }
        }

        fitChild = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        bool interact = GetComponent<Button>().interactable;

        if (fitChild && interact)
        {
            foreach (ChildPositonCombo childCombo in childPosCombo)
            {
                childCombo.child.GetComponent<RectTransform>().anchoredPosition = childCombo.newPosition;

                if (childCombo.changeColor)
                {
                    childCombo.child.GetComponent<TMP_Text>().faceColor = childCombo.colorPressedFace;
                    childCombo.child.GetComponent<TMP_Text>().outlineColor = childCombo.colorPressedOutline;
                }
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        foreach (ChildPositonCombo childCombo in childPosCombo)
        {
            childCombo.child.GetComponent<RectTransform>().anchoredPosition = childCombo.originalPosition;

            if (childCombo.changeColor)
            {
                childCombo.child.GetComponent<TMP_Text>().faceColor = childCombo.colorOriginalFace;
                childCombo.child.GetComponent<TMP_Text>().outlineColor = childCombo.colorOriginalOutline;
            }

        }
    }
}
