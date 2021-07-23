using UnityEngine;

public class PlayerFootCollider : MonoBehaviour
{
    public PlayerData pData;
    public SimPlayerController pController;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Tile")
        {
            TData tile = collision.GetComponent<TData>();
            if (tile != pData.curLoc)
            {
                pData.curLoc = tile;
                pData.curSimLoc = tile.pos;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Tile")
        {
            TData tile = collision.GetComponent<TData>();
            pData.curLoc = tile;
            pData.curSimLoc = tile.pos;

            if (tile.tileType != TileTypes.Road)
            {
                if (tile.tileType == TileTypes.Ground)
                {
                    pController.speed = pData.speed / 2;
                    pController.stopDirection = Vector2.zero;                    
                    pData.slowed = true;
                }
                else
                {
                    //stop movement
                    pController.stopDirection = pController.direction;
                }
            }
            else
            {
                pController.speed = pData.speed;
                pController.stopDirection = Vector2.zero;
                pData.curLoc = tile;
                pData.slowed = false;
            }
        }
    }    
}
