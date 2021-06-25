using UnityEngine;

public class VillagerBehavior : MonoBehaviour
{
    public Pathfinding pathing;
    public TileManager tManager;

    VillagerData vData;

    private void Start()
    {
        vData = GetComponent<VillagerData>();
    }

    public void ActivateVillager()
    {
        vData.isActive = true;
        GoToTownCenter();
    }

    private void GoToTownCenter()
    {
        //start - need TData of where villager is standing
        if(vData.currentLocation == null)
        {
            FindCurrentLocation();
        }
        //Target - need TData of location (is there an entrance to use?)        
        //
        pathing.FindPath(vData.currentLocation, tManager.GetOneTileOfType(TileTypes.TownCenter));
    }

    void FindCurrentLocation()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up);
        if (hit.collider != null)
        {
            if(hit.collider.tag == "Tile")
            {
                vData.currentLocation = hit.collider.GetComponent<TData>();
            }
        }
    }
}
