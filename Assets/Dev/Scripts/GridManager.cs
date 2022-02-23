using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GridManager : MonoBehaviour, IManageable  //singleton , only instantiate one time 
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


     // when we click on a tile
    public void SetCurrentSelectedTile(Tile selectedTile)
    {
        if (currentlySelectedTile == selectedTile)  // if we click on the same tile as we clicked just before
        {
            bool validTarget = CheckTileAvailability(selectedTile);

            if (validTarget) // if the tile is empty, move on it
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
            // if we clicked on a new tile (not the same as we clicked before) this one become the new current one and we will have a visual aspect on it
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


    // feed the tutorial UI with text if possible
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
    
    /// ////////
    

    public void Start()
    {
        int i = 0;

        foreach (Tile tile in allTilesInLevel)
        {
            tile.index = i;
            i++;
        }
    }
}
