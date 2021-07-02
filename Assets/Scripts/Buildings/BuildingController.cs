using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class BuildingController : MonoBehaviour
{
    public GameboardController gController;

    public GameObject BuildingUI;
    public Text BuildingName;
    public Text BuildingDetails;
    public GameObject assignButton;
    public Image BuildingImage;

    public GameObject AssignmentPanel;
    public GameObject AssignmentPanelContent;
    public Button AssignmentButton;

    private TileTypes assignmentType = TileTypes.None;
    private List<VillagerData> availableVillagers = new List<VillagerData>();
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

            GetBuildingDetails();
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
                if (gController.tilesInRange.Contains(tile))
                {
                    //if (tile.isBuilding)
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

        CreateAssignmentLists(tile);
    }

    private void CreateAssignmentLists(TData tile)
    {
        List<VillagerData> vData = gController.vController.GetActiveVillagers();
        availableVillagers = new List<VillagerData>();

        for (int i = 0; i < vData.Count; i++)
        {
            if(tile.tileType == TileTypes.House)
            {
                if(!tile.owned)
                {
                    if(!vData[i].hasHome)
                    {
                        if (!assignButton.activeSelf)
                        {
                            assignButton.SetActive(true);
                        }
                        availableVillagers.Add(vData[i]);
                        assignmentType = TileTypes.House;
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
                        availableVillagers.Add(vData[i]);
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
            GameObject.Destroy(i.gameObject);
        }

        for (int i = 0; i < availableVillagers.Count; i++)
        {
            VillagerData vData = availableVillagers[i];

            Button newButton = Instantiate(AssignmentButton) as Button;
            Text bText = newButton.GetComponentInChildren<Text>();
            bText.text = vData.FName + " " + vData.LName;

            newButton.gameObject.name = bText.text;
            newButton.transform.parent = AssignmentPanelContent.transform;

            newButton.onClick.AddListener(delegate { AssignBuildingTo(vData); });            
        }
    }

    public void AssignBuildingTo(VillagerData vData)
    {
        AssignmentPanel.SetActive(false);

        if (selectedBuilding.tileType == TileTypes.House)
        {
            vData.hasHome = true;
            vData.homeLoc = selectedBuilding.pos;
            vData.homeID = selectedBuilding.id;
            vData.goingTo = TileTypes.House;
            vData.target = selectedBuilding;
            vData.atLocation = false;

            gController.vController.homeTotal++;
        }

        if (selectedBuilding.tileType == TileTypes.Workshop)
        {
            vData.hasJob = true;
            vData.job = JobType.Carpenter;
            vData.jobLoc = selectedBuilding.pos;
            vData.jobID = selectedBuilding.id;
            vData.goingTo = TileTypes.Workshop;
            vData.target = selectedBuilding;
            vData.atLocation = false;

            assignedBuildingTotal++;
        }

        selectedBuilding.owned = true;
        selectedBuilding.owner = vData;
        List<TData> nTiles = gController.tManager.GetClosestNeighborsOfType(selectedBuilding, selectedBuilding.tileType);
        for (int i = 0; i < nTiles.Count; i++)
        {
            nTiles[i].owned = true;
            nTiles[i].owner = vData;
        }

        assignButton.SetActive(false);

        BuildingInfo(selectedBuilding);
    }
}
