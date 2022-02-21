using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour, IManageable //singleton , only instantiate one time 
{
    public static EventManager instance;

    public void initManager()
    {
        instance = this;
        Debug.Log("success events");
    }

    public void ShootEvent(UnityEvent actionEvent)
    {
        actionEvent.Invoke();
    }
}
