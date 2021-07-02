using UnityEngine.UI;
using UnityEngine;

public class VillagerBehavior : MonoBehaviour
{
    public GameboardController gController;
    //public Pathfinding pathing;
    public TileManager tManager;
    public BoxCollider2D bCollider;
    public SpriteRenderer speechBubble;   

    public VillagerData vData;
    private int pathVal = 0;

    public GameObject villagerUI;
    public Text villagerName;
    public Text villagerStats;

    private void Update()
    {
        if (vData.isActive)
        {            
            Behaviors();
        }

        if(Input.GetMouseButtonDown(0))
        {
            if (villagerUI.activeSelf)
            {
                villagerUI.SetActive(false);
            }
        }
    }

    public void ShowStats()
    {
        villagerUI.SetActive(true);

        villagerName.text = vData.FName + " " + vData.LName;

        villagerStats.text = vData.Gender + "\n";
        villagerStats.text += vData.job + "\n";
        villagerStats.text += vData.homeLoc + "\n";
        villagerStats.text += vData.job + " " + vData.jobLoc + "\n";
        villagerStats.text += vData.goingTo + "\n";
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
            //GoToTownCenter
            SpeechBubble(gController.vController.speechBubbleHomeless);
            GoTo(TileTypes.TownCenter);
        }

        //walk path
        if (vData.currentPath.Length > 0)
        {
            WalkPath();
        }
    }

    private void MorningActions()
    {
        //GoHome
        if (vData.hasHome)
        {
            SpeechBubble(gController.vController.speechBubbleHome);
            GoTo(TileTypes.House);
        }
        else
        {
            SpeechBubble(gController.vController.speechBubbleIdle);
            GoTo(TileTypes.TownCenter);
        }
    }

    private void NoonActions()
    {
        if (vData.hasJob)
        {
            SpeechBubble(gController.vController.speechBubbleWork);
            GoTo(TileTypes.Workshop);
        }
        else
        {
            SpeechBubble(gController.vController.speechBubbleIdle);
            GoTo(TileTypes.TownCenter);
        }
    }

    private void NightActions()
    {
        if (vData.hasHome)
        {
            SpeechBubble(gController.vController.speechBubbleHome);
            GoTo(TileTypes.House);
        }
        else
        {
            GoTo(TileTypes.TownCenter);
        }
    }

    private void WalkPath()
    {
        if (vData.currentPath.Length < pathVal)
            return;

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

    private void GoTo(TileTypes typeLocation)
    {
        if (!vData.isMoving && !vData.atLocation && gController.vController.timeChange)
        {
            gController.vController.timeChange = false;
            TData targetTile = tManager.GetOneTileOfType(typeLocation);
            vData.target = targetTile;
            vData.goingTo = typeLocation;
            vData.isMoving = true;
            vData.currentPath = new Vector3Int[0];
            pathVal = 0;
            vData.currentPath = gController.pathing.FindPath(vData.currentLocation, targetTile).ToArray();
        }
    }

    public void AtHome()
    {
        vData.atHome = true;
        vData.atWork = false;
    }

    public void AtWork()
    {
        vData.atHome = false;
        vData.atWork = true;        
    }

    private void SpeechBubble(Sprite newSprite)
    {
        if (!speechBubble.gameObject.activeSelf)
        {
            speechBubble.gameObject.SetActive(true);
        }
        else
        {
            speechBubble.sprite = newSprite;
        }
    }
}
