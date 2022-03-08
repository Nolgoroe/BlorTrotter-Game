using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class EntityManager : MonoBehaviour, IManageable  //singleton , only instantiate one time 
{
    public static EntityManager instance;

    [SerializeField] private Entity player;
    

    [SerializeField] private List<Entity> allEnemies; // list is dynamic

    public void initManager()
    {
        instance = this;
        allEnemies = new List<Entity>();     
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

        if (!targetTile.isGooPiece)
        {
            MoveAllEnemies();
        }
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
        Debug.Log("All enemies done moving");
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


    public void SetEnemyTargetTiles()
    {
        for (int i = 0; i < allEnemies.Count; i++)
        {
            allEnemies[i].SetTargetTileForAstarPath();
        }
    }
}
