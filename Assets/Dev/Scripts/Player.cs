using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class Player : Entity
{
    public static bool isPlayerTurn; // NEW

    public override async Task MoveEntity(Tile targetTile) // NEW
    {
        isPlayerTurn = false;

        GridManager.instance.SetTileFull(currentTile);
        GridManager.instance.SetTileFull(targetTile);

        currentTile.isGooPiece = true;

        // Call some function to leave goo behind here
        GridManager.instance.LeaveGooOnTile(currentTile);

        currentTile = targetTile;

        transform.position = new Vector3(currentTile.transform.position.x, currentTile.transform.position.y + (LevelEditor.instance.offsetY * 2), currentTile.transform.position.z);
        GetComponent<SpriteRenderer>().sortingOrder = targetTile.GetComponent<SpriteRenderer>().sortingOrder + 1;

        if (!targetTile.isGooPiece)
        {
            AddGooTiles(targetTile);
            LevelManager.instance.DecreaseNumberOfMoves();
        }
        else
        {
           EntityManager.instance.SetPlayerTurn();
        }

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

    public override void ManageTurnStart() // NEW
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
