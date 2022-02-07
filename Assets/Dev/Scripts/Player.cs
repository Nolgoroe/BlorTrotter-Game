using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class Player : Entity
{
    public override void SayMyName()
    {
        Debug.Log("I AM THE PLAYER!!!");
    }

    public override async Task MoveEntity(Tile targetTile)
    {
        await Task.Yield();
    }

    public override void PlayerAnimation()
    {
        /// Set player animation Data here
    }
}
