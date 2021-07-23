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

    //saving player data
    public Vector3Int[] playerCurPath;    
    public Vector3Int playerCurSimLoc;
    public Vector3Int playerCurLoc;

    //saving villager data
    public string[] vId;
    public Vector2Int[] vpos;
    public int[] vspeed;
    public bool[] visActive;
    public bool[] visMoving;
    public Vector3Int[][] vcurrentPath;
    public string[] vFName;
    public string[] vLName;
    public int[] vGender;
    public bool[] vhasJob;
    public int[] vjob;
    public Vector3Int[] vjobLoc;
    public int[] vjobID;
    public bool[] vatWork;
    public bool[] vhasHome;
    public Vector3Int[] vhomeLoc;
    public int[] vhomeID;
    public bool[] vatHome;
    public Vector3Int[] vcurrentLocation;
    public int[] vgoingTo;
    public Vector3Int[] vtarget;
    public bool[] vatLocation;
    public int[][] vallowedTypes;
    public int[] vmood;
    public int[] vAC;
    public int[] vHP;
    public int[] vAGI;

    //saving enemy data
    public string[] eid;
    public Vector2Int[] epos;
    public bool[] eisMoving;
    public bool[] eisActive;
    public int[] espeed;
    public int[] eHP;
    public int[] eAC;
    public int[] eAgility;
    public string[] efName;
    public string[] elName;
    public int[] egender;
    public bool[] eslowed;
    public Vector3[] ecurrentLoc;
    public Vector3Int[][] ecurrentPath;
    public int[][] eallowedTypes;
    public Vector3Int[] etargetTile;
}
