using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;

public class Slug : Entity
{
    public Tile tempPublictargetTile;

    public override async Task MoveEntity(Tile targetTile)
    {
        GridManager.instance.SetTileFull(currentTile);
        GridManager.instance.SetTileFull(targetTile);

        await Task.Delay(1 * 1000);  // *1000 because Delay is in miliseconds

        currentTile = targetTile;

        transform.position = new Vector3(currentTile.transform.position.x, currentTile.transform.position.y + (LevelEditor.instance.offsetY * 2), currentTile.transform.position.z);
        GetComponent<SpriteRenderer>().sortingOrder = targetTile.GetComponent<SpriteRenderer>().sortingOrder + 1;

        enemyPath.RemoveAt(0);

        Debug.Log("ENEMY DONE");
    }

    public override void PlayAnimation()
    {
        /// Set Slug animation Data here
    }

    public override void SetTargetTileForAstarPath() 
    {      
        int randomIndex = UnityEngine.Random.Range(0, GridManager.instance.allEdgeTileInLevel.Count);
        Tile targetTile = GridManager.instance.allEdgeTileInLevel[randomIndex];

        tempPublictargetTile = targetTile;

        enemyPath = PathFinding.instance.FindPath(currentTile, targetTile, this);
    }
    public override void AddGooTiles(Tile gooTile)
    {
        gooTiles.Add(gooTile);
    }

    public override void ManageTurnStart()
    {
    }

    public override void SetCurrentTile(Tile tileOn)
    {
        currentTile = tileOn;
    }

}
