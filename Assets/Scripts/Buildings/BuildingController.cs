using UnityEngine;
using UnityEngine.UI;
public class BuildingController : MonoBehaviour
{
    public SimGameboardController gController;

    public GameObject BuildingUI;
    public Text BuildingName;
    public Text BuildingDetails;
    public GameObject assignButton;
    public Image BuildingImage;

    public GameObject AssignmentPanel;
    public GameObject AssignmentPanelContent;
    public Button AssignmentButton;

    private TileTypes assignmentType = TileTypes.None;
    private VillagerData[] availableVillagers;
    private TData selectedBuilding;

    public int assignedBuildingTotal;
        
    private void Start()
    {
        assignButton.SetActive(false);
        AssignmentPanelContent.SetActive(false);
        BuildingUI.SetActive(false);        
    }
    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if (!gController.UIOn)
            {                
                GetBuildingDetails();
            }            
        }

        InteractionHighlightCheck();
    }

    private void InteractionHighlightCheck()
    {
        foreach (TData t in gController.tManager.GetAllBuildings())
        {
            SpriteRenderer s = t.GetComponent<SpriteRenderer>();
            s.color = new Color(1,1,1,1);
            if (gController.tilesInRange.Length > 0)
            {
                for (int i = 0; i < gController.tilesInRange.Length; i++)
                {
                    if (gController.tilesInRange[i] == t)
                    {
                        s.color = Color.yellow;
                    }
                }
            }
        }
    }

    private void GetBuildingDetails()
    {        
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), -Vector2.up);
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.tag == "Tile")
            {
                TData tile = hit.collider.GetComponent<TData>();
                for (int i = 0; i < gController.tilesInRange.Length; i++)
                {
                    if (gController.tilesInRange[i] == tile)
                    {
                        gController.SetUIOn(true);
                        BuildingUI.SetActive(true);
                        AssignmentPanelContent.SetActive(false);
                        BuildingInfo(hit.collider.GetComponent<TData>());
                    }
                }
            }
        }
    }

    private void BuildingInfo(TData tile)
    {
        BuildingUI.transform.position = tile.pos;
        BuildingName.text = tile.tileType.ToString();
        BuildingDetails.text = "";
        selectedBuilding = tile;
        BuildingImage.sprite = gController.tManager.GetTileSprite(tile.tileType);

        if (tile.owned)
        {
            BuildingDetails.text += "Owned By : " + tile.owner.FName + " " + tile.owner.LName;
        }
        else
        {
            BuildingDetails.text += "Unoccupied";
        }

        assignButton.SetActive(tile.canInterract);
        if (tile.canInterract)
        {
            CreateAssignmentLists(tile);
        }
    }

    private void CreateAssignmentLists(TData tile)
    {
        VillagerData[] vData = gController.vController.GetActiveVillagers();        
        availableVillagers = new VillagerData[vData.Length];

        for (int i = 0; i < vData.Length; i++)
        {
            if(tile.tileType == TileTypes.House)
            {
                if(!tile.owned)
                {
                    if (vData[i] != null)
                    {
                        if (!vData[i].hasHome)
                        {
                            if (!assignButton.activeSelf)
                            {
                                assignButton.SetActive(true);
                            }

                            availableVillagers[i] = vData[i];
                            assignmentType = TileTypes.House;
                        }
                    }
                }
            }
            else if(tile.tileType == TileTypes.Workshop)
            {
                if(!tile.owned)
                {
                    if(!vData[i].hasJob && vData[i].hasHome)
                    {
                        if (!assignButton.activeSelf)
                        {
                            assignButton.SetActive(true);
                        }
                        availableVillagers[i] = vData[i];
                        assignmentType = TileTypes.Workshop;
                    }
                }
            }
            else
            {
                assignButton.SetActive(false);
            }
        }
    }

    public void CreateAssignmentButtonFor()
    {
        AssignmentPanelContent.SetActive(true);
        foreach (Button i in AssignmentPanelContent.GetComponentsInChildren<Button>())
        {
            Destroy(i.gameObject);
        }

        for (int i = 0; i < availableVillagers.Length; i++)
        {
            VillagerData vData = availableVillagers[i];

            Button newButton = Instantiate(AssignmentButton);
            Text bText = newButton.GetComponentInChildren<Text>();
            bText.text = vData.FName + " " + vData.LName;

            newButton.gameObject.name = bText.text;
            newButton.transform.SetParent(AssignmentPanelContent.transform, false);

            newButton.onClick.AddListener(delegate { AssignBuildingTo(vData.id); });
        }
    }

    public void AssignBuildingTo(string villagerID)
    {
        Debug.Log("AssignBuilgingTo");
        VillagerData vData = gController.vController.FindVillager(villagerID);
        AssignmentPanel.SetActive(false);

        if (selectedBuilding.tileType == TileTypes.House)
        {
            vData.hasHome = true;
            vData.homeLoc = selectedBuilding.pos;
            vData.homeID = selectedBuilding.id;
            vData.atLocation = false;

            gController.vController.homeTotal++;
        }
        else
        if (selectedBuilding.tileType == TileTypes.Workshop)
        {
            vData.hasJob = true;
            vData.job = JobType.Carpenter;
            vData.jobLoc = selectedBuilding.pos;
            vData.jobID = selectedBuilding.id;            

            assignedBuildingTotal++;
        }

        vData.goingTo = selectedBuilding.tileType;
        vData.target = selectedBuilding;
        vData.atLocation = false;
        
        TData[] nTiles = gController.tManager.GetClosestNeighborsOfType(selectedBuilding, selectedBuilding.tileType);
        for (int i = 0; i < nTiles.Length; i++)
        {
            nTiles[i].owned = true;
            nTiles[i].owner = vData;
        }

        selectedBuilding.owned = true;
        selectedBuilding.owner = vData;
        assignButton.SetActive(false);
        BuildingInfo(selectedBuilding);
    }
}
