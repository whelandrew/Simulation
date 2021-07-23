using UnityEngine;

public class PlayerInteractionDetector : MonoBehaviour
{
    ArrayManager aManager = new ArrayManager();
    public SimGameboardController gController;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Tile")
        {
            TData tile = collision.GetComponent<TData>();
            if (tile.canInterract)
            {
                TData[] inRange = new TData[] { tile };
                //Debug.Log("InteractionDector for " + tile.name);
                for(int i=0;i<tile.neighbors.Length;i++)
                {
                    TData neigh = gController.tManager.FindTileData(tile.neighbors[i]);
                    if(neigh != null && neigh.tileType == tile.tileType)
                    {
                        inRange = aManager.AddTData(inRange, neigh);
                    }
                }

                gController.SetTilesInRange(inRange);                
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Tile")
        {
            TData tile = collision.GetComponent<TData>();
            if(tile.canInterract)
            {
                gController.SetTilesInRange(new TData[0]);
            }
        }
    }
}
