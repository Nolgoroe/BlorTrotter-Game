using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

[Serializable]
public class WaterTiles
{
    public int cost;
    public GameObject[] water;
}

public class LevelEditor : MonoBehaviour, IManageable
{
    public static LevelEditor instance;

    public List<WaterTiles> waterTiles;
    public Sprite levelMap;
    public Sprite levelObstacles;
    public LevelParser[] colorMappingsTiles;
    public LevelParser[] colorMappingsObstacles;

    [SerializeField]
    private float threshold;

    float offsetY = 0.592f;
    //fake isometric view mean we have to have an offset for each new row
    int offsetNewRowX = 0;
    float offsetNewRowY = 0;
    enum checkDirection { bottomLeft, bottomRight, topLeft, topRight }


    GameObject waterTile;

    public void initManager()
    {   
        instance = this;      
    }

    public void CallGenerateLevel()
    {
        GenerateLevel();
    }
    
    void GenerateLevel()
    {
        for (int y = levelMap.texture.height - 1; y >= 0; y--)
        {
            for (int x = 0; x < levelMap.texture.width; x++)
            {
                
                GameObject tileGenerated =  GenerateTile(x, y);
                if(tileGenerated != null)
                {
                    GenerateObstacles(tileGenerated.transform, x, y);
                }              
            }

            //implementing the offset for each row
            offsetNewRowX++;
            offsetNewRowY -= offsetY;
        }
    }

    GameObject GenerateTile(int x, int y)
    {
        Color pixelColor = levelMap.texture.GetPixel(x, y);
        GameObject tile = null;

        // if transparent, ignore 
        if (pixelColor.a == 0)
        {
            return null;
        }
 
        
        foreach (LevelParser colorMapping in colorMappingsTiles)
        {           
            if (Mathf.Abs(pixelColor.r - colorMapping.color.r) <= threshold && Mathf.Abs(pixelColor.g - colorMapping.color.g) <= threshold && Mathf.Abs(pixelColor.b - colorMapping.color.b) <= threshold)
            {
                Vector3 position = new Vector3(x + offsetNewRowX, (x * offsetY) + offsetNewRowY, 60);
                tile = Instantiate(colorMapping.prefab, position, Quaternion.identity);
                
                SpriteRenderer spriteRenderer = tile.GetComponent<SpriteRenderer>();

                //each tile should be behind the tile above, we decrement the layer of each tile and start from a higher value each new line
                spriteRenderer.sortingOrder = levelMap.texture.width + (offsetNewRowX - x);

                GridManager.instance.AddTileToTileList(tile.GetComponent<Tile>());         
            }
                 
            
            // if the color of the pixel is blue, look if the color of the pixel above, behind, to the left and right are blue, and increment neighbour value to 1,2,4,8(2^0,2^1,2^2,2^3)
            // depending of the position 
            if ((Mathf.Abs(pixelColor.r - 0) <= threshold && Mathf.Abs(pixelColor.g - 0) <= threshold && Mathf.Abs(pixelColor.b - 1) <= threshold))
            {

                int neighbourValue = 0;

                if ((Mathf.Abs(levelMap.texture.GetPixel((x + 1), y).r - 0) <= threshold && Mathf.Abs(levelMap.texture.GetPixel((x + 1), y).g - 0) <= threshold && Mathf.Abs(levelMap.texture.GetPixel((x + 1), y).b - 1) <= threshold))
                {
                    neighbourValue += 2; ///right
                }
                if ((Mathf.Abs(levelMap.texture.GetPixel(x, (y + 1)).r - 0) <= threshold && Mathf.Abs(levelMap.texture.GetPixel(x, (y + 1)).g - 0) <= threshold && Mathf.Abs(levelMap.texture.GetPixel(x, (y + 1)).b - 1) <= threshold))
                {
                    neighbourValue += 1; ///top
                }
                if ((Mathf.Abs(levelMap.texture.GetPixel(x, (y - 1)).r - 0) <= threshold && Mathf.Abs(levelMap.texture.GetPixel(x, (y - 1)).g - 0) <= threshold && Mathf.Abs(levelMap.texture.GetPixel(x, (y - 1)).b - 1) <= threshold))
                {
                    neighbourValue += 4;///bottom 
                }
                if ((Mathf.Abs(levelMap.texture.GetPixel((x - 1), y).r - 0) <= threshold && Mathf.Abs(levelMap.texture.GetPixel((x - 1), y).g - 0) <= threshold && Mathf.Abs(levelMap.texture.GetPixel((x - 1), y).b - 1) <= threshold))
                {
                    neighbourValue += 8;///left
                }

                 
                bool isSurrendedByWater_1 = CheckCorner(x, y, checkDirection.topLeft);
                bool isSurrendedByWater_2 = CheckCorner(x, y, checkDirection.bottomLeft);
                bool isSurrendedByWater_3 = CheckCorner(x, y, checkDirection.topRight);
                bool isSurrendedByWater_4 = CheckCorner(x, y, checkDirection.bottomRight);

                if (isSurrendedByWater_1)
                {
                    neighbourValue += 16;
                }
                if (isSurrendedByWater_2)
                {
                    neighbourValue += 128;
                }
                if (isSurrendedByWater_3)
                {
                    neighbourValue += 32;
                }
                if (isSurrendedByWater_4)
                {
                    neighbourValue += 64;
                }


                WaterTiles waterTileToInstantiate = waterTiles.Where(p => p.cost == neighbourValue).SingleOrDefault();
                if (waterTileToInstantiate != null)
                {
                    int i = UnityEngine.Random.Range(0, waterTileToInstantiate.water.Length);
                    waterTile = waterTileToInstantiate.water[i];

                }

          
                Vector3 position = new Vector3(x + offsetNewRowX, (x * offsetY) + offsetNewRowY, 60);
                tile = Instantiate(waterTile, position, Quaternion.identity);
                tile.AddComponent<Tile>().cost = neighbourValue;

                SpriteRenderer spriteRenderer = tile.GetComponent<SpriteRenderer>();

                //each tile should be behind the tile above, we decrement the layer of each tile and start from a higher value each new line

                spriteRenderer.sortingOrder = levelMap.texture.width + (offsetNewRowX - x);

                GridManager.instance.AddTileToTileList(tile.GetComponent<Tile>());

            }
        }

        return tile.gameObject;

    }

    bool CheckCorner(int x, int y, checkDirection checkdirection)
    {
        switch (checkdirection)
        {
            case checkDirection.bottomLeft:
                if ((Mathf.Abs(levelMap.texture.GetPixel((x - 1), y).r - 0) <= threshold && Mathf.Abs(levelMap.texture.GetPixel((x - 1), y).g - 0) <= threshold && Mathf.Abs(levelMap.texture.GetPixel((x - 1), y).b - 1) <= threshold))
                {
                    if ((Mathf.Abs(levelMap.texture.GetPixel(x, (y - 1)).r - 0) <= threshold && Mathf.Abs(levelMap.texture.GetPixel(x, (y - 1)).g - 0) <= threshold && Mathf.Abs(levelMap.texture.GetPixel(x, (y - 1)).b - 1) <= threshold))
                    {
                        if ((Mathf.Abs(levelMap.texture.GetPixel(x - 1, (y - 1)).r - 0) >= threshold || Mathf.Abs(levelMap.texture.GetPixel(x - 1, (y - 1)).g - 0) >= threshold || Mathf.Abs(levelMap.texture.GetPixel(x - 1, (y - 1)).b - 1) >= threshold))
                        {
                            return true;
                        }
                    }
                }
                break;


            case checkDirection.bottomRight:

                if ((Mathf.Abs(levelMap.texture.GetPixel((x + 1), y).r - 0) <= threshold && Mathf.Abs(levelMap.texture.GetPixel((x + 1), y).g - 0) <= threshold && Mathf.Abs(levelMap.texture.GetPixel((x + 1), y).b - 1) <= threshold))
                {
                    if ((Mathf.Abs(levelMap.texture.GetPixel(x, (y - 1)).r - 0) <= threshold && Mathf.Abs(levelMap.texture.GetPixel(x, (y - 1)).g - 0) <= threshold && Mathf.Abs(levelMap.texture.GetPixel(x, (y - 1)).b - 1) <= threshold))
                    {
                        if ((Mathf.Abs(levelMap.texture.GetPixel(x + 1, (y - 1)).r - 0) >= threshold || Mathf.Abs(levelMap.texture.GetPixel(x + 1, (y - 1)).g - 0) >= threshold || Mathf.Abs(levelMap.texture.GetPixel(x + 1, (y - 1)).b - 1) >= threshold))
                        {
                            return true;
                        }
                    }
                }
                break;


            case checkDirection.topLeft:

                if ((Mathf.Abs(levelMap.texture.GetPixel((x - 1), y).r - 0) <= threshold && Mathf.Abs(levelMap.texture.GetPixel((x - 1), y).g - 0) <= threshold && Mathf.Abs(levelMap.texture.GetPixel((x - 1), y).b - 1) <= threshold))
                {
                    if ((Mathf.Abs(levelMap.texture.GetPixel(x, (y + 1)).r - 0) <= threshold && Mathf.Abs(levelMap.texture.GetPixel(x, (y + 1)).g - 0) <= threshold && Mathf.Abs(levelMap.texture.GetPixel(x, (y + 1)).b - 1) <= threshold))
                    {
                        if ((Mathf.Abs(levelMap.texture.GetPixel(x - 1, (y + 1)).r - 0) >= threshold || Mathf.Abs(levelMap.texture.GetPixel(x - 1, (y + 1)).g - 0) >= threshold || Mathf.Abs(levelMap.texture.GetPixel(x - 1, (y + 1)).b - 1) >= threshold))
                        {
                            return true;
                        }
                    }
                }
                break;


            case checkDirection.topRight:

                if ((Mathf.Abs(levelMap.texture.GetPixel((x + 1), y).r - 0) <= threshold && Mathf.Abs(levelMap.texture.GetPixel((x + 1), y).g - 0) <= threshold && Mathf.Abs(levelMap.texture.GetPixel((x + 1), y).b - 1) <= threshold))
                {
                    if ((Mathf.Abs(levelMap.texture.GetPixel(x, (y + 1)).r - 0) <= threshold && Mathf.Abs(levelMap.texture.GetPixel(x, (y + 1)).g - 0) <= threshold && Mathf.Abs(levelMap.texture.GetPixel(x, (y + 1)).b - 1) <= threshold))
                    {
                        if ((Mathf.Abs(levelMap.texture.GetPixel(x + 1, (y + 1)).r - 0) >= threshold || Mathf.Abs(levelMap.texture.GetPixel(x + 1, (y + 1)).g - 0) >= threshold || Mathf.Abs(levelMap.texture.GetPixel(x + 1, (y + 1)).b - 1) >= threshold))
                        {
                            return true;
                        }
                    }
                }
                break;


            default:
                break;

        }

        return false;
    }


    void GenerateObstacles(Transform parent, int x , int y )
    {
        Color pixelColor = levelObstacles.texture.GetPixel(x, y);
        GameObject parentObject = parent.gameObject;

        // if transparent, ignore 
        if (pixelColor.a == 0)
        {
            return;
        }


        // Debug.Log("pixelcolor" + pixelColor);

        foreach (LevelParser colorMapping in colorMappingsObstacles)
        {
            //  Debug.Log("colormapping" + colorMapping.color);


            if (Mathf.Abs(pixelColor.r - colorMapping.color.r) <= threshold && Mathf.Abs(pixelColor.g - colorMapping.color.g) <= threshold && Mathf.Abs(pixelColor.b - colorMapping.color.b) <= threshold)
            {
                Vector3 position = new Vector3(parent.position.x, parent.position.y + (offsetY * 2) , parent.position.z);
               

                GameObject obstacle = Instantiate(colorMapping.prefab, position, Quaternion.identity, parent );
                obstacle.GetComponent<SpriteRenderer>().sortingOrder = parentObject.GetComponent<SpriteRenderer>().sortingOrder;
              
            }
        }
    }   
}
