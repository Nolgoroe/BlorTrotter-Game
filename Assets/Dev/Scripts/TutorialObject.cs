using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class TutorialObject : MonoBehaviour
{
    [SerializeField] private string description;

    [SerializeField] private UnityEvent actionEvent;


    private void Start()
    {
        UnityEvent tutorialDescription = new UnityEvent();
        tutorialDescription.AddListener(() => TutorialManager.instance.DisplayTutorialText(description));

        actionEvent = tutorialDescription;
    }


    public UnityEvent GetActionEvent()
    {
        return actionEvent;
    }

    public void SetDescription(TypeOfTutorial typwIN)
    {
        typeTutorialDescriptionCombo combo = TutorialDescriptions.instance.descriptions.Where(p => p.type == typwIN).SingleOrDefault();

        if(combo != null)
        {
            description = combo.description;
        }
        else
        {
            Debug.LogError("PROBLEM WITH TUTORIAL");
        }
    }


    [ContextMenu("HERE")]
    public void CallDisplayTutorialText() //DELETE THIS AFTER SHOWING NATHAN
    {
        EventManager.instance.ShootEvent(actionEvent);
    }

}
