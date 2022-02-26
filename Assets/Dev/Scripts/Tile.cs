using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EdgeType {notEdge, leftEdge, rightEdge, bottomEdge, topEdge, topRightEdge, topLeftEdge, bottomRightEdge, bottomLeftEdge}
public class Tile : MonoBehaviour
{
    public GameObject selectedSprite;

    public int cost;
    public int index;

    public bool isFull;
    public EdgeType edgeType = EdgeType.notEdge;
}
