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

    public int HP;
    public int AC;
    public int footTraffic;    

    public Vector3Int[] neighbors = new Vector3Int[8];

    //for pathfinding
    public int gCost;
    public int hCost;
    public int fCost;
    public TData cameFrom;
    public int pathVal;

    //building details for onmouseover
    public bool canInterract;
    public bool isBuilding;
    public bool owned;
    public VillagerData owner;
}
