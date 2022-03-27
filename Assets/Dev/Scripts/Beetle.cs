using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;

public class Beetle : Entity
{
    public Tile PublicTargetTile;

    public int currentAnimState = 0;

    public bool hasPickedFood;
    public GameObject foodObject;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public override async Task ManageTurnStart()
    {
        ChangeAnimState();

        await Task.Delay(1000);
    }

    private void ChangeAnimState()
    {
        if(currentAnimState > -1)
        {
            anim.SetBool(anim.parameters[currentAnimState].name, false);
        }

        currentAnimState++;

        if(currentAnimState == 1)
        {
            currentTile.isFull = false;
            currentTile.isBeetle = false;
        }

        if (currentAnimState == anim.parameterCount) // 3 = num of animation booleans
        {
            anim.SetBool(anim.parameters[anim.parameterCount - 1].name, false);

            currentAnimState = 0;
        }

        anim.SetBool(anim.parameters[currentAnimState].name, true);



    }

    public override void SetCurrentTile(Tile tileOn)
    {
        currentTile = tileOn;
    }

    public void SetTargetTileBeetle()
    {
        int rand = UnityEngine.Random.Range(0, EntityManager.instance.beetleTargetAndSpawnTiles.Count);

        PublicTargetTile = EntityManager.instance.beetleTargetAndSpawnTiles[rand];

        int counter = 1000;

        while (currentTile == PublicTargetTile || PublicTargetTile.isFood)
        {
            Debug.Log("SAME");
            counter--;

            rand = UnityEngine.Random.Range(0, EntityManager.instance.beetleTargetAndSpawnTiles.Count);

            PublicTargetTile = EntityManager.instance.beetleTargetAndSpawnTiles[rand];


            if (counter <= 0)
            {
                Debug.LogError("PROBLEM WITH Beetle");
                break;
            }
        }

        currentTile.isFull = true;
        currentTile.isBeetle = true;
    }

    public void CheckEatFood()
    {
        if (currentTile.isFood)
        {
            EatFood(currentTile);
        }
        else
        {
            hasPickedFood = false;
        }

        if (currentTile.isGooPiece)
        {
            EatGooPiece(currentTile);
        }
    }

    public void EatGooPiece(Tile target)
    {
        target.isGooPiece = false;
        GridManager.instance.RemoveGooTileDisplay(target);
        EntityManager.instance.GetPlayer().RemoveGooTiles(target);

        if (target.isMainPlayerBody)
        {
            target.isMainPlayerBody = false;

            EntityManager.instance.SpawnPlayerRandomGooLocation();
        }
    }

    void EatFood(Tile current)
    {
        current.isFood = false;

        current.foodObject.SetActive(false); 

        foodObject = current.foodObject;

        current.foodObject = null;

        hasPickedFood = true;

        if (current.GetComponent<TutorialObject>())
        {
            Destroy(current.GetComponent<TutorialObject>());
        }
    }

    public void CheckPlaceFood()
    {
        if (!currentTile.isFood && foodObject && !hasPickedFood)
        {
            MoveFoodObject(foodObject);
        }
    }

    private void MoveFoodObject(GameObject toMove)
    {
        toMove.transform.SetParent(currentTile.transform);
        Vector3 position = new Vector3(0, 1.184f, 0);
        toMove.transform.localPosition = position;
        toMove.SetActive(true);

        currentTile.isFood = true;
        currentTile.foodObject = toMove;

        currentTile.gameObject.AddComponent<TutorialObject>().SetDescription(TypeOfTutorial.Food);
        toMove.GetComponent<SpriteRenderer>().sortingOrder = toMove.transform.parent.GetComponent<SpriteRenderer>().sortingOrder + 1;

        foodObject = null;
    }

    public void ReleaseTile()
    {
        currentTile.isFull = false;
    }

    public void MoveBeetle()
    {
        Transform parent = PublicTargetTile.transform;

        transform.SetParent(parent);
        Vector3 position = new Vector3(parent.position.x, parent.position.y + (LevelEditor.instance.offsetY * 2), parent.position.z);

        transform.position = position;

        LevelEditor.instance.SetParentByTag(gameObject);

        transform.GetComponent<SpriteRenderer>().sortingOrder = parent.GetComponent<SpriteRenderer>().sortingOrder + 2;


        if (PublicTargetTile)
        {
            SetCurrentTile(PublicTargetTile);
        }
    }
}
