using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour, IManageable
{

    // all here is new

    public static PathFinding instance;

    public Tile startTile, targetTile;

    public Entity sendingEntity;

    public void initManager()
    {
        instance = this;
    }

   

    public List<Tile> FindPath(Tile startTile, Tile TargetTile, Entity callingEntity)
    {
        List<Tile> finalPath = new List<Tile>();

        List<Tile> openSet = new List<Tile>();
        HashSet<Tile> closedSet = new HashSet<Tile>();

        openSet.Add(startTile);


        while (openSet.Count > 0)
        {
            Tile currentTile = openSet[0];

            for (int i = 1; i < openSet.Count; i++)
            {
                if(openSet[i].fCost < currentTile.fCost || openSet[i].fCost == currentTile.fCost && openSet[i].hCost < currentTile.hCost)
                {
                    currentTile = openSet[i];
                }
            }

            openSet.Remove(currentTile);
            closedSet.Add(currentTile);

            if(currentTile == TargetTile)
            {
                finalPath = RetracePath(startTile, TargetTile);
                return finalPath;
            }

            foreach (Tile neighbourTile in GridManager.instance.GetNeighbours(currentTile, callingEntity))
            {
                if((neighbourTile.isFull && !neighbourTile.isMainPlayerBody) || neighbourTile.isWaterTile || closedSet.Contains(neighbourTile))
                {
                    continue;
                }


                int newMoveCostToNeighbour = currentTile.gCost + GetDistance(currentTile, neighbourTile);

                if(newMoveCostToNeighbour < neighbourTile.gCost || !openSet.Contains(neighbourTile))
                {
                    neighbourTile.gCost = newMoveCostToNeighbour;
                    neighbourTile.hCost = GetDistance(neighbourTile, TargetTile);
                    neighbourTile.parentTileForPath = currentTile;

                    openSet.Add(neighbourTile);
                }
            }
        }

        return null;
    }

    int GetDistance(Tile tileA, Tile tileB)
    {
        int distX = Mathf.Abs(tileA.tileX - tileB.tileX);
        int distY = Mathf.Abs(tileA.tileY - tileB.tileY);

        if(distX > distY)
        {
            return 10 * (distX - distY);
        }
        else
        {
            return 10 * (distY - distX);
        }
    }

    List<Tile> RetracePath(Tile startTile, Tile endTile)
    {
        List<Tile> path = new List<Tile>();
        Tile currentTile = endTile;


        while (currentTile != startTile)
        {
            path.Add(currentTile);
            currentTile = currentTile.parentTileForPath;
        }

        path.Reverse();

        return path;        
    }
}
