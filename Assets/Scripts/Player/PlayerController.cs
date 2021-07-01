using UnityEngine;

public class playerController : MonoBehaviour
{
    public bool canControl;

    public Rigidbody2D rBody;
    public BoxCollider2D bCollider;
    public SpriteRenderer sprite;

    private int speed;
    public int baseSpeed;

    bool isWalking = false;
    private Vector2 direction = Vector2.zero;
    Vector2 finalVelocity = Vector2.zero;

    public KeyCode[] MappedKeys = new KeyCode[] { KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.Space, KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.UpArrow, KeyCode.DownArrow };
    private float inputTimer;

    bool stopMovement;

    void Start()
    {
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
        }

    }

    private void FixedUpdate()
    {
        rBody.velocity = finalVelocity;
    }

    private bool CanMove()
    {
        inputTimer += 1f * Time.deltaTime;

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
        }
        if (Input.GetKey(KeyCode.D))
        {
            direction = Vector2.right;
        }
        if (Input.GetKey(KeyCode.W))
        {
            direction = Vector2.up;
        }
        if (Input.GetKey(KeyCode.S))
        {
            direction = Vector2.down;
        }

        finalVelocity = direction * speed;

        isWalking = true;
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
                }
                else
                {
                    //stop movement
                    speed = 0;
                    isWalking = false;
                }
            }
            else
            {
                speed = baseSpeed;
            }    
        }
    }
}
