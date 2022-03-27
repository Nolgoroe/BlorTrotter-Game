using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EdgeType {notEdge, leftEdge, rightEdge, bottomEdge, topEdge, topRightEdge, topLeftEdge, bottomRightEdge, bottomLeftEdge}
public class Tile : MonoBehaviour
{
    public GameObject canBeSelectedSprite;
    public GameObject selectedSprite;
    public GameObject gooSprite;
    public GameObject enemyGooSprite;
    public GameObject foodObject;
    public GameObject enemySpawnParent;

    public int cost;
    public int index;

    public bool isFull;
    //public bool isLocked;
    public bool isWaterTile; 
    public bool isGooPiece; 
    public bool isEnemyGooPiece; 
    public bool isBeetle; 
    public bool isAdjacentToMainBody;  //new
    public bool isMainPlayerBody;  //new
    public bool isFood;  //new
    public bool isKinine;  //new
    public bool isSalt;  //new
    public bool isLightTile;

    public int gCost, hCost;
    public int tileX, tileY;
    public Tile parentTileForPath;
    public Tile playerTeleportTile;


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
        isAdjacentToMainBody = false;
        isMainPlayerBody = false;
        turnsUntilEnemyGooDissappears = 3;
    }
    public void SetEnemySpawnDataBeetle()
    {
        isFull = true;
        isGooPiece = false;
        //isFood = false;

        //isEnemyGooPiece = true;
        //isAdjacentToMainBody = false;
        //isMainPlayerBody = false;
    }
}
