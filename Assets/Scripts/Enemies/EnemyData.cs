using UnityEngine;

public class EnemyData : MonoBehaviour
{
    public string id;
    public Vector2Int pos;

    public bool isMoving;
    public bool isActive;

    public int speed;

    public string fName;
    public string lName;

    public int gender;

    public bool slowed;

    public TData currentLoc;
    public Vector3Int[] currentPath;
}
