using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GridManager : MonoBehaviour, IManageable
{
    public static GridManager instance;

    public List<Tile> allTilesInLevel;

    [SerializeField] private Tile currentlySelectedTile;

    [SerializeField] private TileDisplayManager tileDisplayManager;

    public void initManager()
    {
        instance = this;
        
        allTilesInLevel = new List<Tile>();

        currentlySelectedTile = null;

        Debug.Log("success Grid Manager");
    }

    public void AddTileToTileList(Tile toAdd)
    {
        allTilesInLevel.Add(toAdd);
    }

    public void ClearTileManagerData()
    {
        allTilesInLevel.Clear();
        currentlySelectedTile = null;
    }

    public void SetCurrentSelectedTile(Tile selectedTile)
    {
        if (currentlySelectedTile == selectedTile)
        {
            bool validTarget = CheckTileAvailability(selectedTile);

            if (validTarget)
            {
                EntityManager.instance.MovePlayer(selectedTile); /// HAS TO BE A BETTER WAY TO DO THIS
                //move the player to selectedTile here since we just selected the same tile twice..
            }
            else
            {
                // dunno what to do here?
            }

            tileDisplayManager.SetTileNotSelectedDisplay(currentlySelectedTile);



            currentlySelectedTile = null;
        }
        else
        {
            currentlySelectedTile = selectedTile;
            tileDisplayManager.SetTileSelectedDisplay(currentlySelectedTile);

            CheckDisplayTutorialText(selectedTile);
        }
    }

    public bool CheckTileAvailability(Tile TargetTile)
    {
        // Nathan will populate this with relevant logic


        if (TargetTile.isFull)
        {
            return false;
        }
        else
        {
            return true;
        }
    }



    public void CheckDisplayTutorialText(Tile tileToCheck)
    {
        if (tileToCheck.GetComponent<TutorialObject>())
        {
            EventManager.instance.ShootEvent(tileToCheck.GetComponent<TutorialObject>().GetActionEvent());
        }
    }


    public Tile GetAdjacentTile(Tile currentTile)
    {
        /// this is not how the logic is supposed to work - we need to check the up, down, left and right tiles and choose one out of them


        return null;
    }

}
