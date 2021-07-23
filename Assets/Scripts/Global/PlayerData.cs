using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public string id;
    public string fName;
    public string lName;

    public int speed;
    public bool slowed;

    public Vector3Int[] curPath;

    //for platforming
    public Vector3Int curPlatformLoc;

    //for sim
    public Vector3Int curSimLoc;
    public TData curLoc;
    public TileTypes[] allowedTypes = new TileTypes[0];

    public int[] personaPoints = new int[3];
}
