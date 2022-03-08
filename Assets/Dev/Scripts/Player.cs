using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class Player : Entity
{
    public static bool isPlayerTurn; 

    public override async Task MoveEntity(Tile targetTile) 
    {
        isPlayerTurn = false;

        GridManager.instance.SetTileFull(currentTile);
        
        currentTile.isGooPiece = true;

        // Call some function to leave goo behind here
        
        GooManager.instance.playerGooList.Add(currentTile); /// good place to do it ?
        foreach (Tile element in GooManager.instance.playerGooList)
        {
            GridManager.instance.LeaveGooOnTile(element);
        }

        currentTile = targetTile;

        transform.position = new Vector3(currentTile.transform.position.x, currentTile.transform.position.y + (LevelEditor.instance.offsetY * 2), currentTile.transform.position.z);
        GetComponent<SpriteRenderer>().sortingOrder = currentTile.GetComponent<SpriteRenderer>().sortingOrder + 1;

        if (!targetTile.isGooPiece)
        {
            AddGooTiles(targetTile);
            LevelManager.instance.DecreaseNumberOfMoves();
        }
        else
        {
           EntityManager.instance.SetPlayerTurn();
        }

        GridManager.instance.SetTileFull(targetTile); ///before, it was just under the setTileFull(CurrentTile)

        await Task.Yield();
    }

    public override void PlayAnimation()
    {
        /// Set player animation Data here
    }

    public override void AddGooTiles(Tile gooTile)
    {
        gooTiles.Add(gooTile);
    }

    public override void ManageTurnStart() 
    {
        isPlayerTurn = true;

        entityAdjacentTiles.Clear();
        GridManager.instance.SetInteractableTilesDisplayOFF();

        foreach (Tile tile in gooTiles)
        {
            GridManager.instance.GetAdjacentTile(tile, this); // reference to the script it's attached to
        }

        GridManager.instance.SetInteractableTilesDisplay(this);
    }

    public override void SetCurrentTile(Tile tileOn)
    {
        currentTile = tileOn;
    }
}
