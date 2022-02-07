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
    }


    public UnityEvent GetActionEvent()
    {
        return actionEvent;
    }
}
