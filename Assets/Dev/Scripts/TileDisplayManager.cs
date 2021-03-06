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
    public void SetTileDisplayEnemySpawnON(Tile tile)
    {
        tile.enemySpawnParent.SetActive(true);
        tile.enemySpawnParent.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = tile.GetComponent<SpriteRenderer>().sortingOrder + 1;
        tile.enemySpawnParent.transform.GetChild(1).GetComponent<SpriteRenderer>().sortingOrder = tile.GetComponent<SpriteRenderer>().sortingOrder + 2;
    }
    public void SetTileDisplayEnemySpawnOFF(Tile tile)
    {
        tile.enemySpawnParent.SetActive(false);
    }

    public void SetTileDisplayBeetleTargetON(Tile tile)
    {
        tile.targetForBeetleDisplay.SetActive(true);

        tile.targetForBeetleDisplay.GetComponent<SpriteRenderer>().sortingOrder = tile.GetComponent<SpriteRenderer>().sortingOrder + 1;
    }
    public void SetTileDisplayBeetleTargetOFF(Tile tile)
    {
        tile.targetForBeetleDisplay.SetActive(false);
    }
}
