using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class Player : Entity
{
    public override async Task MoveEntity(Tile targetTile)
    {
        await Task.Yield();
    }

    public override void PlayAnimation()
    {
        /// Set player animation Data here
    }
}
