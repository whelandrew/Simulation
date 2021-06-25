using UnityEngine;

public class VillagerBehavior : MonoBehaviour
{
    public Pathfinding pathing;
    public TileManager tManager;
    public BoxCollider2D bCollider;

    public VillagerData vData;
    private int pathVal = 0;

    private void Update()
    {
        if (vData.isActive)
        {
            WalkPath();
            Behaviors();
        }
    }

    private void Behaviors()
    {        
        if (vData.hasHome)
        {
            //go home
            //go to work
        }
        else
        {
            if (!vData.isMoving)
            {
                //if homeless
                GoToTownCenter();
            }
        }
    }

    private void WalkPath()
    {
        if (vData.currentPath.Length > 0)
        {
            vData.isMoving = true;
            Debug.Log(vData.currentPath.Length);
            Debug.Log(pathVal);
            transform.position = Vector3.MoveTowards(transform.position, vData.currentPath[pathVal], vData.speed * Time.deltaTime);
            
            if (transform.position == vData.currentPath[pathVal])
            {
                pathVal++;

                if (pathVal == vData.currentPath.Length)
                {
                    pathVal = 0;
                    vData.isMoving = false;
                    vData.currentPath = new Vector3Int[0];
                }
            }
        }
    }

    public void ActivateVillager()
    {
        vData.isActive = true;
        bCollider.enabled = true;
    }

    private void GoToTownCenter()
    {  
        vData.currentPath = pathing.FindPath(vData.currentLocation, tManager.GetOneTileOfType(TileTypes.TownCenter)).ToArray();                
    }
}
