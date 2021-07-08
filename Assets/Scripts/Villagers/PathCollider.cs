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
                    vData.atLocation = false;
                }

                if (tile == vData.target)
                {
                    vBehavior.StopWalking();
                    vData.atLocation = true;
                    switch(tile.tileType)
                    {
                        case TileTypes.House:
                            if(tile.owner==vData)
                            {
                                vBehavior.AtHome();
                            }
                            break;
                        case TileTypes.Workshop:
                            if(tile.owner == vData)
                            {
                                vBehavior.AtWork();
                            }
                            break;
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
                if(vData.atLocation && tile == vData.target)
                {
                    vData.atLocation = false;
                }
            }
        }
    }
}
