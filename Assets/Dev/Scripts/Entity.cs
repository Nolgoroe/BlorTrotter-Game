using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public enum AnimationType { Move, Hurt, Win, Teleport, Land, LiftOff}
public enum MoveDirection { up, down, left, right}
public abstract class Entity : MonoBehaviour // abstract class for inheritance and polymorphism
{

    public List<Tile> gooTiles;
    public Tile currentTile;
    public List<Tile> entityAdjacentTiles;
    public List<Tile> enemyPath;
    public Animator anim;
    public EntityTypes typeEntity;

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


    public virtual async Task PlayAnimation(AnimationType animType) //new
    {
        Debug.Log("what happened?");
        await Task.Delay(300);

    }

    public virtual async Task PrepareToMove(Tile targetTile) //new
    {
        await Task.Delay(300);

        Debug.Log("what happened?");
    }
    public virtual void DetectMoveDirection(Tile from, Tile TileTo) //new
    {
        Debug.Log("what happened?");
    }
    public virtual async Task CheckWhatIsNextTile(Tile from, Tile TileTo) //new
    {
        Debug.Log("what happened?");
        await Task.Delay(300);
    }
    public virtual void ReleaseTargetTile() //new
    {
        Debug.Log("what happened?");
    }
    public virtual void AddGooTiles(Tile gooTile)
    {

    }
    public virtual void RemoveGooTiles(Tile gooTile)
    {

    }
    public virtual async Task ManageTurnStart()
    {
        Debug.Log("what happened?");
        await Task.Delay(300);
    }
    public abstract void SetCurrentTile(Tile tileOn);
}
