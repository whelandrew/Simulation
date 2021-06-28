using UnityEngine;

public class VillagerBehavior : MonoBehaviour
{
    public GameboardController gController;
    public Pathfinding pathing;
    public TileManager tManager;
    public BoxCollider2D bCollider;

    public VillagerData vData;
    private int pathVal = 0;

    public bool atLocation;
    private bool atHome;
    private bool atWork;

    private void Update()
    {
        if (vData.isActive)
        {            
            Behaviors();
        }
    }

    private void Behaviors()
    {
        if (vData.hasHome)
        {
            //get current time from gameboard
            switch (gController.TOD)
            {
                case Times.Morning:
                    MorningActions();
                    break;
                case Times.Noon:
                    NoonActions();
                    break;
                case Times.Night:
                    NightActions();
                    break;
            }
        }
        else
        {
            GoToTownCenter();
        }

        //walk path
        if (vData.currentPath.Length > 0)
        {
            WalkPath();
        }
    }

    private void MorningActions()
    {
        GoHome();   
    }

    private void NoonActions()
    {
        
    }

    private void NightActions()
    {
        
    }

    private void WalkPath()
    {
        if (vData.currentPath.Length > 0)
        {            
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

    private void Reset()
    {
        vData.currentPath = new Vector3Int[0];
    }

    private void GoToTownCenter()
    {
        if (!atLocation)
        {
            if (!vData.isMoving)
            {
                vData.goingTo = TileTypes.TownCenter;
                vData.isMoving = true;
                vData.currentPath = pathing.FindPath(vData.currentLocation, tManager.GetOneTileOfType(TileTypes.TownCenter)).ToArray();
            }
        }
    }

    private void GoHome()
    {
        if (!atLocation)
        {
            if (!vData.isMoving)
            {
                vData.goingTo = TileTypes.House;
                vData.isMoving = true;
                vData.currentPath = pathing.FindPath(vData.currentLocation, tManager.FindTileData(vData.homeLoc)).ToArray();
            }
        }
    }

    private void GoToWork()
    {
        if(!atWork)
        {
            if (!vData.isMoving)
            {
                vData.isMoving = true;
            }
            //if (vData.currentPath.Length < 1)
            //{
               //vData.currentPath = pathing.FindPath(vData.currentLocation, tManager.FindTileData(vData.jobLoc)).ToArray();
            //}
        }
    }

    public void AtHome()
    {
        atHome = true;
        atWork = false;
    }

    public void AtWork()
    {
        atHome = false;
        atWork = true;        
    }

    public void AtDestination()
    {
    }
}
