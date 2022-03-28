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
    public LevelParser[] colorMappingsTiles;
    public LevelParser[] colorMappingsObstacles;
    public LevelParser[] colorMappingsBeetleData;

    [SerializeField]
    private float threshold;

    [HideInInspector]
    public float offsetY = 0.592f;
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
        offsetNewRowX = 0;
        offsetNewRowY = 0;

        GenerateLevel();
    }
    
    void GenerateLevel()
    {
        for (int y = LevelManager.instance.currentLevel.levelMap.texture.height - 1; y >= 0; y--)
        {
            GameObject tileGenerated = null;

            for (int x = 0; x < LevelManager.instance.currentLevel.levelMap.texture.width; x++)
            {

                tileGenerated = GenerateTile(x, y);

                if (tileGenerated != null)
                {
                    GenerateObstacles(tileGenerated.transform, x, y);

                    if (LevelManager.instance.currentLevel.levelBeetleData)
                    {
                        GenerateBeetleData(tileGenerated.transform, x, y);
                    }
                }

                if (x==0)
                {                  
                    tileGenerated.GetComponent<Tile>().edgeType = EdgeType.leftEdge;
                }
                else if(y== LevelManager.instance.currentLevel.levelMap.texture.height - 1)
                {
                    tileGenerated.GetComponent<Tile>().edgeType = EdgeType.topEdge;
                }
                else if (y == 0)
                {
                    tileGenerated.GetComponent<Tile>().edgeType = EdgeType.bottomEdge;
                }
            }

            tileGenerated.GetComponent<Tile>().edgeType = EdgeType.rightEdge;

            
            //implementing the offset for each row
            offsetNewRowX++;
            offsetNewRowY -= offsetY;
        }     
    }

    GameObject GenerateTile(int x, int y)
    {
        Color pixelColor = LevelManager.instance.currentLevel.levelMap.texture.GetPixel(x, y);
        GameObject tile = null;

        string s = ColorUtility.ToHtmlStringRGB(pixelColor);

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

                if(colorMapping.prefab.Length > 1)
                {
                    int index = UnityEngine.Random.Range(0, colorMapping.prefab.Length);
                    tile = Instantiate(colorMapping.prefab[index], position, Quaternion.identity, ObjectRefrencer.instance.levelMap.transform);
                }
                else
                {
                    tile = Instantiate(colorMapping.prefab[0], position, Quaternion.identity, ObjectRefrencer.instance.levelMap.transform);
                }

                SpriteRenderer spriteRenderer = tile.GetComponent<SpriteRenderer>();
                tile.GetComponent<Tile>().SetXY(x, y);

                //each tile should be behind the tile above, we decrement the layer of each tile and start from a higher value each new line
                spriteRenderer.sortingOrder = LevelManager.instance.currentLevel.levelMap.texture.width + (offsetNewRowX - x);

                GridManager.instance.AddTileToTileList(tile.GetComponent<Tile>());

                break;
            }
                 
            
            // if the color of the pixel is blue, look if the color of the pixel above, behind, to the left and right are blue, and increment neighbour value to 1,2,4,8(2^0,2^1,2^2,2^3)
            // depending of the position 
            if ((Mathf.Abs(pixelColor.r - 0) <= threshold && Mathf.Abs(pixelColor.g - 0) <= threshold && Mathf.Abs(pixelColor.b - 1) <= threshold))
            {

                int neighbourValue = 0;

                if (((Mathf.Abs(LevelManager.instance.currentLevel.levelMap.texture.GetPixel((x + 1), y).r - 0) <= threshold && Mathf.Abs(LevelManager.instance.currentLevel.levelMap.texture.GetPixel((x + 1), y).g - 0) <= threshold && Mathf.Abs(LevelManager.instance.currentLevel.levelMap.texture.GetPixel((x + 1), y).b - 1) <= threshold)) || LevelManager.instance.currentLevel.levelMap.texture.GetPixel((x+1), y) == null)
                {
                    neighbourValue += 2; ///right
                }
                if (((Mathf.Abs(LevelManager.instance.currentLevel.levelMap.texture.GetPixel(x, (y + 1)).r - 0) <= threshold && Mathf.Abs(LevelManager.instance.currentLevel.levelMap.texture.GetPixel(x, (y + 1)).g - 0) <= threshold && Mathf.Abs(LevelManager.instance.currentLevel.levelMap.texture.GetPixel(x, (y + 1)).b - 1) <= threshold)) || LevelManager.instance.currentLevel.levelMap.texture.GetPixel((x ), y + 1) == null)
                {
                    neighbourValue += 1; ///top
                }
                if (((Mathf.Abs(LevelManager.instance.currentLevel.levelMap.texture.GetPixel(x, (y - 1)).r - 0) <= threshold && Mathf.Abs(LevelManager.instance.currentLevel.levelMap.texture.GetPixel(x, (y - 1)).g - 0) <= threshold && Mathf.Abs(LevelManager.instance.currentLevel.levelMap.texture.GetPixel(x, (y - 1)).b - 1) <= threshold)) || LevelManager.instance.currentLevel.levelMap.texture.GetPixel((x ), y - 1) == null)
                {
                    neighbourValue += 4;///bottom 
                }
                if (((Mathf.Abs(LevelManager.instance.currentLevel.levelMap.texture.GetPixel((x - 1), y).r - 0) <= threshold && Mathf.Abs(LevelManager.instance.currentLevel.levelMap.texture.GetPixel((x - 1), y).g - 0) <= threshold && Mathf.Abs(LevelManager.instance.currentLevel.levelMap.texture.GetPixel((x - 1), y).b - 1) <= threshold)) || LevelManager.instance.currentLevel.levelMap.texture.GetPixel((x - 1), y) == null)
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
                tile = Instantiate(waterTile, position, Quaternion.identity, ObjectRefrencer.instance.levelMap.transform);
                tile.GetComponent<Tile>().cost = neighbourValue;
                tile.GetComponent<Tile>().isFull = true;
                

                SpriteRenderer spriteRenderer = tile.GetComponent<SpriteRenderer>();

                //each tile should be behind the tile above, we decrement the layer of each tile and start from a higher value each new line

                spriteRenderer.sortingOrder = LevelManager.instance.currentLevel.levelMap.texture.width + (offsetNewRowX - x);

                GridManager.instance.AddTileToTileList(tile.GetComponent<Tile>());
                break;

            }
        }

        return tile.gameObject;

    }

    bool CheckCorner(int x, int y, checkDirection checkdirection)
    {
        switch (checkdirection)
        {
            case checkDirection.bottomLeft:
                if ((Mathf.Abs(LevelManager.instance.currentLevel.levelMap.texture.GetPixel((x - 1), y).r - 0) <= threshold && Mathf.Abs(LevelManager.instance.currentLevel.levelMap.texture.GetPixel((x - 1), y).g - 0) <= threshold && Mathf.Abs(LevelManager.instance.currentLevel.levelMap.texture.GetPixel((x - 1), y).b - 1) <= threshold))
                {
                    if ((Mathf.Abs(LevelManager.instance.currentLevel.levelMap.texture.GetPixel(x, (y - 1)).r - 0) <= threshold && Mathf.Abs(LevelManager.instance.currentLevel.levelMap.texture.GetPixel(x, (y - 1)).g - 0) <= threshold && Mathf.Abs(LevelManager.instance.currentLevel.levelMap.texture.GetPixel(x, (y - 1)).b - 1) <= threshold))
                    {
                        if ((Mathf.Abs(LevelManager.instance.currentLevel.levelMap.texture.GetPixel(x - 1, (y - 1)).r - 0) >= threshold || Mathf.Abs(LevelManager.instance.currentLevel.levelMap.texture.GetPixel(x - 1, (y - 1)).g - 0) >= threshold || Mathf.Abs(LevelManager.instance.currentLevel.levelMap.texture.GetPixel(x - 1, (y - 1)).b - 1) >= threshold))
                        {
                            return true;
                        }
                    }
                }
                break;


            case checkDirection.bottomRight:

                if ((Mathf.Abs(LevelManager.instance.currentLevel.levelMap.texture.GetPixel((x + 1), y).r - 0) <= threshold && Mathf.Abs(LevelManager.instance.currentLevel.levelMap.texture.GetPixel((x + 1), y).g - 0) <= threshold && Mathf.Abs(LevelManager.instance.currentLevel.levelMap.texture.GetPixel((x + 1), y).b - 1) <= threshold))
                {
                    if ((Mathf.Abs(LevelManager.instance.currentLevel.levelMap.texture.GetPixel(x, (y - 1)).r - 0) <= threshold && Mathf.Abs(LevelManager.instance.currentLevel.levelMap.texture.GetPixel(x, (y - 1)).g - 0) <= threshold && Mathf.Abs(LevelManager.instance.currentLevel.levelMap.texture.GetPixel(x, (y - 1)).b - 1) <= threshold))
                    {
                        if ((Mathf.Abs(LevelManager.instance.currentLevel.levelMap.texture.GetPixel(x + 1, (y - 1)).r - 0) >= threshold || Mathf.Abs(LevelManager.instance.currentLevel.levelMap.texture.GetPixel(x + 1, (y - 1)).g - 0) >= threshold || Mathf.Abs(LevelManager.instance.currentLevel.levelMap.texture.GetPixel(x + 1, (y - 1)).b - 1) >= threshold))
                        {
                            return true;
                        }
                    }
                }
                break;


            case checkDirection.topLeft:

                if ((Mathf.Abs(LevelManager.instance.currentLevel.levelMap.texture.GetPixel((x - 1), y).r - 0) <= threshold && Mathf.Abs(LevelManager.instance.currentLevel.levelMap.texture.GetPixel((x - 1), y).g - 0) <= threshold && Mathf.Abs(LevelManager.instance.currentLevel.levelMap.texture.GetPixel((x - 1), y).b - 1) <= threshold))
                {
                    if ((Mathf.Abs(LevelManager.instance.currentLevel.levelMap.texture.GetPixel(x, (y + 1)).r - 0) <= threshold && Mathf.Abs(LevelManager.instance.currentLevel.levelMap.texture.GetPixel(x, (y + 1)).g - 0) <= threshold && Mathf.Abs(LevelManager.instance.currentLevel.levelMap.texture.GetPixel(x, (y + 1)).b - 1) <= threshold))
                    {
                        if ((Mathf.Abs(LevelManager.instance.currentLevel.levelMap.texture.GetPixel(x - 1, (y + 1)).r - 0) >= threshold || Mathf.Abs(LevelManager.instance.currentLevel.levelMap.texture.GetPixel(x - 1, (y + 1)).g - 0) >= threshold || Mathf.Abs(LevelManager.instance.currentLevel.levelMap.texture.GetPixel(x - 1, (y + 1)).b - 1) >= threshold))
                        {
                            return true;
                        }
                    }
                }
                break;


            case checkDirection.topRight:

                if ((Mathf.Abs(LevelManager.instance.currentLevel.levelMap.texture.GetPixel((x + 1), y).r - 0) <= threshold && Mathf.Abs(LevelManager.instance.currentLevel.levelMap.texture.GetPixel((x + 1), y).g - 0) <= threshold && Mathf.Abs(LevelManager.instance.currentLevel.levelMap.texture.GetPixel((x + 1), y).b - 1) <= threshold))
                {
                    if ((Mathf.Abs(LevelManager.instance.currentLevel.levelMap.texture.GetPixel(x, (y + 1)).r - 0) <= threshold && Mathf.Abs(LevelManager.instance.currentLevel.levelMap.texture.GetPixel(x, (y + 1)).g - 0) <= threshold && Mathf.Abs(LevelManager.instance.currentLevel.levelMap.texture.GetPixel(x, (y + 1)).b - 1) <= threshold))
                    {
                        if ((Mathf.Abs(LevelManager.instance.currentLevel.levelMap.texture.GetPixel(x + 1, (y + 1)).r - 0) >= threshold || Mathf.Abs(LevelManager.instance.currentLevel.levelMap.texture.GetPixel(x + 1, (y + 1)).g - 0) >= threshold || Mathf.Abs(LevelManager.instance.currentLevel.levelMap.texture.GetPixel(x + 1, (y + 1)).b - 1) >= threshold))
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
        Color pixelColor = LevelManager.instance.currentLevel.levelObstacles.texture.GetPixel(x, y);
        GameObject parentObject = parent.gameObject;

        // if transparent, ignore 
        if (pixelColor.a == 0)
        {
            return;
        }

        foreach (LevelParser colorMapping in colorMappingsObstacles)
        {
            if (Mathf.Abs(pixelColor.r - colorMapping.color.r) <= threshold && Mathf.Abs(pixelColor.g - colorMapping.color.g) <= threshold && Mathf.Abs(pixelColor.b - colorMapping.color.b) <= threshold)
            {
                GameObject toSummon = null;

                if (colorMapping.prefab.Length > 1)
                {
                    int index = UnityEngine.Random.Range(0, colorMapping.prefab.Length);
                    toSummon = Instantiate(colorMapping.prefab[index], parent);
                }
                else
                {
                    toSummon = Instantiate(colorMapping.prefab[0], parent);
                }


                toSummon.GetComponent<SpriteRenderer>().sortingOrder = parentObject.GetComponent<SpriteRenderer>().sortingOrder + 1;     
                
                Tile t = parent.GetComponent<Tile>();
                t.isFull = true;

                if (!toSummon.CompareTag("Obstacle"))
                {
                    SetParentByTag(toSummon);
                   
                }

                if (toSummon.CompareTag("Player"))
                {

                    Entity et = toSummon.GetComponent<Player>();

                    EntityManager.instance.SetPlayer(et);
                    EntityManager.instance.CheckNextSpawnTileEnemy();
                    et.AddGooTiles(t);
                    et.SetCurrentTile(t);
                    t.isMainPlayerBody = true; //new

                    Vector3 position = new Vector3(parent.position.x, parent.position.y + (offsetY * 2), parent.position.z);
                    et.transform.position = position;
                }

                if (toSummon.CompareTag("Enemy"))
                {
                    EntityManager.instance.slugSpawnTiles.Add(t);

                    Destroy(toSummon.gameObject);

                    t.isFull = false;
                    //Entity et = toSummon.GetComponent<Slug>();    

                    //EntityManager.instance.AddEnemyToEnemiesList(et);
                    //et.SetCurrentTile(t);

                    //Vector3 position = new Vector3(parent.position.x, parent.position.y + (offsetY * 2), parent.position.z);
                    //et.transform.position = position;
                }

                if (toSummon.CompareTag("Food"))
                {
                    t.isFood = true;
                    t.foodObject = toSummon;

                    t.gameObject.AddComponent<TutorialObject>().SetDescription(TypeOfTutorial.Food);
                }

                if (toSummon.CompareTag("Kinine"))
                {
                    Animator anim = toSummon.GetComponent<Animator>();
                    anim.SetBool("Spawn", true);

                    t.isKinine = true;
                    t.foodObject = toSummon;

                    t.gameObject.AddComponent<TutorialObject>().SetDescription(TypeOfTutorial.Kinine);
                }

                if (toSummon.CompareTag("Salt"))
                {
                    Animator anim = toSummon.GetComponent<Animator>();
                    anim.SetBool("Spawn", true);

                    t.isSalt = true;
                    t.foodObject = toSummon;

                    t.gameObject.AddComponent<TutorialObject>().SetDescription(TypeOfTutorial.Salt);
                }

                if (toSummon.CompareTag("Lock Salt") || toSummon.CompareTag("Lock Kinine"))
                {
                    t.isLocked = true;
                    t.isFull = false;
                    Animator anim = Instantiate(UIManager.instance.lockPrefab, parent).GetComponent<Animator>();
                    ConnecetdElement connected = anim.GetComponent<ConnecetdElement>();

                    connected.connectedElement = t.gameObject;



                    if (toSummon.CompareTag("Lock Salt"))
                    {

                        LevelManager.instance.saltLocks.Add(anim);

                        GreyScalLock greyScale =  toSummon.GetComponent<GreyScalLock>();

                        SpriteRenderer greyScaleChildOne = greyScale.childGrey.transform.GetChild(0).GetComponent<SpriteRenderer>();
                        SpriteRenderer greyScaleChildTwo = greyScale.childGrey.transform.GetChild(1).GetComponent<SpriteRenderer>();

                        greyScaleChildOne.sortingOrder = parentObject.GetComponent<SpriteRenderer>().sortingOrder;
                        greyScaleChildTwo.sortingOrder = parentObject.GetComponent<SpriteRenderer>().sortingOrder + 2;

                        SpriteRenderer colorScaleChildOne = greyScale.childColor.transform.GetChild(0).GetComponent<SpriteRenderer>();
                        SpriteRenderer colorScaleChildTwo = greyScale.childColor.transform.GetChild(1).GetComponent<SpriteRenderer>();

                        colorScaleChildOne.sortingOrder = parentObject.GetComponent<SpriteRenderer>().sortingOrder;
                        colorScaleChildTwo.sortingOrder = parentObject.GetComponent<SpriteRenderer>().sortingOrder + 2;



                        greyScale.childGrey.SetActive(true);
                        greyScale.childColor.SetActive(false);
                        t.gameObject.AddComponent<TutorialObject>().SetDescription(TypeOfTutorial.SaltTile);

                        t.connectedLockDisplay = toSummon;

                        t.GoToGreyScale();
                    }
                    else
                    {
                        LevelManager.instance.kinineLocks.Add(anim);

                        GreyScalLock greyScale = toSummon.GetComponent<GreyScalLock>();

                        SpriteRenderer greyScaleChildOne = greyScale.childGrey.transform.GetChild(0).GetComponent<SpriteRenderer>();
                        SpriteRenderer greyScaleChildTwo = greyScale.childGrey.transform.GetChild(1).GetComponent<SpriteRenderer>();

                        greyScaleChildOne.sortingOrder = parentObject.GetComponent<SpriteRenderer>().sortingOrder;
                        greyScaleChildTwo.sortingOrder = parentObject.GetComponent<SpriteRenderer>().sortingOrder + 2;

                        SpriteRenderer colorScaleChildOne = greyScale.childColor.transform.GetChild(0).GetComponent<SpriteRenderer>();
                        SpriteRenderer colorScaleChildTwo = greyScale.childColor.transform.GetChild(1).GetComponent<SpriteRenderer>();

                        colorScaleChildOne.sortingOrder = parentObject.GetComponent<SpriteRenderer>().sortingOrder;
                        colorScaleChildTwo.sortingOrder = parentObject.GetComponent<SpriteRenderer>().sortingOrder + 2;



                        greyScale.childGrey.SetActive(true);
                        greyScale.childColor.SetActive(false);
                        t.gameObject.AddComponent<TutorialObject>().SetDescription(TypeOfTutorial.kinineTile);

                        t.connectedLockDisplay = toSummon;

                        t.GoToGreyScale();
                    }
                }
            }
        }
    }   
    void GenerateBeetleData(Transform parent, int x , int y )
    {
        Color pixelColor = LevelManager.instance.currentLevel.levelBeetleData.texture.GetPixel(x, y);
        GameObject parentObject = parent.gameObject;

        // if transparent, ignore 
        if (pixelColor.a == 0)
        {
            return;
        }

        foreach (LevelParser colorMapping in colorMappingsBeetleData)
        {
            if (Mathf.Abs(pixelColor.r - colorMapping.color.r) <= threshold && Mathf.Abs(pixelColor.g - colorMapping.color.g) <= threshold && Mathf.Abs(pixelColor.b - colorMapping.color.b) <= threshold)
            {
                GameObject toSummon = null;

                if (colorMapping.prefab.Length > 1)
                {
                    int index = UnityEngine.Random.Range(0, colorMapping.prefab.Length);
                    toSummon = colorMapping.prefab[index];
                }
                else
                {
                    toSummon = colorMapping.prefab[0];
                }

                Tile t = parent.GetComponent<Tile>();

                if (toSummon.CompareTag("Beetle Spawn Point"))
                {
                    EntityManager.instance.beetleSpawnTiles.Add(t);
                    EntityManager.instance.beetleTargetAndSpawnTiles.Add(t);                  
                }

                if (toSummon.CompareTag("Beetle Target Point"))
                {
                    EntityManager.instance.beetleTargetTiles.Add(t);
                    EntityManager.instance.beetleTargetAndSpawnTiles.Add(t);
                }
            }
        }
    }   

    public void SetParentByTag(GameObject inObject)
    {
        switch (inObject.tag)
        {
            case "Player":
                inObject.transform.SetParent(ObjectRefrencer.instance.blobs.transform);
                break;

            case "Enemy":
                inObject.transform.SetParent(ObjectRefrencer.instance.enemies.transform);
                break;

            case "Salt":
                inObject.transform.SetParent(ObjectRefrencer.instance.blobs.transform);
                break;
            case "Kinine":
                inObject.transform.SetParent(ObjectRefrencer.instance.blobs.transform);
                break;


            default:
                break;
        }
    }
}
