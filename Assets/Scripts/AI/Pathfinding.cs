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
    private ArrayManager aManager = new ArrayManager();

    TData[] allTiles = new TData[0];
    TData[] openList = new TData[0];
    TData[] closedList = new TData[0];
    
    private void Start()
    {
        allTiles = tManager.GetAllTileData();
    }
     
    public Vector3Int[] FindPath(TData start, TData end, TileTypes[] allowedTypes)
    {
        //initialize
        for (int i = 0; i < allTiles.Length; i++)
        {
            TData tile = allTiles[i];
            tile.gCost = int.MaxValue;
            tile.fCost = tile.gCost + tile.hCost;
            tile.cameFrom = null;
        }

        if(start == null)
        {
            Debug.LogError("start == null");
            return null;
        }

        openList = aManager.AddTData(openList, start);
        start.gCost = 0;
        start.hCost = CalculateDistancecost(start, end);
        start.fCost = start.gCost + start.hCost;

        if(end == null)
        {
            Debug.LogError("end == null");
            return null;
        }
        closedList = new TData[0];        

        //cycle
        while (openList.Length > 0)
        {
            openList = aManager.ResizeTData(openList);
            TData cur = GetLowestFCost(openList);
            if (cur == end)
            {
                //end of path
                return CalculatePath(end);
            }

            openList = aManager.RemoveTData(openList, cur);
            closedList = aManager.AddTData(closedList, cur);

            //search neighbors            
            if (cur == null || cur.neighbors == null)
            {
                //no way out!
                Debug.LogError("cur.neighbors == null");
                return null;
            }

            //check all valid neighbor tiles            
            for (int i = 0; i < cur.neighbors.Length; i++)
            {
                TData neighbor = tManager.FindTileData(cur.neighbors[i]);
                if (!aManager.FindMatchingTData(closedList, neighbor))
                {
                    for (int j = 0; j < allowedTypes.Length; j++)
                    {
                        if (neighbor.tileType == allowedTypes[j])
                        {
                            int tentativeGCost = cur.gCost + CalculateDistancecost(cur, neighbor);
                            if (tentativeGCost < neighbor.gCost)
                            {
                                neighbor.cameFrom = cur;
                                neighbor.gCost = tentativeGCost;
                                neighbor.hCost = CalculateDistancecost(neighbor, end);
                                neighbor.fCost = neighbor.gCost + neighbor.hCost;

                                if (!aManager.FindMatchingTData(openList, neighbor))
                                {
                                    openList = aManager.AddTData(openList, neighbor);
                                }
                            }
                        }
                    }
                }
            }
        }

        //out of cycle. No path found
        Debug.LogWarning("No Path Found");
        return null;
    }

    private Vector3Int[] CalculatePath(TData end)
    {
        Vector3Int[] path = new Vector3Int[100];
        path[0] = end.pos;

        TData cur = end;
        for (int i = 1; i < path.Length; i++)
        {
            if (cur.cameFrom != null)
            {
                path[i] = cur.cameFrom.pos;
                cur = cur.cameFrom;
            }
        }

        int count = 0;
        for (int i = 0; i < path.Length; i++)
        {
            if (path[i] != null)
            {
                if (path[i] != Vector3Int.zero)
                {
                    count++;
                }
            }
        }
        Vector3Int[] finalPath = new Vector3Int[count];

        count = 0;
        for (int i = finalPath.Length - 1; i >= 0; i--)
        {
            if (path[count] != null && path[count] != Vector3Int.zero)
            {
                finalPath[i] = path[count];
            }
            count++;
        }
        return finalPath;
    }
    
    private TData GetLowestFCost(TData[] list)
    {        
        if(list == null || list.Length<1)
        {
            list = aManager.ResizeTData(list);
        }

        TData lowestCost = list[0];
        if (lowestCost == null)
        {
            Debug.Log("lowestCost == null");
            return null;
        }

        for (int i = 0; i < list.Length; i++)
        {
            if (list[i] != null)
            {
                if (list[i].fCost < lowestCost.fCost)
                {
                    lowestCost = list[i];
                }
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
