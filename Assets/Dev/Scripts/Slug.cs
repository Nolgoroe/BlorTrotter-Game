using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class Slug : Entity
{
    public override async Task MoveEntity(Tile targetTile)
    {
        await Task.Delay(3 * 1000);  // *1000 because Delay is in miliseconds

        Debug.Log("ENEMY DONE");
    }

    public override void PlayAnimation()
    {
        /// Set Slug animation Data here
    }
    public override void SetTargetTile()
    {
        bool tileAvailable = false;
        tileAvailable = GridManager.instance.CheckTileAvailability(currentTile);
        // randomize available tile, chose one
        // currentTile = ?
        //each slugs's this will reference a different slug
    }

    public override void AddGooTiles(Tile gooTile)
    {
        gooTiles.Add(gooTile);
    }

    public override void ManageTurnStart()
    {
        throw new System.NotImplementedException();
    }

    public override void SetCurrentTile(Tile tileOn)
    {
        currentTile = tileOn;
    }

}
