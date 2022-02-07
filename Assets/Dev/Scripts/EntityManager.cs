using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class EntityManager : MonoBehaviour, IManagable
{
    public static EntityManager instance;

    [SerializeField] private Entity player;

    [SerializeField] private List<Entity> allEnemies;

    public void initManager()
    {
        instance = this;
        allEnemies = new List<Entity>();
        Debug.Log("success Entity Manager");
    }

    public void AddEnemyToEnemiesList(Entity enemyToAdd)
    {
        allEnemies.Add(enemyToAdd);
    }

    public void SetPlayer(Entity createdPlayer)
    {
        player = createdPlayer;
    }


    public void ResetEntityManagerData()
    {
        player = null;
        allEnemies.Clear();
    }


    public async void MovePlayer(Tile targetTile)
    {
        player.SetTargetTile(); /// HAS TO BE A BETTER WAY TO DO THIS

        await player.MoveEntity(player.currentTile); /// HAS TO BE A BETTER WAY TO DO THIS.... THE MOVE ENTITY PART - THE AWAIT PART IS COOL

        MoveAllEnemies();
    }

    public async void MoveAllEnemies()
    {
        List<Task> tasks = new List<Task>();

        for (int i = 0; i < allEnemies.Count; i++)
        {
            allEnemies[i].SetTargetTile(); /// HAS TO BE A BETTER WAY TO DO THIS

            tasks.Add(allEnemies[i].MoveEntity(allEnemies[i].currentTile)); /// HAS TO BE A BETTER WAY TO DO THIS
        }

        await Task.WhenAll(tasks);

        Debug.Log("All enemies done moving");
    }








    [ContextMenu("test inheritence")]
    public void TestInheritence() //DELTE THIS AFTER SHOWING NATHAN
    {
        foreach (Entity e in allEnemies)
        {
            e.SayMyName();
        }

        player.SayMyName();
    }

    [ContextMenu("Move Enemeies")]
    public void TestMoveEnemies() //DELTE THIS AFTER SHOWING NATHAN
    {
        MoveAllEnemies();
    }
}
