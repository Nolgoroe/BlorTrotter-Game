using UnityEngine;
using UnityEngine.EventSystems;

public class UISelectable : MonoBehaviour, IPointerClickHandler  // can interact with a ui element 
{

    public GameObject wikiPage;
    public GameObject basePage;
    
    public void OnPointerClick(PointerEventData pointerEventData)
    {       
        wikiPage.SetActive(true);
        basePage.SetActive(false);
        SoundManager.instance.UIInteractSound();
    }
}
