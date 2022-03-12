using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;

public class Slug : Entity
{
    public Tile tempPublictargetTile;

    public MoveDirection currentMoveDirection = MoveDirection.left;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public override async Task MoveEntity(Tile targetTile)
    {
        GridManager.instance.SetTileFull(currentTile);
        GridManager.instance.SetTileFull(targetTile);


        DetectMoveDirection(currentTile, targetTile);

        currentTile = targetTile;

        //transform.position = new Vector3(currentTile.transform.position.x, currentTile.transform.position.y + (LevelEditor.instance.offsetY * 2), currentTile.transform.position.z);
        //GetComponent<SpriteRenderer>().sortingOrder = targetTile.GetComponent<SpriteRenderer>().sortingOrder + 1;

        PlayAnimation(AnimationType.Move);

        await Task.Delay(100);

        if (currentTile.isGooPiece)
        {
            EatGooPiece(targetTile);
        }


        enemyPath.RemoveAt(0);

        //Debug.Log("ENEMY DONE");
    }

    public override async void PlayAnimation(AnimationType animType)
    {
        switch (animType)
        {
            case AnimationType.Move:

                await Task.Delay(300);

                Vector3 targetVector = new Vector3(currentTile.transform.position.x, currentTile.transform.position.y + (LevelEditor.instance.offsetY * 2), currentTile.transform.position.z);

                LeanTween.move(gameObject, targetVector, 0.05f);
                GetComponent<SpriteRenderer>().sortingOrder = currentTile.GetComponent<SpriteRenderer>().sortingOrder + 1;
                break;
            case AnimationType.Hurt:
                break;
            case AnimationType.Eat:
                break;
            default:
                break;
        }
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
    public override void RemoveGooTiles(Tile gooTile)
    {
        gooTiles.Remove(gooTile);
    }

    public override void ManageTurnStart()
    {
    }

    public override void SetCurrentTile(Tile tileOn)
    {
        currentTile = tileOn;
    }

    public override async void DetectMoveDirection(Tile from, Tile TileTo) //new
    {
        if (TileTo.tileY < from.tileY)
        {
            currentMoveDirection = MoveDirection.down;

            anim.SetBool("isTurned", false);

            await Task.Delay(100);

            Vector3 rotation = new Vector3(0, 180, 0);
            transform.rotation = Quaternion.Euler(rotation);

            if (TileTo.isGooPiece || TileTo.isMainPlayerBody)
            {
                anim.SetBool("isEating", true);
            }
            else
            {
                anim.SetBool("isMoving", true);
            }

            Debug.Log("Down");
        }
        else if (TileTo.tileY > from.tileY)
        {
            currentMoveDirection = MoveDirection.up;

            anim.SetBool("isTurned", true);

            await Task.Delay(100);

            Vector3 rotation = new Vector3(0, 0, 0);
            transform.rotation = Quaternion.Euler(rotation);

            if (TileTo.isGooPiece || TileTo.isMainPlayerBody)
            {
                anim.SetBool("isEating", true);
            }
            else
            {
                anim.SetBool("isMovingBack", true);
            }



            Debug.Log("up");

        }
        else if (TileTo.tileX < from.tileX)
        {
            currentMoveDirection = MoveDirection.left;

            anim.SetBool("isTurned", false);

            await Task.Delay(100);

            Vector3 rotation = new Vector3(0, 0, 0);
            transform.rotation = Quaternion.Euler(rotation);

            if (TileTo.isGooPiece || TileTo.isMainPlayerBody)
            {
                anim.SetBool("isEating", true);
            }
            else
            {
                anim.SetBool("isMoving", true);
            }


            Debug.Log("left");

        }
        else if (TileTo.tileX > from.tileX)
        {
            currentMoveDirection = MoveDirection.right;

            anim.SetBool("isTurned", true);

            await Task.Delay(100);

            Vector3 rotation = new Vector3(0, 180, 0);
            transform.rotation = Quaternion.Euler(rotation);

            

            if (TileTo.isGooPiece || TileTo.isMainPlayerBody)
            {
                anim.SetBool("isEating", true);
            }
            else
            {
                anim.SetBool("isMovingBack", true);
            }


            Debug.Log("right");

        }
    }

    void EatGooPiece(Tile target)
    {
        target.isGooPiece = false;
        GridManager.instance.RemoveGooTileDisplay(target);
        EntityManager.instance.GetPlayer().RemoveGooTiles(target);

        if (target.isMainPlayerBody)
        {
            EntityManager.instance.SpawnPlayerRandomGooLocation();
        }
    }
}
