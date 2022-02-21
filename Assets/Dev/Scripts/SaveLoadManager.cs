using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveLoadManager : MonoBehaviour, IManageable   //singleton , only instantiate one time 
{
    public static SaveLoadManager instance;

    [SerializeField] private SaveLoadData saveLoadDataObject;

    [SerializeField] private string savePath;

    public void initManager()
    {
        instance = this;

        Debug.Log("success save load manager");
    }

    public void SaveGameState()
    {
        string savedData = JsonUtility.ToJson(saveLoadDataObject);   // save the data in a file called Game State

        if (Application.platform == RuntimePlatform.Android)
        {
            savePath = Application.persistentDataPath + "/Game State.txt"; 
        }
        else
        {
            savePath = Application.dataPath + "/Game State.txt";
        }

        File.WriteAllText(savePath, savedData);

        Debug.Log("Done saving game state!");
    }

    public void LoadGameState()
    {                                                               //load Game State
        if (Application.platform == RuntimePlatform.Android)
        {
            savePath = Application.persistentDataPath + "/Game State.txt";
        }
        else
        {
            savePath = Application.dataPath + "/Game State.txt";
        }

        if (File.Exists(savePath))
        {
            JsonUtility.FromJsonOverwrite(File.ReadAllText(savePath), saveLoadDataObject);
        }

        Debug.Log("Done loading game state!");
    }
}
