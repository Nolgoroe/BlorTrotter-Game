using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonReturn : MonoBehaviour
{
    public GameObject precedentMenu;
    
    

    public void ReturnButton()
    {
         Transform parentTransform = this.transform.parent; //need the transform of the parent to get the gameobject 
         GameObject parent = parentTransform.gameObject;

        parent.SetActive(false);                ///disactive this object and reactive the menu
        precedentMenu.SetActive(true);

    }
    
}
