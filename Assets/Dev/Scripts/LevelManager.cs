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

        float offset = 1.184f;
        

        foreach (IndexToGameobject element in currentLevel.objectsOnGrid)
        {
            Tile tilePosition = GridManager.instance.allTilesInLevel.Where(p => p.index == element.tileIndex).SingleOrDefault();

            ////////////////HERE
            Vector3 position = new Vector3(tilePosition.transform.position.x, tilePosition.transform.position.y + offset, tilePosition.transform.position.z);
            GameObject obstacle =  Instantiate(element.prefab, position, tilePosition.transform.rotation);
            obstacle.GetComponent<SpriteRenderer>().sortingOrder = tilePosition.GetComponent<SpriteRenderer>().sortingOrder;
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
