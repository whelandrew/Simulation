using UnityEngine;

public class PlayerFootCollider : MonoBehaviour
{
    public PlayerData pData;
    public playerController pController;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Tile")
        {
            TData tile = collision.GetComponent<TData>();

            if (tile.tileType != TileTypes.Road)
            {
                if (tile.tileType == TileTypes.Ground)
                {
                    pController.speed = pData.speed / 2;
                    pController.stopDirection = Vector2.zero;
                    pData.currentLoc = tile;
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
                pData.currentLoc = tile;
                pData.slowed = false;
            }
        }
    }    
}
