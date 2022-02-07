using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class Slug : Entity
{

    public override void SayMyName()
    {
        Debug.Log("I AM SLUG");
    }

    public override async Task MoveEntity(Tile targetTile)
    {
        await Task.Delay(3 * 1000);

        Debug.Log("ENEMY DONE");
    }

    public override void PlayerAnimation()
    {
        /// Set Slug animation Data here
    }
}
