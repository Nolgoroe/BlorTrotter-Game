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

    public override async Task PrepareToMove(Tile targetTile) //new
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

        DetectMoveDirection(currentTile, targetTile);

        //await CheckWhatIsNextTile(currentTile, targetTile);



        if (targetTile.isKinine)
        {
            await targetTile.RotateEatenBlobDisplay(currentTile,targetTile);
        }
        else if (targetTile.isSalt)
        {
           await targetTile.RotateEatenBlobDisplay(currentTile, targetTile);
        }

        //await Task.Delay(10000);
        currentTile = targetTile;
        currentTile.isMainPlayerBody = true;
        currentTile.isAdjacentToMainBody = false;


        await PlayAnimation(AnimationType.Move);

        foreach (Tile element in gooTiles)
        {
            GridManager.instance.LeaveGooOnTile(element);
        }

        GridManager.instance.SetTileFull(currentTile);

        //transform.position = new Vector3(currentTile.transform.position.x, currentTile.transform.position.y + (LevelEditor.instance.offsetY * 2), currentTile.transform.position.z);

        //if (targetTile.isGooPiece)
        //{
        //    EntityManager.instance.SetPlayerTurn();
        //}

        LevelManager.instance.CheckLoseLevel();

        await Task.Delay(500);
    }

    public override async Task PlayAnimation(AnimationType animType) //new
    {
        switch (animType)
        {
            case AnimationType.Move:

                Vector3 targetVector = new Vector3(currentTile.transform.position.x, currentTile.transform.position.y + (LevelEditor.instance.offsetY * 2), currentTile.transform.position.z);
                GetComponent<SpriteRenderer>().sortingOrder = currentTile.GetComponent<SpriteRenderer>().sortingOrder + 1;

                if (!currentTile.isFood && !currentTile.isKinine && !currentTile.isSalt)
                {
                    CheckWhatIsNextTile(currentTile);

                    await Task.Delay(249);

                    CheckSoundPlayType();

                    await Task.Delay(83);

                    LeanTween.move(gameObject, targetVector, 0.1f);
                }
                else
                {
                    //SpriteRenderer foodObjectRenderer = currentTile.foodObject.GetComponent<SpriteRenderer>();
                    //int currentSorting = foodObjectRenderer.sortingOrder;

                        //foodObjectRenderer.sortingOrder = currentSorting - 1;


                    await Task.Delay(166);

                    EatFood(currentTile, (currentTile.isKinine || currentTile.isSalt));

                    await Task.Delay(166);

                    CheckSoundPlayType();

                    CheckWhatIsNextTile(currentTile);
                    await Task.Delay(480);

                    LeanTween.move(gameObject, targetVector, 0.210f);


                    currentTile.isFood = false;
                    currentTile.isFull = false;

                    if(!currentTile.isKinine && !currentTile.isSalt)
                    {
                        await Task.Delay(100);
                        Destroy(currentTile.foodObject.gameObject);

                        LevelManager.instance.CheckWinLevel();
                    }
                    else if (currentTile.isKinine)
                    {
                        currentTile.isKinine = false;
                    }
                    else if(currentTile.isSalt)
                    {
                        currentTile.isSalt = false;
                    }

                    currentTile.foodObject = null;

                    GetAdjacentcyData();
                }

                if (!LevelManager.instance.levelEnded)
                {
                    if (currentTile.isLightTile)
                    {
                        await Task.Delay(480);

                        LevelManager.instance.DecreaseNumberOfMoves(3);
                        await PlayAnimation(AnimationType.Hurt);
                    }
                    else
                    {
                        LevelManager.instance.DecreaseNumberOfMoves(1);
                    }
                }

                //await Task.Delay(400);
                //Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetVector, 10f, 0.0f);
                //transform.rotation = Quaternion.Euler(newDirection);

                break;
            case AnimationType.Hurt:
                anim.SetBool("isHurting", true);
                //await Task.Delay(250);

                SoundManager.instance.PlaySound(SoundManager.instance.SFXAudioSource, Sounds.Blob_Hurt);

                break;
            case AnimationType.Teleport:
                anim.SetBool("isRetracting", true);

                await Task.Delay(1100);

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

    private void CheckSoundPlayType()
    {
        if (!currentTile.isFood && !currentTile.isKinine && !currentTile.isSalt)
        {
            SoundManager.instance.PlaySound(SoundManager.instance.SFXAudioSource, Sounds.Blob_Moving_Spawning);
        }
        else
        {
            SoundManager.instance.PlaySound(SoundManager.instance.SFXAudioSource, Sounds.Blob_Eat_Absorb);
        }
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
        isPlayerTurn = true;

        entityAdjacentTiles.Clear();
        GridManager.instance.SetInteractableTilesDisplayOFF();

        foreach (Tile tile in gooTiles)
        {
            GridManager.instance.GetAdjacentTile(tile, this); // reference to the script it's attached to
        }

        GridManager.instance.SetInteractableTilesDisplay(this);

        LevelManager.instance.CheckLoseLevel();

        await Task.Yield();
    }

    public void GetAdjacentcyData()
    {

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

            //Debug.Log("Down");
        }
        else if (TileTo.tileY > from.tileY)
        {
            currentMoveDirection = MoveDirection.up;

            Vector3 rotation = new Vector3(0, 0, 0);
            transform.rotation = Quaternion.Euler(rotation);

            //Debug.Log("up");

        }
        else if (TileTo.tileX < from.tileX)
        {
            currentMoveDirection = MoveDirection.left;

            Vector3 rotation = new Vector3(0, 180, 0);
            transform.rotation = Quaternion.Euler(rotation);

            //Debug.Log("left");

        }
        else if (TileTo.tileX > from.tileX)
        {
            currentMoveDirection = MoveDirection.right;

            Vector3 rotation = new Vector3(0, 180, 0);
            transform.rotation = Quaternion.Euler(rotation);

            //Debug.Log("right");

        }
    }

    public void CheckWhatIsNextTile(Tile tile)
    {
        if (currentMoveDirection == MoveDirection.left || currentMoveDirection == MoveDirection.down)
        {
            if (tile.isFood || tile.isKinine || tile.isSalt)
            {
                
                anim.SetBool("isEating", true);
            }
            else
            {
                anim.SetBool("Crawl_Anim_Down_Left", true);
            }

        }
        else
        {
            if (tile.isFood || tile.isKinine || tile.isSalt)
            {
                

                anim.SetBool("isEatingBack", true);
            }
            else
            {
                anim.SetBool("CrawlBack_Anim_Up_Right", true);
            }
        }
    }

    async void EatFood(Tile target, bool isKinineOrSalt)
    {
        if (isKinineOrSalt)
        {
            Animator objectAnim = target.foodObject.GetComponent<Animator>();
            await Task.Delay(166);
            objectAnim.SetBool("Absorb", true);

            if (target.isKinine)
            {
                LevelManager.instance.ActivateKininePower();
                ScoreManager.instance.currentCollectedKnowledge++;
                UIManager.instance.UpdateKnowledgeAmount();

                anim.SetBool("isPink", true);
                anim.SetBool("isBase", false);
                anim.SetBool("isBlue", false);

            }

            if (target.isSalt)
            {
                LevelManager.instance.ActivateSaltPower();
                ScoreManager.instance.currentCollectedKnowledge++;
                UIManager.instance.UpdateKnowledgeAmount();
                anim.SetBool("isBlue", true);
                anim.SetBool("isPink", false);
                anim.SetBool("isBase", false);

            }

            LevelManager.instance.AddMovesEat(LevelManager.instance.currentLevel.amountToAddOnEatBlob);
        }
        else
        {
            LevelManager.instance.AddMovesEat(LevelManager.instance.currentLevel.amountToAddOnEatFood);
            ScoreManager.instance.currentCollectedFood++;
            UIManager.instance.UpdateFoodAmount();

            anim.SetBool("isBase", true);
            anim.SetBool("isPink", false);
            anim.SetBool("isBlue", false);

        }
    }
}
