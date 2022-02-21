using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LevelManager : MonoBehaviour, IManageable
{
    public static LevelManager instance;

    public LevelScriptableObject currentLevel;

    [SerializeField] private int currentLevelNumberOfMoves;

    public void initManager()
    {
        instance = this;
        Debug.Log("success levels");
    }



    public void ChooseLevel(int levelNum) // we instantiate to have a clone of the scriptable object, because we absolutely don't want to change the data in the original one
    {
        currentLevel = Instantiate((LevelScriptableObject)Resources.Load("Scriptable Objects/Levels/Level " + levelNum)); /// NATHA PLEASE COMMENT THIS
    }


    public void LoadLevel()
    {
        currentLevelNumberOfMoves = currentLevel.maxNumberOfMoves;

        foreach (IndexToGameobject element in currentLevel.objectsOnGrid)
        {
            Tile tilePoisition = GridManager.instance.allTilesInLevel.Where(p => p.index == element.tileIndex).SingleOrDefault();

            // add logic of instantiating.. sorting order.. maybe change logic to fit your coding style!
            // there is still morel logic to write here
        }
    }


    public void CheckEndLevel()
    {
        if(currentLevelNumberOfMoves <= 0)
        {
            // level lost logic here
            // send to UI Screen to manage screen display
            Debug.Log("LOST LEVEL, OUT OF MOVES!");
        }
    }










    [ContextMenu("Choose Level")]
    public void callChooseLevel() //DELTE THIS AFTER SHOWING NATHAN
    {
        ChooseLevel(1);
    }

    [ContextMenu("Load Level")]
    public void CallLoadLevel() //DELTE THIS AFTER SHOWING NATHAN
    {
        LoadLevel();
    }

    [ContextMenu("Decrease Number Of Moves")]
    public void test() //DELTE THIS AFTER SHOWING NATHAN
    {
        currentLevelNumberOfMoves--;
        CheckEndLevel();
    }
}
