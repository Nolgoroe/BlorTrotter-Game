using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILayerChanger : MonoBehaviour
{
    void Start()
    {
        transform.GetComponent<SpriteRenderer>().sortingOrder = transform.parent.GetComponent<SpriteRenderer>().sortingOrder + 1;
    }

}
