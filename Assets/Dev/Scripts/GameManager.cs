using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class GameManager : MonoBehaviour  //singleton , only instantiate one time 
{
    public static GameManager instance;  

    [SerializeField] private GameObject[] allManagerObjects;

    private void Awake()
    {
        instance = this;
        Debug.Log("success Game Manager");


        InitAllGame();
        //LevelManager.instance.ChooseLevel(1);
        //LevelManager.instance.LoadLevel();
        //StartLevelSetData(); 
        //CameraController.instance.CenterOnBlob();

        UIManager.instance.DisplaySpecificScreens(new UIScreenTypes[] { UIScreenTypes.StartGifScreen, UIScreenTypes.MainMenu });
    }

    [ContextMenu("Init complete system")]
    public void InitAllGame()
    {
        foreach (GameObject manager in allManagerObjects)
        {
            if (manager.GetComponent<IManageable>() != null)
            {
                manager.GetComponent<IManageable>().initManager(); // initizalize all singleton objects
            }
            else
            {
                Debug.LogError("Object named: " + manager.name + " Has no IManagable interface! make sure it does!!");
            }
        }


        SaveLoadManager.instance.LoadGameState(); // load the game
    }











    public void ReloadScene()
    {
        SceneManager.LoadScene(0);
    }

    //[ContextMenu("start level set data")]
    //public void CallStartLevelSetData() //DELTE THIS AFTER SHOWING NATHAN
    //{
    //    StartLevelSetData();
    //}

    //[ContextMenu("end level set data")]
    //public void CallEndLevelSetData() //DELTE THIS AFTER SHOWING NATHAN
    //{
    //    EndLevelSetData();
    //}
}
