using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private GameObject[] allManagerObjects;

    private void Awake()
    {
        instance = this;
        Debug.Log("success Game Manager");
    }

    [ContextMenu("Init complete system")]
    public void InitAllGame()
    {
        foreach (GameObject manager in allManagerObjects)
        {
            if (manager.GetComponent<IManagable>() != null)
            {
                manager.GetComponent<IManagable>().initManager();
            }
            else
            {
                Debug.LogError("Object named: " + manager.name + " Has no IManagable interface! make sure it does!!");
            }
        }


        SaveLoadManager.instance.LoadGameState();
    }



    public void StartLevelSetData()
    {
        CameraController.canControlCamera = true;

        //call reset datas here?
    }


    public void EndLevelSetData()
    {
        CameraController.canControlCamera = false;

        //call reset datas here?
    }
















    [ContextMenu("start level set data")]
    public void CallStartLevelSetData() //DELTE THIS AFTER SHOWING NATHAN
    {
        StartLevelSetData();
    }

    [ContextMenu("end level set data")]
    public void CallEndLevelSetData() //DELTE THIS AFTER SHOWING NATHAN
    {
        EndLevelSetData();
    }
}
