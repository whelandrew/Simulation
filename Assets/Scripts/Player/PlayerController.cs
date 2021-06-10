using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rBody;

    public bool canControl = true;

    public Vector2 finalVelocity = Vector2.zero;
    private Vector2 direction = Vector2.zero;
    private Vector2 moveDir = Vector2.zero;
    private int facing = 1;
    private bool isWalking = false;

    public float moveSpeed = 7f;

    private void Start()
    {
        rBody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if(canControl)
        {
            Walking();
        }
    }

    private void FixedUpdate()
    {
        rBody.velocity = finalVelocity;
    }

    private void Walking()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            //up
            moveDir = Vector2.up;
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            //down
            moveDir = Vector2.down;
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            //left
            moveDir = Vector2.left;
            facing = -1;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            //right
            moveDir = Vector2.right;
            facing = 1;
        }       

        if(!Input.anyKey)
        {
            moveDir = Vector2.zero;
        }    
        
        //play movement animations
        finalVelocity = moveDir * moveSpeed;        
    }
}
