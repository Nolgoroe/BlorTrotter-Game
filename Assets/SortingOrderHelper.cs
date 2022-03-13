using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingOrderHelper : MonoBehaviour
{
    void Start()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Transform parentObject = transform.parent.parent;

        sr.sortingOrder = parentObject.GetComponent<SpriteRenderer>().sortingOrder + 1;
    }

}
