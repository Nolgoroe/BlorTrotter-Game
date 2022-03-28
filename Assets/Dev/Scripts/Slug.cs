using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;

public class Slug : Entity
{
    public Tile PublicTargetTile;

    public MoveDirection currentMoveDirection = MoveDirection.left;

    public GameObject DownArrowPrefab;
    public GameObject UpArrowPrefab;
    public GameObject LeftArrowPrefab;
    public GameObject RightArrowPrefab;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public override async Task MoveEntity(Tile targetTile)
    {
        if (!targetTile.isBeetleForTutorial && !targetTile.isSlugBody)
        {
            GridManager.instance.SetTileFull(currentTile);

            currentTile.isEnemyGooPiece = true;
            targetTile.isEnemyGooPiece = true;
            currentTile.isSlugBody = false;
            targetTile.isSlugBody = true;

            //currentTile.turnsUntilEnemyGooDissappears = 3;
            targetTile.turnsUntilEnemyGooDissappears = 3;

            if (!GridManager.instance.allEnemyGooTiles.Contains(currentTile))
            {
                GridManager.instance.allEnemyGooTiles.Add(currentTile);
            }

            if (!GridManager.instance.allEnemyGooTiles.Contains(targetTile))
            {
                GridManager.instance.allEnemyGooTiles.Add(targetTile);
            }

            DetectMoveDirection(currentTile, targetTile);

            if (!gooTiles.Contains(currentTile))
            {
                gooTiles.Add(currentTile);
            }

            if (!gooTiles.Contains(targetTile))
            {
                gooTiles.Add(targetTile);
            }


            await Task.Delay(300);

            CheckWhatIsNextTile(currentTile, targetTile);



            await Task.Delay(300);

            foreach (Tile element in gooTiles)
            {
                GridManager.instance.LeaveGooOnTileEnemy(element);
            }


            currentTile = targetTile;

            PlayAnimation(AnimationType.Move);

            GridManager.instance.SetTileFull(targetTile);


            if (currentTile.isGooPiece)
            {
                await EatGooPiece(targetTile);
            }

            enemyPath.RemoveAt(0);


            await Task.Delay(100);
            if (enemyPath.Count > 0)
            {
                CalculateDirectionNextTile(currentTile, enemyPath[0], this);
            }
            else
            {
                TurnOffAllEnemyArrows();
            }
            //Debug.Log("ENEMY DONE");
        }
        else
        {
            Debug.LogError("Target Is Enemy");

            return;
        }
    }

    public override async Task PlayAnimation(AnimationType animType)
    {
        Vector3 targetVector = new Vector3(currentTile.transform.position.x, currentTile.transform.position.y + (LevelEditor.instance.offsetY * 2), currentTile.transform.position.z);

        LeanTween.move(gameObject, targetVector, 0.05f);
        GetComponent<SpriteRenderer>().sortingOrder = currentTile.GetComponent<SpriteRenderer>().sortingOrder + 1;

        SoundManager.instance.PlaySound(SoundManager.instance.SFXAudioSource, Sounds.Slug_Moving);

        await Task.Yield();
    }

    public override void SetTargetTileForAstarPath()
    {
        int randomIndex = UnityEngine.Random.Range(0, GridManager.instance.allEdgeTileInLevel.Count);
        Tile targetTile = GridManager.instance.allEdgeTileInLevel[randomIndex];

        PublicTargetTile = targetTile;

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

    public override async Task ManageTurnStart()
    {
        if (enemyPath != null)
        {
            if (enemyPath.Count <= 0)
            {
                ReleaseTargetTile();
                await EntityManager.instance.RemoveEnemyFromList(this, EntityManager.instance.allEnemies);

                await Task.Delay(1 * 1000);
                Destroy(transform.parent.gameObject);

                currentTile.isFull = false;
                currentTile.isSlugBody = false;
            }
        }
        else
        {
            currentTile.isFull = false;
            currentTile.isSlugBody = false;

            await EntityManager.instance.RemoveEnemyFromList(this, EntityManager.instance.allEnemies);

            await Task.Delay(1 * 1000);
            Destroy(transform.parent.gameObject);
        }

        if (enemyPath != null)
        {
            if (enemyPath.Count > 0)
            {
                await MoveEntity(enemyPath[0]);
            }
        }
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

            ///Debug.Log("Down");
        }
        else if (TileTo.tileY > from.tileY)
        {
            currentMoveDirection = MoveDirection.up;

            anim.SetBool("isTurned", true);

            await Task.Delay(100);

            Vector3 rotation = new Vector3(0, 0, 0);
            transform.rotation = Quaternion.Euler(rotation);

            //Debug.Log("up");

        }
        else if (TileTo.tileX < from.tileX)
        {
            currentMoveDirection = MoveDirection.left;

            anim.SetBool("isTurned", false);

            await Task.Delay(100);

            Vector3 rotation = new Vector3(0, 0, 0);
            transform.rotation = Quaternion.Euler(rotation);

            //Debug.Log("left");

        }
        else if (TileTo.tileX > from.tileX)
        {
            currentMoveDirection = MoveDirection.right;

            anim.SetBool("isTurned", true);

            await Task.Delay(100);

            Vector3 rotation = new Vector3(0, 180, 0);
            transform.rotation = Quaternion.Euler(rotation);
       
            //Debug.Log("right");

        }
    }

    public override async Task CheckWhatIsNextTile(Tile from, Tile TileTo)
    {
        if (currentMoveDirection == MoveDirection.left || currentMoveDirection == MoveDirection.down)
        {
            if (TileTo.isGooPiece || TileTo.isMainPlayerBody)
            {
                anim.SetBool("isEating", true);
            }
            else
            {
                anim.SetBool("isMoving", true);
            }
        }
        else
        {
            if (TileTo.isGooPiece || TileTo.isMainPlayerBody)
            {
                anim.SetBool("isEating", true);
            }
            else
            {
                anim.SetBool("isMovingBack", true);
            }
        }

        await Task.Yield();
    }

    public async Task EatGooPiece(Tile target)
    {
        target.isGooPiece = false;
        GridManager.instance.RemoveGooTileDisplay(target);
        EntityManager.instance.GetPlayer().RemoveGooTiles(target);

        await EntityManager.instance.GetPlayer().PlayAnimation(AnimationType.Hurt);

        await Task.Delay(750);
        if (target.isMainPlayerBody)
        {
            target.isMainPlayerBody = false;
            await EntityManager .instance.SpawnPlayerRandomGooLocation();
        }
    }

    public override void ReleaseTargetTile()
    {
        PublicTargetTile.isFull = false;
    }


    public void TurnOffAllEnemyArrows()
    {
        RightArrowPrefab.SetActive(false);
        LeftArrowPrefab.SetActive(false);
        DownArrowPrefab.SetActive(false);
        UpArrowPrefab.SetActive(false);

    }


    public void CalculateDirectionNextTile(Tile from, Tile TileTo, Slug calling)
    {
        calling.TurnOffAllEnemyArrows();

        if (TileTo.tileY < from.tileY)
        {
            calling.DownArrowPrefab.SetActive(true);

            calling.DownArrowPrefab.transform.position = new Vector3(calling.transform.position.x + 1, calling.transform.position.y - 1, calling.transform.position.z);
            Debug.Log("Down");
        }
        else if (TileTo.tileY > from.tileY)
        {
            calling.UpArrowPrefab.SetActive(true);

            calling.UpArrowPrefab.transform.position = new Vector3(calling.transform.position.x - 1, calling.transform.position.y, calling.transform.position.z);
            Debug.Log("up");

        }
        else if (TileTo.tileX < from.tileX)
        {
            calling.LeftArrowPrefab.SetActive(true);

            calling.LeftArrowPrefab.transform.position = new Vector3(calling.transform.position.x - 1, calling.transform.position.y - 1, calling.transform.position.z);

            Debug.Log("left");

        }
        else if (TileTo.tileX > from.tileX)
        {
            calling.RightArrowPrefab.SetActive(true);

            calling.RightArrowPrefab.transform.position = new Vector3(calling.transform.position.x + 1, calling.transform.position.y, calling.transform.position.z);

            Debug.Log("right");

        }
    }
}
