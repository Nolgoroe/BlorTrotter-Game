using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.Events;

public class TutorialManager : MonoBehaviour, IManageable
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

    public void CheckDisplayTutorialText(Tile tileToCheck)
    {
        if (tileToCheck.GetComponent<TutorialObject>())
        {
            EventManager.instance.ShootEvent(tileToCheck.GetComponent<TutorialObject>().GetActionEvent());

            UIManager.instance.DisplayTutorialExclimation();
        }
    }




}
