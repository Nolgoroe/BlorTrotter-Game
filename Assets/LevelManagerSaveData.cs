using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class LevelSavedData
{
    public int levelID;
    public int AmountOfStars;
}
public class LevelManagerSaveData : MonoBehaviour
{
    public static LevelManagerSaveData instance;
    public List<LevelSavedData> levelsSaved;

    private void Start()
    {
        instance = this;
    }

    public void SaveLevel(int score)
    {
        LevelSavedData newData = new LevelSavedData();
        newData.levelID = LevelManager.instance.currentLevel.levelID;
        newData.AmountOfStars = score;
    }
}
