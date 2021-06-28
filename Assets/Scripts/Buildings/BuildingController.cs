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

    public GameObject AssignmentPanel;
    public GameObject AssignmentPanelContent;
    public Button AssignmentButton;

    private TileTypes assignmentType = TileTypes.None;
    private List<VillagerData> availableVillagers = new List<VillagerData>();
    private TData selectedBuilding;


    private void Start()
    {
        AssignmentPanelContent.SetActive(false);
        BuildingUI.SetActive(false);
    }
    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
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
                if (hit.collider.GetComponent<TData>().isBuilding)
                {
                    BuildingUI.SetActive(true);
                    AssignmentPanelContent.SetActive(false);
                    BuildingInfo(hit.collider.GetComponent<TData>());
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
            else
            {

            }
        }
    }

    public void CreateAssignmentButtonFor()
    {
        AssignmentPanelContent.SetActive(true);

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
        }

        selectedBuilding.owned = true;
        selectedBuilding.owner = vData;
        List<TData> nTiles = gController.tManager.GetClosestNeighborsOfType(selectedBuilding, selectedBuilding.tileType);
        for (int i = 0; i < nTiles.Count; i++)
        {
            nTiles[i].owned = true;
            nTiles[i].owner = vData;
        }


        BuildingInfo(selectedBuilding);
    }
}
