using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileDisplayManager : MonoBehaviour
{
    public Sprite tileSelectedSprite;

    public void SetTileSelectedDisplay(Tile toSetSelect) // NEW
    {
        toSetSelect.selectedSprite.SetActive(true);
    }

    public void SetTileNotSelectedDisplay(Tile toSetSelect) // NEW
    {
        toSetSelect.selectedSprite.SetActive(false);
    }

    public void SetTileCanBeSelectedDisplayON(Tile tile) // NEW
    {
        tile.canBeSelectedSprite.SetActive(true);
    }
    public void SetTileCanBeSelectedDisplayOFF(Tile tile) // NEW 
    {
        tile.canBeSelectedSprite.SetActive(false);
    }
}
