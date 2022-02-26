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
                EntityManager.instance.MovePlayer(selectedTile); 
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


    public void GetAdjacentTile(Tile currentTile, Entity callingEntity)
    {
        /// this is not how the logic is supposed to work - we need to check the up, down, left and right tiles and choose one out of them
     
        int w = LevelEditor.instance.levelMap.texture.width;
        int h = LevelEditor.instance.levelMap.texture.height;

        switch (currentTile.edgeType)
        {
            case EdgeType.notEdge:
                if (GetTileBottom(currentTile))
                {
                    callingEntity.entityAdjacentTiles.Add(GetTileBottom(currentTile));
                }

                if (GetTileTop(currentTile))
                {
                    callingEntity.entityAdjacentTiles.Add(GetTileTop(currentTile));
                }

                if (GetTileRight(currentTile))
                {
                    callingEntity.entityAdjacentTiles.Add(GetTileRight(currentTile));
                }

                if (GetTileLeft(currentTile))
                {
                    callingEntity.entityAdjacentTiles.Add(GetTileLeft(currentTile));
                }
                break;

            case EdgeType.leftEdge:
                if (GetTileBottom(currentTile))
                {
                    callingEntity.entityAdjacentTiles.Add(GetTileBottom(currentTile));
                }

                if (GetTileTop(currentTile))
                {
                    callingEntity.entityAdjacentTiles.Add(GetTileTop(currentTile));
                }

                if (GetTileRight(currentTile))
                {
                    callingEntity.entityAdjacentTiles.Add(GetTileRight(currentTile));
                }
                break;

            case EdgeType.rightEdge:
                if (GetTileLeft(currentTile))
                {
                    callingEntity.entityAdjacentTiles.Add(GetTileLeft(currentTile));
                }

                if (GetTileBottom(currentTile))
                {
                    callingEntity.entityAdjacentTiles.Add(GetTileBottom(currentTile));
                }

                if (GetTileTop(currentTile))
                {
                    callingEntity.entityAdjacentTiles.Add(GetTileTop(currentTile));
                }
                break;

            case EdgeType.bottomEdge:
                if (GetTileTop(currentTile))
                {
                    callingEntity.entityAdjacentTiles.Add(GetTileTop(currentTile));
                }

                if (GetTileRight(currentTile))
                {
                    callingEntity.entityAdjacentTiles.Add(GetTileRight(currentTile));
                }

                if (GetTileLeft(currentTile))
                {
                    callingEntity.entityAdjacentTiles.Add(GetTileLeft(currentTile));
                }
                break;

            case EdgeType.topEdge:
                if (GetTileRight(currentTile))
                {
                    callingEntity.entityAdjacentTiles.Add(GetTileRight(currentTile));
                }

                if (GetTileLeft(currentTile))
                {
                    callingEntity.entityAdjacentTiles.Add(GetTileLeft(currentTile));
                }

                if (GetTileBottom(currentTile))
                {
                    callingEntity.entityAdjacentTiles.Add(GetTileBottom(currentTile));
                }
                break;

            case EdgeType.topRightEdge:
                if (GetTileLeft(currentTile))
                {
                    callingEntity.entityAdjacentTiles.Add(GetTileLeft(currentTile));
                }

                if (GetTileBottom(currentTile))
                {
                    callingEntity.entityAdjacentTiles.Add(GetTileBottom(currentTile));
                }
                    
                break;

            case EdgeType.topLeftEdge:
                if (GetTileRight(currentTile))
                {
                    callingEntity.entityAdjacentTiles.Add(GetTileRight(currentTile));
                }

                if (GetTileBottom(currentTile))
                {
                    callingEntity.entityAdjacentTiles.Add(GetTileBottom(currentTile));
                }
                break;

            case EdgeType.bottomRightEdge:
                if (GetTileLeft(currentTile))
                {
                    callingEntity.entityAdjacentTiles.Add(GetTileLeft(currentTile));
                }

                if (GetTileTop(currentTile))
                {
                    callingEntity.entityAdjacentTiles.Add(GetTileTop(currentTile));
                }
                break;

            case EdgeType.bottomLeftEdge:
                if(GetTileTop(currentTile))
                {
                    callingEntity.entityAdjacentTiles.Add(GetTileTop(currentTile));
                }

                if(GetTileRight(currentTile))
                {
                    callingEntity.entityAdjacentTiles.Add(GetTileRight(currentTile));
                }
                
                break;

            default:
                break;
        }
    }

    public Tile GetTileBottom(Tile tile)
    {
        int tileIndex = tile.index + LevelEditor.instance.levelMap.texture.width;

        if(allTilesInLevel[tileIndex].isFull)
        {
            return null;
        }
        else
        {
            return allTilesInLevel[tileIndex];
        }                 
    }
    public Tile GetTileTop(Tile tile)
    {
        int tileIndex = tile.index - LevelEditor.instance.levelMap.texture.width;

        if (allTilesInLevel[tileIndex].isFull)
        {
            return null;
        }
        else
        {
            return allTilesInLevel[tileIndex];
        }
        
    }
    public Tile GetTileLeft(Tile tile)
    {
        int tileIndex = tile.index - 1;

        if (allTilesInLevel[tileIndex].isFull)
        {
            return null;
        }
        else
        {
            return allTilesInLevel[tileIndex];
        }
    }
    public Tile GetTileRight(Tile tile)
    {
        int tileIndex = tile.index + 1;

        if (allTilesInLevel[tileIndex].isFull)
        {
            return null;
        }
        else
        {
            return allTilesInLevel[tileIndex];
        }
    }
    /// ////////

    public void TilesIndexer()
    {
        int i = 0;

        foreach (Tile tile in allTilesInLevel)
        {
            tile.index = i;
            i++;
        }
    }

    public void SetMapEdges()
    {
        int w = LevelEditor.instance.levelMap.texture.width;
        int h = LevelEditor.instance.levelMap.texture.height;


        foreach (Tile tile in allTilesInLevel)
        {
            if (tile.index == 0)
            {
                tile.edgeType = EdgeType.topLeftEdge;
            }
            else if(tile.index == w-1)
            {
                tile.edgeType = EdgeType.topRightEdge;
            }
            else if(tile.index == ( w*(h-1)))
            {
                tile.edgeType = EdgeType.bottomLeftEdge;
            }
            else if(tile.index == (w*h)-1)
            {
                tile.edgeType = EdgeType.bottomRightEdge;
            }           
        }
    }

    
}
