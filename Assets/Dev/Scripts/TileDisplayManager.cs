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

    public void SetColorAdjacentTile()
    {

    }
}
