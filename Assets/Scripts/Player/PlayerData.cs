using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public string id;

    public int speed;

    public string fName;
    public string lName;

    public bool slowed;

    public TData currentLoc;
    public Vector3Int[] currentPath;
    public TileTypes[] allowedTypes = new TileTypes[] { TileTypes.Road, TileTypes.Road };
}
