using System;
using UnityEngine;

[Serializable]
public class LoadedData
{
    //for loaded TData in simulation    
    public int[] tileId;

    public int[] tileType;
    public int[] tileLayer;

    public string[] tileName;
    public Vector3Int[] tilePos;
    public Vector2Int[] tileSize;

    public int[] tileHP;
    public int[] tileAC;
    public int[] tileFootTraffic;

    public Vector3Int[][] tileNeighbors;
    public string[] sprite;

    //building details for onmouseover
    public bool[] tileCanInterract;
    public bool[] tileIsBuilding;
    public bool[] tileOwned;
}
