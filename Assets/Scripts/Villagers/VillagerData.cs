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

    public bool hasHome;
    public Vector3Int homeLoc;
    public string homeID;

    public TData currentLocation;
}
