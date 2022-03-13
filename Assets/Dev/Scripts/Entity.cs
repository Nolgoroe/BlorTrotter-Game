using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public enum AnimationType { Move, Hurt, Teleport}
public enum MoveDirection { up, down, left, right}
public abstract class Entity : MonoBehaviour // abstract class for inheritance and polymorphism
{

    public List<Tile> gooTiles;
    public Tile currentTile;
    public List<Tile> entityAdjacentTiles;
    public List<Tile> enemyPath;
    public Animator anim;

    public virtual async Task MoveEntity(Tile targetTile) // can be override
    {
        await Task.Yield();
    }

    #region   
    public virtual void SetTargetTileForAstarPath() 
    {
        Debug.Log("not a player or enemy - problem");
    }



    #endregion


    public virtual async void PlayAnimation(AnimationType animType) //new
    {
        Debug.Log("what happened?");
        await Task.Delay(300);

    }

    public virtual void PrepareToMove(Tile targetTile) //new
    {
        Debug.Log("what happened?");
    }
    public virtual void DetectMoveDirection(Tile from, Tile TileTo) //new
    {
        Debug.Log("what happened?");
    }
    public abstract void AddGooTiles(Tile gooTile);
    public abstract void RemoveGooTiles(Tile gooTile);
    public abstract void ManageTurnStart();
    public abstract void SetCurrentTile(Tile tileOn);
}
