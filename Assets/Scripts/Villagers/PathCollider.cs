using UnityEngine;

public class PathCollider : MonoBehaviour
{
    public VillagerData vData;
    public VillagerBehavior vBehavior;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (vData.isActive)
        {
            if (collision.tag == "Tile")
            {
                TData tile = collision.GetComponent<TData>();
                if (tile != vData.currentLocation)
                {
                    vData.currentLocation = collision.GetComponent<TData>();
                }

                if(tile.tileType == vData.goingTo)
                {
                    vData.currentPath = new Vector3Int[0];
                    vData.isMoving = false;
                    vBehavior.atLocation = true;
                }

                if (tile.tileType == TileTypes.House)
                {
                    if (tile.owner == vData)
                    {
                        vBehavior.AtHome();
                    }
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (vData.isActive)
        {
            
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (vData.isActive)
        {
            if (collision.tag == "Tile")
            {
                TData tile = collision.GetComponent<TData>();
                if (tile != vData.currentLocation)
                {
                    vData.currentLocation = collision.GetComponent<TData>();
                }
            }
        }
    }
}
