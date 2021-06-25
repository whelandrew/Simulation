using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    private int x;
    private int y;

    public int gCost;
    public int hCost;
    public int fCost;

    public PathNode cameFromNode;

    public PathNode(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public override string ToString()
    {
        return x + "," + y;
    }
}

public class Pathfinding : MonoBehaviour
{
    public const int MOVE_STRAIGHT_COST = 10;
    public const int MOVE_DIAGONAL_COST = 14;

    public TileManager tManager;
    List<TData> alltiles = new List<TData>();
    List<TData> openList = new List<TData>();
    List<TData> closedList = new List<TData>();

    public List<Vector3Int> FindPath(TData start, TData end)
    {
        if (alltiles.Count < 1)
        {
            alltiles = tManager.GetAllTileData();
        }

        openList.Add(start);
        closedList = new List<TData>();

        //initialize
        for (int i = 0; i < alltiles.Count; i++)
        {
            TData tile = alltiles[i].GetComponent<TData>();
            tile.gCost = int.MaxValue;
            tile.fCost = CalculateFCost(tile);
            tile.cameFrom = null;
        }

        start.gCost = 0;
        start.hCost = CalculateDistancecost(start, end);
        start.fCost = CalculateFCost(start);

        //cycle
        while (openList.Count > 0)
        {
            TData cur = GetLowestFCost(openList);
            if (cur == end)
            {
                //end of path
                return CalculatePath(end);
            }

            openList.Remove(cur);
            closedList.Add(cur);

            //search neighbors
            Vector3Int[] neighbors = cur.neighbors;
            for (int j = 0; j < neighbors.Length; j++)
            {
                TData nTile = tManager.FindTileData(neighbors[j]);
                if (closedList.Contains(nTile))
                {
                    continue;
                }

                int tentativeGCost = cur.gCost + CalculateDistancecost(cur, nTile);
                if (tentativeGCost < nTile.gCost)
                {
                    nTile.cameFrom = cur;
                    nTile.gCost = tentativeGCost;
                    nTile.hCost = CalculateDistancecost(nTile, end);
                    nTile.fCost = CalculateFCost(nTile);

                    if (!openList.Contains(nTile))
                    {
                        openList.Add(nTile);
                    }
                }
            }
        }


        //out of cycle. No path found
        return null;
    }

    private List<Vector3Int> CalculatePath(TData end)
    {
        List<Vector3Int> path = new List<Vector3Int>();
        path.Add(end.pos);
        TData cur = end;
        while(cur.cameFrom != null)
        {
            path.Add(cur.cameFrom.pos);
            cur = cur.cameFrom;
        }
        path.Reverse();

        return path ;
    }

    private TData GetLowestFCost(List<TData> list)
    {
        TData lowestCost = new TData();
        for(int i=0;i<list.Count;i++)
        {
            if(list[i].fCost<lowestCost.fCost)
            {
                lowestCost = list[i];
            }
        }

        return lowestCost;
    }

    private int CalculateDistancecost(TData a, TData b)
    {
        int xDistance = Mathf.Abs(a.pos.x - b.pos.x);
        int yDistance = Mathf.Abs(a.pos.y - b.pos.y);
        int remaining = Mathf.Abs(xDistance - yDistance);
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST + remaining;
    }

    private int CalculateFCost(TData tile)
    {
        return tile.fCost = tile.gCost + tile.hCost;
    }

}
