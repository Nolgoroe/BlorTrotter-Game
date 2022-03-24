using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileDisplayManager : MonoBehaviour
{
    public Sprite tileSelectedSprite;

    public void SetTileSelectedDisplay(Tile toSetSelect) 
    {
        toSetSelect.selectedSprite.SetActive(true);
    }

    public void SetTileNotSelectedDisplay(Tile toSetSelect) 
    {
        toSetSelect.selectedSprite.SetActive(false);
    }

    public void SetTileCanBeSelectedDisplayON(Tile tile) 
    {
        tile.canBeSelectedSprite.SetActive(true);
    }
    public void SetTileCanBeSelectedDisplayOFF(Tile tile) 
    {
        tile.canBeSelectedSprite.SetActive(false);
    }
    public void SetTileDisplayGooON(Tile tile)
    {
        tile.gooSprite.SetActive(true);
    }
    public void SetTileDisplayGooOFF(Tile tile)
    {
        tile.gooSprite.SetActive(false);
    }
    public void SetTileDisplayEnemyGooON(Tile tile)
    {
        tile.enemyGooSprite.SetActive(true);
    }
    public void SetTileDisplayEnemyGooOFF(Tile tile)
    {
        tile.enemyGooSprite.SetActive(false);
    }
}
