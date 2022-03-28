using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;

public enum EntityTypes { player, Slug, Beetle};

public class EntityManager : MonoBehaviour, IManageable  //singleton , only instantiate one time 
{
    public static EntityManager instance;

    [SerializeField] private Entity player;
    

    public List<Entity> allEnemies; // list is dynamic

    public List<Tile> slugSpawnTiles;
    public List<Tile> beetleSpawnTiles;
    public List<Tile> beetleTargetTiles;
    public List<Tile> beetleTargetAndSpawnTiles;

    public GameObject slugPrefab;
    public GameObject beetlePrefab;

    public Tile nextTileToSpawnEnemySlug;
    public Tile nextTileToSpawnEnemyBeetle;

    public bool summonSlug, summonBeetle;

    public void initManager()
    {
        instance = this;
        allEnemies = new List<Entity>();
        slugSpawnTiles = new List<Tile>();
        beetleSpawnTiles = new List<Tile>();
        beetleTargetAndSpawnTiles = new List<Tile>();

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

        if (LevelManager.instance.currentLevel.hasEnemies)
        {
            if (GridManager.instance.allEnemyGooTiles.Count > 0)
            {
                GridManager.instance.CountdownEnemyGooTiles();
            }

            if (allEnemies.Count > 0)
            {
                MoveAllEnemies();
            }
            else
            {
                SetPlayerTurn();
            }

            DecreaseSpawnEnemyCooldown();

            CheckNextSpawnTileEnemy();
        }
        else
        {
            SetPlayerTurn();
        }
    }

    public void DecreaseSpawnEnemyCooldown()
    {
        LevelManager.instance.currentCooldownSummonEnemies--;

        if (LevelManager.instance.currentCooldownSummonEnemies <= 0)
        {
            if (!CheckLimitOfEnemiesReachedGlobal())
            {
                SpawnEnemy();
            }

            LevelManager.instance.currentCooldownSummonEnemies = LevelManager.instance.currentLevel.summonEnemyCooldown;
        }
    }

    public void CheckNextSpawnTileEnemy()
    {
        if (LevelManager.instance.currentCooldownSummonEnemies - 1 == 0 && (!nextTileToSpawnEnemySlug || !nextTileToSpawnEnemyBeetle))
        {
            if (!nextTileToSpawnEnemySlug && LevelManager.instance.currentLevel.hasSlugs)
            {
                int rand = UnityEngine.Random.Range(0, slugSpawnTiles.Count);

                Tile t = slugSpawnTiles[rand].GetComponent<Tile>();

                int counter = 1000;

                while (t.isFull)
                {
                    counter--;
                    rand = UnityEngine.Random.Range(0, slugSpawnTiles.Count);
                    t = slugSpawnTiles[rand].GetComponent<Tile>();

                    if (counter <= 0)
                    {
                        Debug.LogError("PROBLEM WITH SLUG SPAWN");
                        break;
                    }
                }

                nextTileToSpawnEnemySlug = t;
            }

            if (!nextTileToSpawnEnemyBeetle && LevelManager.instance.currentLevel.hasBeetles)
            {
                int rand = UnityEngine.Random.Range(0, beetleSpawnTiles.Count);

                Tile t = beetleSpawnTiles[rand].GetComponent<Tile>();

                rand = UnityEngine.Random.Range(0, beetleSpawnTiles.Count);
                t = beetleSpawnTiles[rand].GetComponent<Tile>();

                nextTileToSpawnEnemyBeetle = t;
            }


            bool reachedMaxConcurrentBeetles = CheckLimitOfEnemiesReached(EntityTypes.Beetle);
            bool reachedMaxConcurrentSlugs = CheckLimitOfEnemiesReached(EntityTypes.Slug);

            if (!reachedMaxConcurrentBeetles && !reachedMaxConcurrentSlugs)
            {
                int rand = UnityEngine.Random.Range(0, 2);

                if (rand == 0)
                {
                    GridManager.instance.SetSpawnTileON(nextTileToSpawnEnemySlug);
                    summonSlug = true;
                    summonBeetle = false;
                }
                else
                {
                    GridManager.instance.SetSpawnTileON(nextTileToSpawnEnemyBeetle);
                    summonSlug = false;
                    summonBeetle = true;
                }

            }
            else if (!reachedMaxConcurrentBeetles)
            {
                GridManager.instance.SetSpawnTileON(nextTileToSpawnEnemyBeetle);
                summonSlug = false;
                summonBeetle = true;
            }
            else if (!reachedMaxConcurrentSlugs)
            {
                GridManager.instance.SetSpawnTileON(nextTileToSpawnEnemySlug);
                summonSlug = true;
                summonBeetle = false;
            }
        }
    }

    public async void SpawnEnemy()
    {
        if (summonSlug)
        {

            GameObject toSummon = Instantiate(slugPrefab, nextTileToSpawnEnemySlug.transform);
            Transform parent = nextTileToSpawnEnemySlug.transform;
            Entity et = toSummon.transform.GetChild(0).GetComponent<Slug>();

            if (nextTileToSpawnEnemySlug.isGooPiece)
            {
                toSummon.transform.GetChild(0).GetComponent<Slug>().EatGooPiece(nextTileToSpawnEnemySlug);
            }

            nextTileToSpawnEnemySlug.SetEnemySpawnDataSlug();


            if (!GridManager.instance.allEnemyGooTiles.Contains(nextTileToSpawnEnemySlug))
            {
                GridManager.instance.allEnemyGooTiles.Add(nextTileToSpawnEnemySlug);
            }

            et.AddGooTiles(nextTileToSpawnEnemySlug);

            foreach (Tile element in et.gooTiles)
            {
                GridManager.instance.LeaveGooOnTileEnemy(element);
            }


            Vector3 position = new Vector3(parent.position.x, parent.position.y + (LevelEditor.instance.offsetY * 2), parent.position.z);
            et.transform.position = position;
            toSummon.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = parent.GetComponent<SpriteRenderer>().sortingOrder + 1;

            LevelEditor.instance.SetParentByTag(toSummon);


            AddEnemyToEnemiesList(et);
            et.SetCurrentTile(nextTileToSpawnEnemySlug);


            GridManager.instance.RemoveSpawnTileDisplay(nextTileToSpawnEnemySlug);

            nextTileToSpawnEnemySlug = null;

            await Task.Delay(500);
            SoundManager.instance.PlaySound(SoundManager.instance.SFXAudioSource, Sounds.Slug_Spawning);

        }
        else
        {
            GameObject toSummon = Instantiate(beetlePrefab, nextTileToSpawnEnemyBeetle.transform);

            Transform parent = nextTileToSpawnEnemyBeetle.transform;
            Entity et = toSummon.transform.GetComponent<Beetle>();

            Vector3 position = new Vector3(parent.position.x, parent.position.y + (LevelEditor.instance.offsetY * 2), parent.position.z);
            et.transform.position = position;
            toSummon.transform.GetComponent<SpriteRenderer>().sortingOrder = parent.GetComponent<SpriteRenderer>().sortingOrder + 2;

            LevelEditor.instance.SetParentByTag(toSummon);

            AddEnemyToEnemiesList(et);
            et.SetCurrentTile(nextTileToSpawnEnemyBeetle);


            GridManager.instance.RemoveSpawnTileDisplay(nextTileToSpawnEnemyBeetle);

            nextTileToSpawnEnemyBeetle.SetEnemySpawnDataBeetle();

            nextTileToSpawnEnemyBeetle.isBeetleForTutorial = true;

            nextTileToSpawnEnemyBeetle = null;

        }
    }

    public void CallPrepareToMovePlayer(Tile targetTile) //new
    {
        player.PrepareToMove(targetTile);
    }


    public async void MoveAllEnemies() // move all the enemies in the same time  
    {
        List<Task> tasks = new List<Task>();

        for (int i = 0; i < allEnemies.Count; i++)
        {

            //Tile target = allEnemies[i].SetTargetTileForAstarPath(); 

            tasks.Add(allEnemies[i].ManageTurnStart());
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

    public bool CheckLimitOfEnemiesReached(EntityTypes type)
    {
        int counter = 0;

        switch (type)
        {
            case EntityTypes.Slug:

                foreach (Entity et in allEnemies)
                {
                    if (et.typeEntity == EntityTypes.Slug)
                    {
                        counter++;
                    }
                }

                if (counter == LevelManager.instance.currentLevel.maxConcurrentSlugs)
                {
                    return true;
                }

                break;
            case EntityTypes.Beetle:
                foreach (Entity et in allEnemies)
                {
                    if (et.typeEntity == EntityTypes.Beetle)
                    {
                        counter++;
                    }
                }

                if (counter == LevelManager.instance.currentLevel.maxConcurrentBeetles)
                {
                    return true;
                }

                break;
            default:
                break;
        }

        return false;
    }
    public bool CheckLimitOfEnemiesReachedGlobal()
    {
        if(allEnemies.Count == LevelManager.instance.currentLevel.maxConcurrentGlobalEnemies)
        {
            return true;
        }

        return false;
    }
}
