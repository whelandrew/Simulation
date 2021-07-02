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
            tile.fCost = tile.gCost + tile.hCost;
            tile.cameFrom = null;
        }

        if(start == null)
        {
            Debug.LogError("start == null");
            return null;
        }

        start.gCost = 0;
        start.hCost = CalculateDistancecost(start, end);
        start.fCost = start.gCost + start.hCost;

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
            if(cur.neighbors == null)
            {
                Debug.LogError("cur.neighbors == null");
                return null; 
            }

            //TODO Fix pathing
            AddToPath(cur, neighbors, end, TileTypes.Road);
        }

        //out of cycle. No path found
        Debug.LogWarning("No Path Found");
        return new List<Vector3Int>();
    }

    private void AddToPath(TData cur, Vector3Int[] neighbors, TData end, TileTypes allowedTile)
    {
        TData roadTile = null;
        for (int i = 0; i < neighbors.Length; i++)
        {
            TData tile = tManager.FindTileData(neighbors[i]);
            if (tile.tileType == allowedTile)
            {
                roadTile = tile;
                continue;
            }
        }

        TData nTile = null;
        if (roadTile != null)
        {
            nTile = roadTile;
        }

        if (nTile != null)
        {
            if (closedList.Contains(nTile))
            {
                return;
            }

            int tentativeGCost = cur.gCost + CalculateDistancecost(cur, nTile);
            if (tentativeGCost < nTile.gCost)
            {
                nTile.cameFrom = cur;
                nTile.gCost = tentativeGCost;
                nTile.hCost = CalculateDistancecost(nTile, end);
                nTile.fCost = nTile.gCost + nTile.hCost;

                if (!openList.Contains(nTile))
                {
                    openList.Add(nTile);
                }
            }
        }
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

        return path;
    }

    private TData GetLowestFCost(List<TData> list)
    {
        TData lowestCost = list[0];
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].fCost < lowestCost.fCost)
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
}
