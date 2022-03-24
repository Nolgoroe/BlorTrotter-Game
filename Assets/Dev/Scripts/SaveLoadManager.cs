using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using UnityEngine.SceneManagement;

public class SaveLoadManager : MonoBehaviour, IManageable   //singleton , only instantiate one time 
{
    public static SaveLoadManager instance;

    public SaveLoadData saveLoadDataObject;

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


    public void SaveLevel(int score)
    {
        LevelSavedData data = saveLoadDataObject.levelsSaved.Where(p => p.levelID == LevelManager.instance.currentLevel.levelID).SingleOrDefault();

        if(data == null)
        {
            LevelSavedData newData = new LevelSavedData();
            newData.levelID = LevelManager.instance.currentLevel.levelID;
            newData.AmountOfStars = score;
            saveLoadDataObject.levelsSaved.Add(newData);
        }
        else
        {
            int currentScore = data.AmountOfStars;

            if(score > currentScore)
            {
                data.AmountOfStars = score;
            }
            Debug.Log("Already have this level");
        }
    }
    public void CheckMaxLevelReached(int numIN)
    {
        if(numIN > saveLoadDataObject.maxLevelReached)
        {
            saveLoadDataObject.maxLevelReached = numIN;
        }
    }


    public void ResetGameData()
    {
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
            File.Delete(savePath);
        }

        SceneManager.LoadScene(0);
    }
}
