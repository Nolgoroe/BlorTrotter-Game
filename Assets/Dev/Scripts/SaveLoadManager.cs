using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveLoadManager : MonoBehaviour, IManagable
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
        string savedData = JsonUtility.ToJson(saveLoadDataObject);

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
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            savePath = Application.persistentDataPath + "/Game State.txt";
        }
        else
        {
            savePath = Application.dataPath + "/Game State.txt";
        }

        JsonUtility.FromJsonOverwrite(File.ReadAllText(savePath), saveLoadDataObject);

        Debug.Log("Done loading game state!");
    }
}
