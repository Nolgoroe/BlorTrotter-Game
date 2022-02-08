using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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


    [ContextMenu("HERE")]
    public void CallDisplayTutorialText() //DELETE THIS AFTER SHOWING NATHAN
    {
        EventManager.instance.ShootEvent(actionEvent);
    }

}
