using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EdgeType {notEdge, leftEdge, rightEdge, bottomEdge, topEdge, topRightEdge, topLeftEdge, bottomRightEdge, bottomLeftEdge}
public class Tile : MonoBehaviour
{
    public GameObject canBeSelectedSprite;
    public GameObject selectedSprite;
    public GameObject gooSprite;

    public int cost;
    public int index;

    public bool isFull;
    public bool isWaterTile; 
    public bool isGooPiece; 


    public int gCost, hCost;
    public int tileX, tileY;
    public Tile parentTileForPath;

    public int fCost // np need for set
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
}
