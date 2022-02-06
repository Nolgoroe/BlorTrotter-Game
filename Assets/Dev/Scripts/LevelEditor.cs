using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

[Serializable]
public class blehh
{
    public int cost;
    public GameObject path;
}

public class LevelEditor : MonoBehaviour
{   
    public List<blehh> blahBleh;
    public Texture2D map;
    public LevelParser[] colorMappings;

    [SerializeField]
    private float threshold;


    float offsetY = 0.592f;
    //fake isometric view mean we have to have an offset for each new row
    int offsetNewRowX = 0;
    float offsetNewRowY = 0;
    enum checkDirection { bottomLeft, bottomRight, topLeft, topRight}
    


    GameObject waterTile;


    void Start()
    {
        GenerateLevel();
    }

    void GenerateLevel()
    {
        for (int y = map.height - 1; y >= 0; y--)
        {
            for (int x = 0; x < map.width; x++)
            {

                GenerateTile(x, y);
            }

            //implementing the offset for each row
            offsetNewRowX++;
            offsetNewRowY -= offsetY;
        }
        Debug.Log(map.width);
        Debug.Log(map.height);
    }
    void GenerateTile(int x, int y)
    {
        Color pixelColor = map.GetPixel(x, y);


        // if transparent, ignore 
        if (pixelColor.a == 0)
        {
            return;
        }


        Debug.Log("pixelcolor" + pixelColor);

        foreach (LevelParser colorMapping in colorMappings)
        {
            Debug.Log("colormapping" + colorMapping.color);


            if (Mathf.Abs(pixelColor.r - colorMapping.color.r) <= threshold && Mathf.Abs(pixelColor.g - colorMapping.color.g) <= threshold && Mathf.Abs(pixelColor.b - colorMapping.color.b) <= threshold)
            {
                Vector3 position = new Vector3(x + offsetNewRowX, (x * offsetY) + offsetNewRowY, 60);
                GameObject tile = Instantiate(colorMapping.prefab, position, Quaternion.identity);
                SpriteRenderer spriteRenderer = tile.GetComponent<SpriteRenderer>();

                //each tile should be behind the tile above, we decrement the layer of each tile and start from a higher value each new line
                spriteRenderer.sortingOrder = map.width + (offsetNewRowX - x);
            }







            // if the color of the pixel is blue, look if the color of the pixel above, behind, to the left and right are blue, and increment neighbour value to 1,2,4,8(2^0,2^1,2^2,2^3)
            // depending of the position 
            if ((Mathf.Abs(pixelColor.r - 0) <= threshold && Mathf.Abs(pixelColor.g - 0) <= threshold && Mathf.Abs(pixelColor.b - 1) <= threshold))
            {

                int neighbourValue = 0;
                int randomBtw0and3 = UnityEngine.Random.Range(1, 4);
                int randomBtw0and2 = UnityEngine.Random.Range(1, 3);






                if ((Mathf.Abs(map.GetPixel((x + 1), y).r - 0) <= threshold && Mathf.Abs(map.GetPixel((x + 1), y).g - 0) <= threshold && Mathf.Abs(map.GetPixel((x + 1), y).b - 1) <= threshold))
                {
                    neighbourValue += 2; ///right
                }
                if ((Mathf.Abs(map.GetPixel(x, (y + 1)).r - 0) <= threshold && Mathf.Abs(map.GetPixel(x, (y + 1)).g - 0) <= threshold && Mathf.Abs(map.GetPixel(x, (y + 1)).b - 1) <= threshold))
                {
                    neighbourValue += 1; ///top
                }
                if ((Mathf.Abs(map.GetPixel(x, (y - 1)).r - 0) <= threshold && Mathf.Abs(map.GetPixel(x, (y - 1)).g - 0) <= threshold && Mathf.Abs(map.GetPixel(x, (y - 1)).b - 1) <= threshold))
                {
                    neighbourValue += 4;///bottom 
                }
                if ((Mathf.Abs(map.GetPixel((x - 1), y).r - 0) <= threshold && Mathf.Abs(map.GetPixel((x - 1), y).g - 0) <= threshold && Mathf.Abs(map.GetPixel((x - 1), y).b - 1) <= threshold))
                {
                    neighbourValue += 8;///left
                }

                //if((Mathf.Abs((map.GetPixel((x -1 ), (y +1)))))  ( top left) is surronded by land 

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


                

                blehh skuuu = blahBleh.Where(p => p.cost == neighbourValue).SingleOrDefault();
                if(skuuu != null)
                {
                    waterTile = skuuu.path;
                }


                    //switch (neighbourValue)
                    //{


                    //    case 0:
                    //        waterTile = GameObject.Find("Water/TER_D_Water_SS");
                    //        break;
                    //    case 1:
                    //        waterTile = GameObject.Find("Water/TER_D_Water_EBL");
                    //        break;
                    //    case 2:
                    //        waterTile = GameObject.Find("Water/TER_D_Water_EBR");
                    //        break;
                    //    case 3:
                    //        waterTile = GameObject.Find("Water/TER_D_Water_CB");
                    //        break;
                    //    case 4:
                    //        waterTile = GameObject.Find("Water/TER_D_Water_ETL");
                    //        break;
                    //    case 5:
                    //        waterTile = GameObject.Find("Water/TER_D_Water_LSL_0" + randomBtw0and2.ToString());
                    //        break;
                    //    case 6:
                    //        waterTile = GameObject.Find("Water/TER_D_Water_CL");
                    //        break;
                    //    case 7:
                    //        waterTile = GameObject.Find("Water/TER_D_Water_LBL_0" + randomBtw0and2.ToString());
                    //        break;
                    //    case 8:
                    //        waterTile = GameObject.Find("Water/TER_D_Water_ETR");
                    //        break;
                    //    case 9:
                    //        waterTile = GameObject.Find("Water/TER_D_Water_CSR");
                    //        break;
                    //    case 10:
                    //        waterTile = GameObject.Find("Water/TER_D_Water_LSR_0" + randomBtw0and2.ToString());
                    //        break;
                    //    case 11:
                    //        waterTile = GameObject.Find("Water/TER_D_Water_LBR_0" + randomBtw0and2.ToString());
                    //        break;
                    //    case 12:
                    //        waterTile = GameObject.Find("Water/TER_D_Water_CT");
                    //        break;
                    //    case 13:
                    //        waterTile = GameObject.Find("Water/TER_D_Water_LTL_0" + randomBtw0and2.ToString());
                    //        break;
                    //    case 14:
                    //        waterTile = GameObject.Find("Water/TER_D_Water_LTR_0" + randomBtw0and2.ToString());
                    //        break;
                    //    case 15:
                    //        waterTile = GameObject.Find("Water/TER_D_Water_CF_0" + randomBtw0and3.ToString());
                    //        break;
                    //    case 35:
                    //        waterTile = GameObject.Find("Water/TER_D_Water_CSB");
                    //        break;

                    //}
                    Debug.Log("AND now prefab : " + colorMapping.prefab);
                Vector3 position = new Vector3(x + offsetNewRowX, (x * offsetY) + offsetNewRowY, 60);
                GameObject tile = Instantiate(waterTile, position, Quaternion.identity);
                tile.AddComponent<Tile>().cost = neighbourValue;
                
                SpriteRenderer spriteRenderer = tile.GetComponent<SpriteRenderer>();

                //each tile should be behind the tile above, we decrement the layer of each tile and start from a higher value each new line
                spriteRenderer.sortingOrder = map.width + (offsetNewRowX - x);


            }
        }


    }

    bool CheckCorner(int x , int y, checkDirection checkdirection )
    {
        switch (checkdirection)
        {
            case checkDirection.bottomLeft: 
                if ((Mathf.Abs(map.GetPixel((x - 1), y).r - 0) <= threshold && Mathf.Abs(map.GetPixel((x - 1), y).g - 0) <= threshold && Mathf.Abs(map.GetPixel((x - 1), y).b - 1) <= threshold))
                {
                    if ((Mathf.Abs(map.GetPixel(x, (y - 1)).r - 0) <= threshold && Mathf.Abs(map.GetPixel(x, (y - 1)).g - 0) <= threshold && Mathf.Abs(map.GetPixel(x, (y - 1)).b - 1) <= threshold))
                    {
                        if ((Mathf.Abs(map.GetPixel(x - 1, (y - 1)).r - 0) >= threshold || Mathf.Abs(map.GetPixel(x - 1, (y - 1)).g - 0) >= threshold || Mathf.Abs(map.GetPixel(x - 1, (y - 1)).b - 1) >= threshold))
                        {
                            return true;
                        }
                    }
                }


                break;
            case checkDirection.bottomRight:  

                if ((Mathf.Abs(map.GetPixel((x + 1), y).r - 0) <= threshold && Mathf.Abs(map.GetPixel((x + 1), y).g - 0) <= threshold && Mathf.Abs(map.GetPixel((x + 1), y).b - 1) <= threshold))
                {
                    if ((Mathf.Abs(map.GetPixel(x, (y - 1)).r - 0) <= threshold && Mathf.Abs(map.GetPixel(x, (y - 1)).g - 0) <= threshold && Mathf.Abs(map.GetPixel(x, (y - 1)).b - 1) <= threshold))
                    {
                        if ((Mathf.Abs(map.GetPixel(x + 1, (y - 1)).r - 0) >= threshold || Mathf.Abs(map.GetPixel(x + 1, (y - 1)).g - 0) >= threshold || Mathf.Abs(map.GetPixel(x + 1, (y - 1)).b - 1) >= threshold))
                        {
                            return true;
                        }
                    }
                }
                break;
            case checkDirection.topLeft: 

                if ((Mathf.Abs(map.GetPixel((x - 1), y).r - 0) <= threshold && Mathf.Abs(map.GetPixel((x - 1), y).g - 0) <= threshold && Mathf.Abs(map.GetPixel((x - 1), y).b - 1) <= threshold))
                {
                    if ((Mathf.Abs(map.GetPixel(x, (y + 1)).r - 0) <= threshold && Mathf.Abs(map.GetPixel(x, (y + 1)).g - 0) <= threshold && Mathf.Abs(map.GetPixel(x, (y + 1)).b - 1) <= threshold))
                    {
                        if ((Mathf.Abs(map.GetPixel(x - 1, (y + 1)).r - 0) >= threshold || Mathf.Abs(map.GetPixel(x - 1, (y + 1)).g - 0) >= threshold || Mathf.Abs(map.GetPixel(x - 1, (y + 1)).b - 1) >= threshold))
                        {
                            return true;
                        }
                    }
                }


                break;
            case checkDirection.topRight: 

                if ((Mathf.Abs(map.GetPixel((x + 1), y).r - 0) <= threshold && Mathf.Abs(map.GetPixel((x + 1), y).g - 0) <= threshold && Mathf.Abs(map.GetPixel((x + 1), y).b - 1) <= threshold))
                {
                    if ((Mathf.Abs(map.GetPixel(x, (y + 1)).r - 0) <= threshold && Mathf.Abs(map.GetPixel(x, (y + 1)).g - 0) <= threshold && Mathf.Abs(map.GetPixel(x, (y + 1)).b - 1) <= threshold))
                    {
                        if ((Mathf.Abs(map.GetPixel(x + 1, (y + 1)).r - 0) >= threshold || Mathf.Abs(map.GetPixel(x + 1, (y + 1)).g - 0) >= threshold || Mathf.Abs(map.GetPixel(x + 1, (y + 1)).b - 1) >= threshold))
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
}
