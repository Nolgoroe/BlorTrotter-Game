using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public enum EdgeType {notEdge, leftEdge, rightEdge, bottomEdge, topEdge, topRightEdge, topLeftEdge, bottomRightEdge, bottomLeftEdge}
public class Tile : MonoBehaviour
{
    public GameObject canBeSelectedSprite;
    public GameObject selectedSprite;
    public GameObject gooSprite;
    public GameObject enemyGooSprite;
    public GameObject foodObject;
    public GameObject enemySpawnParent;
    public GameObject connectedLockDisplay;

    public int cost;
    public int index;

    public bool isFull;
    public bool isLocked;
    //public bool isLocked;
    public bool isWaterTile; 
    public bool isGooPiece; 
    public bool isEnemyGooPiece; 
    public bool isBeetle; 
    public bool isBeetleForTutorial; 
    public bool isAdjacentToMainBody;  //new
    public bool isMainPlayerBody;  //new
    public bool isSlugBody;  //new
    public bool isFood;  //new
    public bool isKinine;  //new
    public bool isSalt;  //new
    public bool isLightTile;

    public int gCost, hCost;
    public int tileX, tileY;
    public Tile parentTileForPath;
    public Tile playerTeleportTile;
    public Sprite GreyVer, colorVer;

    public int turnsUntilEnemyGooDissappears;


    public int fCost // no need for set
    {
        get
        {
           return gCost + hCost;
        }
    }

    public void SetXY(int _x, int _y)
    {
        tileX = _x;
        tileY = _y;
    }

    public EdgeType edgeType = EdgeType.notEdge;


    public void SetEnemySpawnDataSlug()
    {
        isFull = true;
        isGooPiece = false;
        isEnemyGooPiece = true;
        isSlugBody = true;
        isAdjacentToMainBody = false;
        isMainPlayerBody = false;
        turnsUntilEnemyGooDissappears = 3;
    }
    public void SetEnemySpawnDataBeetle()
    {
        //isFull = true;
        //isGooPiece = false;
        //isBeetleForTutorial = true;

        //isFood = false;

        //isEnemyGooPiece = true;
        //isAdjacentToMainBody = false;
        //isMainPlayerBody = false;
    }


    public void GoToGreyScale()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.sprite = GreyVer;
    }
    public void GoToColorScale()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.sprite = colorVer;
    }


    public async Task RotateEatenBlobDisplay(Tile from, Tile TileTo)
    {
        if (TileTo.tileY > from.tileY)
        {
            foodObject.GetComponent<Animator>().SetBool("Flip", true);

            foodObject.GetComponent<EntityAnimDataSetter>().isFlippedEaten = true;
            Debug.Log("Down HERE NOW");
        }
        else if (TileTo.tileX < from.tileX)
        {
            foodObject.GetComponent<Animator>().SetBool("Flip", true);

            foodObject.GetComponent<EntityAnimDataSetter>().isFlippedEaten = true;

            Debug.Log("right HERE NOW");

        }
    }

}
