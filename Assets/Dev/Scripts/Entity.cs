using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public abstract class Entity : MonoBehaviour // abstract class for inheritance and polymorphism
{

    public List<Tile> gooTiles;
    public Tile currentTile;
    public List<Tile> entityAdjacentTiles;
    public virtual async Task MoveEntity(Tile targetTile) // can be override
    {
        await Task.Yield();
    }

    #region   
    public virtual void SetTargetTile() 
    {
        Debug.Log("not a player or enemy - problem");
        
    }

    public abstract void SetCurrentTile(Tile tileOn);
   


    #endregion


    public abstract void PlayAnimation();
    public abstract void AddGooTiles(Tile gooTile);
    public abstract void ManageTurnStart();




}
    