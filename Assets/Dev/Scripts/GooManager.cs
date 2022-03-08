using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class GooTileOptions
{
    public int cost;
    public Sprite[] gooSprite;
}

public class GooManager : MonoBehaviour, IManageable
{
    public static GooManager instance;

    public List<Tile> playerGooList;
    public List<Tile> enemiesGooList;
    public List<GooTileOptions> gooTileOptions;

    public void initManager()
    {
        instance = this;     
    }

    public void Start()
    {
        playerGooList = new List<Tile>();
        enemiesGooList = new List<Tile>();
        //gooTileOptions = new List<GooTileOptions>();
    }
}
