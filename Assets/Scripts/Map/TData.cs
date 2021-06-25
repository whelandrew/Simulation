using UnityEngine;

public class TData : MonoBehaviour
{
    //collection of information for each tile
    public int id;

    public TileTypes tileType;
    public TileLayers tileLayer;

    public string name;
    public Vector3Int pos;
    public Vector2Int size;

    public Vector3Int[] neighbors;

    //for pathfinding
    public int gCost;
    public int hCost;
    public int fCost;
    public TData cameFrom;
}
