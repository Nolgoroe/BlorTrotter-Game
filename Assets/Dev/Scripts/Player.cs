using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class Player : Entity
{
    
    public override async Task MoveEntity(Tile targetTile)
    {
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
        foreach (Tile tile in gooTiles)
        {
            GridManager.instance.GetAdjacentTile(tile, this); // reference to the script it's attached to
        }
    }

    public override void SetCurrentTile(Tile tileOn)
    {
        currentTile = tileOn;
    }
}
