using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GridManager : MonoBehaviour, IManageable  //singleton , only instantiate one time 
{
    public static GridManager instance;

    public List<Tile> allTilesInLevel;
    public List<Tile> allEdgeTileInLevel;

    [SerializeField] private Tile currentlySelectedTile;

    [SerializeField] private TileDisplayManager tileDisplayManager;

    public void initManager()
    {
        instance = this;
        
        allTilesInLevel = new List<Tile>();
        allEdgeTileInLevel = new List<Tile>();
        

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
    public void SetCurrentSelectedTile(Tile selectedTile) // NEW
    {
        if (currentlySelectedTile == selectedTile)  // if we click on the same tile as we clicked just before
        {
            //bool validTarget = CheckTileAvailability(selectedTile);

            //if (validTarget) // if the tile is empty, move on it
            //{

            EntityManager.instance.MovePlayer(selectedTile);
            //move the player to selectedTile here since we just selected the same tile twice..

            //}
            //else
            //{
            //    // dunno what to do here?
            //}

            tileDisplayManager.SetTileNotSelectedDisplay(currentlySelectedTile);  



            currentlySelectedTile = null;
        }
        else
        {
            // if we clicked on a new tile (not the same as we clicked before) this one become the new current one and we will have a visual aspect on it

            if (currentlySelectedTile)
            {
                tileDisplayManager.SetTileNotSelectedDisplay(currentlySelectedTile);
            }

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


    public void GetAdjacentTile(Tile currentTile, Entity callingEntity) // NEW IN EVERY SWITCH CASE
    {
        /// this is not how the logic is supposed to work - we need to check the up, down, left and right tiles and choose one out of them
     
        int w = LevelEditor.instance.levelMap.texture.width;
        int h = LevelEditor.instance.levelMap.texture.height;

        switch (currentTile.edgeType)
        {
            case EdgeType.notEdge:
                if (GetTileBottom(currentTile))
                {
                    Tile t = GetTileBottom(currentTile);

                    if (!callingEntity.entityAdjacentTiles.Contains(t))
                    {
                        callingEntity.entityAdjacentTiles.Add(GetTileBottom(currentTile));
                    }
                }

                if (GetTileTop(currentTile))
                {
                    Tile t = GetTileTop(currentTile);

                    if (!callingEntity.entityAdjacentTiles.Contains(t))
                    {
                        callingEntity.entityAdjacentTiles.Add(GetTileTop(currentTile));
                    }
                }

                if (GetTileRight(currentTile))
                {
                    Tile t = GetTileRight(currentTile);

                    if (!callingEntity.entityAdjacentTiles.Contains(t))
                    {
                        callingEntity.entityAdjacentTiles.Add(GetTileRight(currentTile));
                    }
                }

                if (GetTileLeft(currentTile))
                {
                    Tile t = GetTileLeft(currentTile);

                    if (!callingEntity.entityAdjacentTiles.Contains(t))
                    {
                        callingEntity.entityAdjacentTiles.Add(GetTileLeft(currentTile));
                    }
                }
                break;

            case EdgeType.leftEdge:
                if (GetTileBottom(currentTile))
                {
                    Tile t = GetTileBottom(currentTile);

                    if (!callingEntity.entityAdjacentTiles.Contains(t))
                    {
                        callingEntity.entityAdjacentTiles.Add(GetTileBottom(currentTile));
                    }
                }

                if (GetTileTop(currentTile))
                {
                    Tile t = GetTileTop(currentTile);

                    if (!callingEntity.entityAdjacentTiles.Contains(t))
                    {
                        callingEntity.entityAdjacentTiles.Add(GetTileTop(currentTile));
                    }
                }

                if (GetTileRight(currentTile))
                {
                    Tile t = GetTileRight(currentTile);

                    if (!callingEntity.entityAdjacentTiles.Contains(t))
                    {
                        callingEntity.entityAdjacentTiles.Add(GetTileRight(currentTile));
                    }
                }
                break;

            case EdgeType.rightEdge:
                if (GetTileLeft(currentTile))
                {
                    Tile t = GetTileLeft(currentTile);

                    if (!callingEntity.entityAdjacentTiles.Contains(t))
                    {
                        callingEntity.entityAdjacentTiles.Add(GetTileLeft(currentTile));
                    }
                }

                if (GetTileBottom(currentTile))
                {
                    Tile t = GetTileBottom(currentTile);

                    if (!callingEntity.entityAdjacentTiles.Contains(t))
                    {
                        callingEntity.entityAdjacentTiles.Add(GetTileBottom(currentTile));
                    }
                }

                if (GetTileTop(currentTile))
                {
                    Tile t = GetTileTop(currentTile);

                    if (!callingEntity.entityAdjacentTiles.Contains(t))
                    {
                        callingEntity.entityAdjacentTiles.Add(GetTileTop(currentTile));
                    }
                }
                break;

            case EdgeType.bottomEdge:
                if (GetTileTop(currentTile))
                {
                    Tile t = GetTileTop(currentTile);

                    if (!callingEntity.entityAdjacentTiles.Contains(t))
                    {
                        callingEntity.entityAdjacentTiles.Add(GetTileTop(currentTile));
                    }
                }

                if (GetTileRight(currentTile))
                {
                    Tile t = GetTileRight(currentTile);

                    if (!callingEntity.entityAdjacentTiles.Contains(t))
                    {
                        callingEntity.entityAdjacentTiles.Add(GetTileRight(currentTile));
                    }
                }

                if (GetTileLeft(currentTile))
                {
                    Tile t = GetTileLeft(currentTile);

                    if (!callingEntity.entityAdjacentTiles.Contains(t))
                    {
                        callingEntity.entityAdjacentTiles.Add(GetTileLeft(currentTile));
                    }
                }
                break;

            case EdgeType.topEdge:
                if (GetTileRight(currentTile))
                {
                    Tile t = GetTileRight(currentTile);

                    if (!callingEntity.entityAdjacentTiles.Contains(t))
                    {
                        callingEntity.entityAdjacentTiles.Add(GetTileRight(currentTile));
                    }
                }

                if (GetTileLeft(currentTile))
                {
                    Tile t = GetTileLeft(currentTile);

                    if (!callingEntity.entityAdjacentTiles.Contains(t))
                    {
                        callingEntity.entityAdjacentTiles.Add(GetTileLeft(currentTile));
                    }
                }

                if (GetTileBottom(currentTile))
                {
                    Tile t = GetTileBottom(currentTile);

                    if (!callingEntity.entityAdjacentTiles.Contains(t))
                    {
                        callingEntity.entityAdjacentTiles.Add(GetTileBottom(currentTile));
                    }
                }
                break;

            case EdgeType.topRightEdge:
                if (GetTileLeft(currentTile))
                {
                    Tile t = GetTileLeft(currentTile);

                    if (!callingEntity.entityAdjacentTiles.Contains(t))
                    {
                        callingEntity.entityAdjacentTiles.Add(GetTileLeft(currentTile));
                    }
                }

                if (GetTileBottom(currentTile))
                {
                    Tile t = GetTileBottom(currentTile);

                    if (!callingEntity.entityAdjacentTiles.Contains(t))
                    {
                        callingEntity.entityAdjacentTiles.Add(GetTileBottom(currentTile));
                    }
                }

                break;

            case EdgeType.topLeftEdge:
                if (GetTileRight(currentTile))
                {
                    Tile t = GetTileRight(currentTile);

                    if (!callingEntity.entityAdjacentTiles.Contains(t))
                    {
                        callingEntity.entityAdjacentTiles.Add(GetTileRight(currentTile));
                    }
                }

                if (GetTileBottom(currentTile))
                {
                    Tile t = GetTileBottom(currentTile);

                    if (!callingEntity.entityAdjacentTiles.Contains(t))
                    {
                        callingEntity.entityAdjacentTiles.Add(GetTileBottom(currentTile));
                    }
                }
                break;

            case EdgeType.bottomRightEdge:
                if (GetTileLeft(currentTile))
                {
                    Tile t = GetTileLeft(currentTile);

                    if (!callingEntity.entityAdjacentTiles.Contains(t))
                    {
                        callingEntity.entityAdjacentTiles.Add(GetTileLeft(currentTile));
                    }
                }

                if (GetTileTop(currentTile))
                {
                    Tile t = GetTileTop(currentTile);

                    if (!callingEntity.entityAdjacentTiles.Contains(t))
                    {
                        callingEntity.entityAdjacentTiles.Add(GetTileTop(currentTile));
                    }
                }
                break;

            case EdgeType.bottomLeftEdge:
                if (GetTileTop(currentTile))
                {
                    Tile t = GetTileTop(currentTile);

                    if (!callingEntity.entityAdjacentTiles.Contains(t))
                    {
                        callingEntity.entityAdjacentTiles.Add(GetTileTop(currentTile));
                    }
                }

                if (GetTileRight(currentTile))
                {
                    Tile t = GetTileRight(currentTile);

                    if (!callingEntity.entityAdjacentTiles.Contains(t))
                    {
                        callingEntity.entityAdjacentTiles.Add(GetTileRight(currentTile));
                    }
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

            if (!tile.isWaterTile)
            { 
                tile.canBeSelectedSprite.GetComponent<SpriteRenderer>().sortingOrder = tile.GetComponent<SpriteRenderer>().sortingOrder + 1; // NEW
                tile.selectedSprite.GetComponent<SpriteRenderer>().sortingOrder = tile.GetComponent<SpriteRenderer>().sortingOrder + 2; // NEW
            }

            tile.name = "tile" + i;
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

    public void SetInteractableTilesDisplay(Entity callingEntity) // NEW
    {
        foreach (Tile t in callingEntity.entityAdjacentTiles)
        {
            if (!t.isGooPiece)
            {
                tileDisplayManager.SetTileCanBeSelectedDisplayON(t);
            }
        }
    }
    public void SetInteractableTilesDisplayOFF() // NEW
    {
        foreach (Tile t in allTilesInLevel)
        {
            if (!t.isWaterTile)
            {
                tileDisplayManager.SetTileCanBeSelectedDisplayOFF(t);
            }
        }
    }

    public void SetTileFull(Tile t) // NEW
    {
        t.isFull = !t.isFull;
    }

    public void LeaveGooOnTile(Tile gooTile)
    {
        // goo logic here
    }

    public List<Tile> GetNeighbours(Tile currentTile, Entity callingEntity) // NEW
    {
        int w = LevelEditor.instance.levelMap.texture.width;
        int h = LevelEditor.instance.levelMap.texture.height;

        List<Tile> neighbours = new List<Tile>();

        switch (currentTile.edgeType)
        {
            case EdgeType.notEdge:
                if (GetTileBottom(currentTile))
                {
                    Tile t = GetTileBottom(currentTile);

                    neighbours.Add(t);
                }

                if (GetTileTop(currentTile))
                {
                    Tile t = GetTileTop(currentTile);

                    neighbours.Add(t);
                }

                if (GetTileRight(currentTile))
                {
                    Tile t = GetTileRight(currentTile);
                    neighbours.Add(t);
                }

                if (GetTileLeft(currentTile))
                {
                    Tile t = GetTileLeft(currentTile);
                    neighbours.Add(t);
                }
                break;

            case EdgeType.leftEdge:
                if (GetTileBottom(currentTile))
                {
                    Tile t = GetTileBottom(currentTile);

                    neighbours.Add(t);
                }

                if (GetTileTop(currentTile))
                {
                    Tile t = GetTileTop(currentTile);

                    neighbours.Add(t);
                }

                if (GetTileRight(currentTile))
                {
                    Tile t = GetTileRight(currentTile);
                    neighbours.Add(t);
                }
                break;

            case EdgeType.rightEdge:
                if (GetTileLeft(currentTile))
                {
                    Tile t = GetTileLeft(currentTile);
                    neighbours.Add(t);
                }

                if (GetTileBottom(currentTile))
                {
                    Tile t = GetTileBottom(currentTile);
                    neighbours.Add(t);

                }

                if (GetTileTop(currentTile))
                {
                    Tile t = GetTileTop(currentTile);
                    neighbours.Add(t);

                }
                break;

            case EdgeType.bottomEdge:
                if (GetTileTop(currentTile))
                {
                    Tile t = GetTileTop(currentTile);
                    neighbours.Add(t);

                }

                if (GetTileRight(currentTile))
                {
                    Tile t = GetTileRight(currentTile);
                    neighbours.Add(t);

                }

                if (GetTileLeft(currentTile))
                {
                    Tile t = GetTileLeft(currentTile);
                    neighbours.Add(t);
                }
                break;

            case EdgeType.topEdge:
                if (GetTileRight(currentTile))
                {
                    Tile t = GetTileRight(currentTile);
                    neighbours.Add(t);
                }

                if (GetTileLeft(currentTile))
                {
                    Tile t = GetTileLeft(currentTile);
                    neighbours.Add(t);
                }

                if (GetTileBottom(currentTile))
                {
                    Tile t = GetTileBottom(currentTile);
                    neighbours.Add(t);

                }
                break;

            case EdgeType.topRightEdge:
                if (GetTileLeft(currentTile))
                {
                    Tile t = GetTileLeft(currentTile);
                    neighbours.Add(t);

                }

                if (GetTileBottom(currentTile))
                {
                    Tile t = GetTileBottom(currentTile);
                    
                    neighbours.Add(t);
                }

                break;

            case EdgeType.topLeftEdge:
                if (GetTileRight(currentTile))
                {
                    Tile t = GetTileRight(currentTile);

                    neighbours.Add(t);
                }

                if (GetTileBottom(currentTile))
                {
                    Tile t = GetTileBottom(currentTile);
                    neighbours.Add(t);
                }
                break;

            case EdgeType.bottomRightEdge:
                if (GetTileLeft(currentTile))
                {
                    Tile t = GetTileLeft(currentTile);
                    neighbours.Add(t);
                }

                if (GetTileTop(currentTile))
                {
                    Tile t = GetTileTop(currentTile);
                    neighbours.Add(t);

                }
                break;

            case EdgeType.bottomLeftEdge:
                if (GetTileTop(currentTile))
                {
                    Tile t = GetTileTop(currentTile);
                    neighbours.Add(t);

                }

                if (GetTileRight(currentTile))
                {
                    Tile t = GetTileRight(currentTile);
                    neighbours.Add(t);

                }
                break;

            default:
                break;
        }


        return neighbours;
    }

    public void FillallEdgeTileInLevelList()
    {
        foreach (Tile tile in allTilesInLevel)
        {
            if (!tile.isFull)
            {
                if(tile.edgeType != EdgeType.notEdge)
                {
                    allEdgeTileInLevel.Add(tile);
                }
            }
        }
    }
    public void OnDrawGizmos()
    {
        Debug.Log("Drawing Gizmos");

        //foreach (Tile t in allTilesInLevel)
        //{
        //    Gizmos.color = Color.red;

        //    if (path != null)
        //    {
        //        if (path.Contains(t))
        //        {
        //            Gizmos.color = Color.black;
        //        }
        //        else
        //        {
        //            Gizmos.color = Color.grey;
        //        }
        //    }
            
        //    Gizmos.DrawCube(new Vector3(t.transform.position.x, t.transform.position.y + 0.592f, t.transform.position.z), Vector3.one / 2);
        //}

        foreach (Tile t in allEdgeTileInLevel)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawCube(new Vector3(t.transform.position.x, t.transform.position.y + 0.592f, t.transform.position.z), Vector3.one / 2);

        }
    }
}
