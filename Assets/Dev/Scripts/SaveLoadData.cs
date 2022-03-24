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
public class SaveLoadData : MonoBehaviour
{
    public int maxLevelReached;

    public List<LevelSavedData> levelsSaved;


    public void SaveLevel(int score)
    {
        LevelSavedData newData = new LevelSavedData();
        newData.levelID = LevelManager.instance.currentLevel.levelID;
        newData.AmountOfStars = score;
    }
}
