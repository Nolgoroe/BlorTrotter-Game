using UnityEngine;
using UnityEngine.EventSystems;

public class UISelectable : MonoBehaviour, IPointerClickHandler
{

    public GameObject wikiPage;
    public GameObject basePage;
    
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        
        
        wikiPage.SetActive(true);
        basePage.SetActive(false);
        
    }
}
