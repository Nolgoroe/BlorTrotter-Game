using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class Player : Entity
{
    public static bool isPlayerTurn;

    private MoveDirection currentMoveDirection = MoveDirection.down;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public override void PrepareToMove(Tile targetTile) //new
    {
        currentTile.isMainPlayerBody = false;
        GridManager.instance.SetTileFull(currentTile);

        currentTile = targetTile;

        currentTile.isMainPlayerBody = true;
        GridManager.instance.SetTileFull(currentTile);

        PlayAnimation(AnimationType.Teleport);

        ManageTurnStart();
    }

    public override async Task MoveEntity(Tile targetTile) 
    {
        isPlayerTurn = false;

        GridManager.instance.SetTileFull(currentTile);

        currentTile.isGooPiece = true;
        targetTile.isGooPiece = true;

        currentTile.isMainPlayerBody = false;

        if (!gooTiles.Contains(currentTile)) //new
        {
            AddGooTiles(currentTile);
        }

        if (!gooTiles.Contains(targetTile)) //new
        {
            AddGooTiles(targetTile);
        }

        // function to leave goo behind here
        foreach (Tile element in gooTiles)
        {
            GridManager.instance.LeaveGooOnTile(element);
        }

        DetectMoveDirection(currentTile, targetTile);

        await CheckWhatIsNextTile(currentTile, targetTile);

        currentTile = targetTile;
        currentTile.isMainPlayerBody = true;
        currentTile.isAdjacentToMainBody = false;


        await PlayAnimation(AnimationType.Move);


        if (targetTile.isLightTile)
        {
            LevelManager.instance.DecreaseNumberOfMoves(3);
            await PlayAnimation(AnimationType.Hurt);
        }
        else
        {
            LevelManager.instance.DecreaseNumberOfMoves(1);
        }

        //transform.position = new Vector3(currentTile.transform.position.x, currentTile.transform.position.y + (LevelEditor.instance.offsetY * 2), currentTile.transform.position.z);

        //if (targetTile.isGooPiece)
        //{
        //    EntityManager.instance.SetPlayerTurn();
        //}

        if (!targetTile.isFood && !targetTile.isKinine && !targetTile.isSalt)
        {
            GridManager.instance.SetTileFull(targetTile); ///before, it was just under the setTileFull(CurrentTile)
        }
        else
        {
            EatFood(targetTile, (targetTile.isKinine || targetTile.isSalt));
        }

        LevelManager.instance.CheckLoseLevel();

        await Task.Delay(500);
    }

    public override async Task PlayAnimation(AnimationType animType) //new
    {
        switch (animType)
        {
            case AnimationType.Move:

                await Task.Delay(300);

                Vector3 targetVector = new Vector3(currentTile.transform.position.x, currentTile.transform.position.y + (LevelEditor.instance.offsetY * 2), currentTile.transform.position.z);
                //Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetVector, 10f, 0.0f);
                //transform.rotation = Quaternion.Euler(newDirection);

                LeanTween.move(gameObject, targetVector, 0.1f);
                GetComponent<SpriteRenderer>().sortingOrder = currentTile.GetComponent<SpriteRenderer>().sortingOrder + 1;
                break;
            case AnimationType.Hurt:
                anim.SetBool("isHurting", true);
                break;
            case AnimationType.Teleport:
                anim.SetBool("isRetracting", true);

                await Task.Delay(1000);

                GetComponent<SpriteRenderer>().sortingOrder = currentTile.GetComponent<SpriteRenderer>().sortingOrder + 1;
                transform.position = new Vector3(currentTile.transform.position.x, currentTile.transform.position.y + (LevelEditor.instance.offsetY * 2), currentTile.transform.position.z);

                break;
            case AnimationType.Win:
                anim.SetBool("hasWon", true);
                break;
            default:
                break;
        }
        /// Set player animation Data here
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

    public override void DetectMoveDirection(Tile from, Tile TileTo) //new
    {
        if(TileTo.tileY < from.tileY)
        {
            currentMoveDirection = MoveDirection.down;

            Vector3 rotation = new Vector3(0,0,0);
            transform.rotation = Quaternion.Euler(rotation);

            Debug.Log("Down");
        }
        else if (TileTo.tileY > from.tileY)
        {
            currentMoveDirection = MoveDirection.up;

            Vector3 rotation = new Vector3(0, 0, 0);
            transform.rotation = Quaternion.Euler(rotation);

            Debug.Log("up");

        }
        else if (TileTo.tileX < from.tileX)
        {
            currentMoveDirection = MoveDirection.left;

            Vector3 rotation = new Vector3(0, 180, 0);
            transform.rotation = Quaternion.Euler(rotation);

            Debug.Log("left");

        }
        else if (TileTo.tileX > from.tileX)
        {
            currentMoveDirection = MoveDirection.right;

            Vector3 rotation = new Vector3(0, 180, 0);
            transform.rotation = Quaternion.Euler(rotation);

            Debug.Log("right");

        }
    }

    public override async Task CheckWhatIsNextTile(Tile from, Tile TileTo)
    {
        if (currentMoveDirection == MoveDirection.left || currentMoveDirection == MoveDirection.down)
        {
            if (TileTo.isFood || TileTo.isKinine || TileTo.isSalt)
            {
                anim.SetBool("isEating", true);
                await Task.Delay(125);
            }
            else
            {
                anim.SetBool("Crawl_Anim_Down_Left", true);
            }

        }
        else
        {
            if (TileTo.isFood || TileTo.isKinine || TileTo.isSalt)
            {
                anim.SetBool("isEatingBack", true);
                await Task.Delay(125);
            }
            else
            {
                anim.SetBool("CrawlBack_Anim_Up_Right", true);
            }
        }
    }

    void EatFood(Tile target, bool isKinineOrSalt)
    {
        target.isFood = false;

        if (isKinineOrSalt)
        {
            Animator anim = target.foodObject.GetComponent<Animator>();
            anim.SetBool("Absorb", true);

            if (target.isKinine)
            {
                LevelManager.instance.ActivateKininePower();
            }

            if (target.isSalt)
            {
                LevelManager.instance.ActivateSaltPower();
            }

            target.isKinine = false;
            target.isSalt = false;

            LevelManager.instance.AddMovesEat(LevelManager.instance.currentLevel.amountToAddOnEatBlob);
        }
        else
        {
            Destroy(target.foodObject.gameObject);

            LevelManager.instance.AddMovesEat(LevelManager.instance.currentLevel.amountToAddOnEatFood);
            ScoreManager.instance.currentCollectedFood++;
            UIManager.instance.UpdateFoodAmount();

            LevelManager.instance.CheckWinLevel();
        }

        target.foodObject = null;

    }
}
