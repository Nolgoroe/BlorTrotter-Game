using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public abstract class Entity : MonoBehaviour // abstract class for inheritance and polymorphism
{
    public Tile currentTile; /// HAS TO BE A BETTER WAY TO DO THIS

    public virtual async Task MoveEntity(Tile targetTile) // can be override
    {
        await Task.Yield();
    }

    public void SetTargetTile() /// HAS TO BE A BETTER WAY TO DO THIS
    {
        currentTile = GridManager.instance.GetAdjacentTile(currentTile);
    }

    public abstract void PlayAnimation();
}
