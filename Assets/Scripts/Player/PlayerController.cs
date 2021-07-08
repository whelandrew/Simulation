using UnityEngine;

public class playerController : MonoBehaviour
{
    public bool canControl;
    public GameboardController gController;

    public GameObject interactionRange;
    private SpriteRenderer[] interactionSprites;

    private ArrayManager aManager = new ArrayManager();

    public Rigidbody2D rBody;
    public BoxCollider2D bCollider;
    public SpriteRenderer sprite;
    public PlayerData pData;
    public Pathfinding pathing;

    private int speed;
    private int baseSpeed;

    private int facing =0;
    bool isWalking = false;
    private Vector2 direction = Vector2.zero;
    Vector2 finalVelocity = Vector2.zero;
    Vector2 stopDirection = Vector2.zero;

    public KeyCode[] MappedKeys = new KeyCode[] { KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.Space, KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.UpArrow, KeyCode.DownArrow };
    private float inputTimer=0;

    private int curPathVal = 0;
    
    void Start()
    {       

        baseSpeed = pData.speed;
        speed = baseSpeed;

        interactionSprites = interactionRange.GetComponentsInChildren<SpriteRenderer>();
    }

    void Update()
    {
        if (canControl)
        {
            if (CanMove())
            {
                Movement();

                WalkPath();

                DetectInteractionSpots();

                if (Input.GetMouseButtonDown(0))
                {
                    if (!isWalking)
                    {
                        MoveTowards();
                    }
                }
            }
        }
    }
    
    private void FixedUpdate()
    {
        rBody.velocity = finalVelocity;
    }

    private void DetectInteractionSpots()
    {
        gController.tilesInRange = new TData[interactionSprites.Length];
        for (int i = 0; i < interactionSprites.Length; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(interactionSprites[i].gameObject.transform.position, Vector2.down);
            if(hit.collider != null)
            {
                if(hit.collider.tag == "Tile")
                {
                    TData tile = hit.collider.GetComponent<TData>();
                    gController.tilesInRange[i] = tile;
                }
            }
        }
    }

    private void MoveTowards()
    {
        /*
        Vector2 mPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        TData target = gController.tManager.FindTileData(new Vector3Int((int)mPos.x, (int)mPos.y, 0));
        if (target.tileType != TileTypes.Ground && target.tileType != TileTypes.Road)
        {
            for (int i = 0; i < target.neighbors.Length; i++)
            {
                TData neighbor = gController.tManager.FindTileData(target.neighbors[i]);
                if (gController.tilesInRange.Contains(neighbor))
                {
                    return;
                }

                List<TileTypes> types = new List<TileTypes>();
                for(int j=0;j<pData.allowedTypes.Length;j++)
                {
                    types.Add(pData.allowedTypes[j]);
                }
                types.Add(target.tileType);
                pData.currentPath = pathing.FindPath(pData.currentLoc, neighbor, types.ToArray()).ToArray();
            }            
        }
        else
        {
            if(gController.tilesInRange.Contains(target))
            {
                return;
            }
            pData.currentPath = pathing.FindPath(pData.currentLoc, target).ToArray();
        }
        */
    }

    private void WalkPath()
    {
        if(pData.currentPath.Length < curPathVal)
            return;

        if (pData.currentPath.Length > 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, pData.currentPath[curPathVal], pData.speed * Time.deltaTime);

            if (transform.position == pData.currentPath[curPathVal])
            {
                curPathVal++;

                if (curPathVal == pData.currentPath.Length)
                {
                    curPathVal = 0;
                    isWalking = false;
                    pData.currentPath = new Vector3Int[0];
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

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Tile")
        {
            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Tile")
        {
            TData tile = collision.GetComponent<TData>();

            if (tile.tileType != TileTypes.Road)
            {
                if(tile.tileType == TileTypes.Ground)
                {
                    speed = baseSpeed / 2;
                    stopDirection = Vector2.zero;
                    pData.currentLoc = tile;
                    pData.slowed = true;
                }
                else
                {
                    //stop movement
                    stopDirection = direction;                    
                }
            }
            else
            {
                speed = baseSpeed;
                stopDirection = Vector2.zero;
                pData.currentLoc = tile;
                pData.slowed = false;
            }    
        }
    }
}
