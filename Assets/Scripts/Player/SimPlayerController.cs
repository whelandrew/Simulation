using UnityEngine;
public class SimPlayerController : MonoBehaviour
{
    public bool canControl;
    public SimGameboardController gController;

    public GameObject interactionRange;

    public Rigidbody2D rBody;
    public SpriteRenderer sprite;
    public PlayerData pData;
    public Pathfinding pathing;

    public int speed;
    private int baseSpeed;

    private int facing = 0;
    bool isWalking = false;
    public Vector2 direction = Vector2.zero;
    private Vector2 finalVelocity = Vector2.zero;
    public Vector2 stopDirection = Vector2.zero;

    public KeyCode[] MappedKeys = new KeyCode[] { KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.Space, KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.UpArrow, KeyCode.DownArrow };
    private float inputTimer = 0;

    private int curPathVal = 0;

    void Start()
    {
        baseSpeed = pData.speed;
        speed = baseSpeed;
    }

    void Update()
    {
        if (canControl)
        {
            if (CanMove())
            {
                Movement();
            }


            if (Input.GetMouseButtonDown(0))
            {
                if (!isWalking)
                {
                    //MoveTowards();
                }
            }

            WalkPath();
        }
    }

    private void FixedUpdate()
    {
        rBody.velocity = finalVelocity;
    }

    private void MoveTowards()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2Int.down);
        if (hit.collider.tag == "Tile")
        {
            TData tile = hit.collider.GetComponent<TData>();
            TileTypes[] types = new TileTypes[pData.allowedTypes.Length + 2];
            for (int i = 0; i < pData.allowedTypes.Length; i++)
            {
                types[i] = pData.allowedTypes[i];
            }
            types[types.Length - 1] = tile.tileType;
            types[types.Length - 2] = pData.curLoc.tileType;

            pData.curPath = pathing.FindPath(pData.curLoc, tile, types);
        }
    }

    private void WalkPath()
    {
        if (pData.curPath == null)
            return;

        if (pData.curPath.Length > 0)
        {
            isWalking = true;
            transform.position = Vector3.MoveTowards(transform.position, pData.curPath[curPathVal], pData.speed * Time.deltaTime);

            if (transform.position == pData.curPath[curPathVal])
            {
                curPathVal++;

                if (curPathVal == pData.curPath.Length)
                {
                    curPathVal = 0;
                    isWalking = false;
                    pData.curPath = new Vector3Int[0];
                }
            }
        }
    }

    private bool CanMove()
    {
        inputTimer += 1f * Time.deltaTime;

        if (gController.UIOn)
        {
            return false;
        }

        if (!Input.anyKey)
        {
            finalVelocity = Vector2.zero;
            isWalking = false;
            return false;
        }
        else
        {
            for (int i = 0; i < MappedKeys.Length; i++)
            {
                if (Input.GetKey(MappedKeys[i]))
                {
                    inputTimer = 0;
                    isWalking = true;
                    return true;
                }
            }
        }

        return false;
    }

    private void Movement()
    {
        if (Input.GetKey(KeyCode.A))
        {
            direction = Vector2.left;
            facing = -1;
            StopCheck(Vector2.left);
        }
        if (Input.GetKey(KeyCode.D))
        {
            direction = Vector2.right;
            facing = 1;
            StopCheck(Vector2.right);
        }
        if (Input.GetKey(KeyCode.W))
        {
            direction = Vector2.up;
            StopCheck(Vector2.up);
        }
        if (Input.GetKey(KeyCode.S))
        {
            direction = Vector2.down;
            StopCheck(Vector2.down);
        }

        finalVelocity = direction * speed;
    }

    private void StopCheck(Vector2 face)
    {
        if (stopDirection == face)
        {
            direction = Vector2.zero;
        }
    }
}
