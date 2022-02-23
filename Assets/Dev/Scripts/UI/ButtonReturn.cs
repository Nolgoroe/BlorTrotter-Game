using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonReturn : MonoBehaviour
{
    public GameObject precedentMenu;
    
    

    public void ReturnButton()
    {
         Transform parentTransform = this.transform.parent;
         GameObject parent = parentTransform.gameObject;

        parent.SetActive(false);
        precedentMenu.SetActive(true);

    }
    
}
