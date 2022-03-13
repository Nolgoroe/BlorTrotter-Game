using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;

public class EntityManager : MonoBehaviour, IManageable  //singleton , only instantiate one time 
{
    public static EntityManager instance;

    [SerializeField] private Entity player;
    

    [SerializeField] private List<Entity> allEnemies; // list is dynamic

    public List<Tile> enemySpawnTiles;

    public GameObject slugPrefab;

    public void initManager()
    {
        instance = this;
        allEnemies = new List<Entity>();
        enemySpawnTiles = new List<Tile>();

        Debug.Log("success Entity Manager");
    }

    public void AddEnemyToEnemiesList(Entity enemyToAdd) // add enemy to the list 
    {
        allEnemies.Add(enemyToAdd);
    }

    public void SetPlayer(Entity createdPlayer)
    {
        player = createdPlayer;
    }

    public Entity GetPlayer()
    {
        return player;
    }



    public void ResetEntityManagerData()
    {
        player = null;
        allEnemies.Clear();
    }

    public async void MovePlayer(Tile targetTile) // wait for the player to move for moving all the enemies
    {
        await player.MoveEntity(targetTile);

        if (allEnemies.Count > 0)
        {
            MoveAllEnemies();
        }

        LevelManager.instance.DecreaseSummonEnemyCooldown();
    }

    public void SpawnEnemy()
    {
        int rand = UnityEngine.Random.Range(0, enemySpawnTiles.Count);

        GameObject toSummon = Instantiate(slugPrefab, enemySpawnTiles[rand].transform);
        Transform parent = enemySpawnTiles[rand].transform;
        Entity et = toSummon.GetComponent<Slug>();
        Tile t = enemySpawnTiles[rand].GetComponent<Tile>();

        t.isFull = true;

        Vector3 position = new Vector3(parent.position.x, parent.position.y + (LevelEditor.instance.offsetY * 2), parent.position.z);
        et.transform.position = position;
        toSummon.GetComponent<SpriteRenderer>().sortingOrder = parent.GetComponent<SpriteRenderer>().sortingOrder + 1;

        LevelEditor.instance.SetParentByTag(toSummon);


        AddEnemyToEnemiesList(et);
        et.SetCurrentTile(enemySpawnTiles[rand]);
    }

    public void CallPrepareToMovePlayer(Tile targetTile) //new
    {
        player.PrepareToMove(targetTile);
    }


    public async void MoveAllEnemies() // move all the enemies in the same time  
    {
        List<Task> tasks = new List<Task>();

        List<Entity> tempList = new List<Entity>();
        tempList.AddRange(allEnemies);

        for (int i = 0; i < tempList.Count; i++)
        {
            if (tempList[i].enemyPath.Count <= 0)
            {
                tempList[i].ReleaseTargetTile();
                await RemoveEnemyFromList(tempList[i], allEnemies);

                await Task.Delay(1 * 1000);
                Destroy(tempList[i].gameObject);
            }
        }

        for (int i = 0; i < allEnemies.Count; i++)
        {

            //Tile target = allEnemies[i].SetTargetTileForAstarPath(); 
            allEnemies[i].ManageTurnStart();


            tasks.Add(allEnemies[i].MoveEntity(allEnemies[i].enemyPath[0])); 
        }

        await Task.WhenAll(tasks);

        SetPlayerTurn();
        //Debug.Log("All enemies done moving");
    }

    public async Task RemoveEnemyFromList(Entity entity, List<Entity> theList)
    {
        theList.Remove(entity);

        await Task.Yield();
    }

    public void SetPlayerTurn()
    {
        player.ManageTurnStart();
    }


    public void SetEnemyTargetTiles(Entity enemy)
    {
        enemy.SetTargetTileForAstarPath();
        //for (int i = 0; i < allEnemies.Count; i++)
        //{
        //    allEnemies[i].SetTargetTileForAstarPath();
        //}
    }

    public void SpawnPlayerRandomGooLocation()
    {
        int rand = UnityEngine.Random.Range(0, player.gooTiles.Count);

        player.PrepareToMove(player.gooTiles[rand]);
    }

    public bool CheckLimitOfEnemiesReached(int amount)
    {
        if(allEnemies.Count >= LevelManager.instance.currentLevel.maxConcurrentSlugs)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
