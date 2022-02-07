using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.Events;

public class TutorialManager : MonoBehaviour, IManagable
{
    public static TutorialManager instance;

    public void initManager()
    {
        instance = this;
        Debug.Log("success tutorial");
    }

    public void DisplayTutorialText(string text)
    {
        UIManager.instance.TypeWriterWrite(text, UIManager.instance.tutorialTextObject);
    }





    [ContextMenu("HERE")]
    public void CallDisplayTutorialText(UnityEvent actionEvent) //DELTE THIS AFTER SHOWING NATHAN
    {
        EventManager.instance.ShootEvent(actionEvent);
    }
}
