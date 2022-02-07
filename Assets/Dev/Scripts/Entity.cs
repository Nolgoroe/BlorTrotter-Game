using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public abstract class Entity : MonoBehaviour
{
    public Tile currentTile; /// HAS TO BE A BETTER WAY TO DO THIS

    public abstract void SayMyName();

    public virtual async Task MoveEntity(Tile targetTile)
    {
        await Task.Yield();
    }

    public void SetTargetTile() /// HAS TO BE A BETTER WAY TO DO THIS
    {
        currentTile = GridManager.instance.GetAdjacentTile(currentTile);
    }

    public abstract void PlayerAnimation();
}
