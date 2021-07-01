using UnityEngine;

public class VillagerData : MonoBehaviour
{
    public string id;

    public Vector2Int pos;
    public int speed;

    public bool isActive;

    public bool isMoving;
    public Vector3Int[] currentPath;

    public string FName;
    public string LName;

    public int Gender;

    public bool hasJob;
    public JobType job;
    public Vector3Int jobLoc;
    public int jobID;
    public bool atWork;

    public bool hasHome;
    public Vector3Int homeLoc;
    public int homeID;
    public bool atHome;

    public TData currentLocation;
    public TileTypes goingTo;
    public TData target;
    public bool atLocation;
}
