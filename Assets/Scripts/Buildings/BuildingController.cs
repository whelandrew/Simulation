using UnityEngine;
using UnityEngine.UI;

public class BuildingController : MonoBehaviour
{

    public GameboardController gController;

    public GameObject BuildingUI;
    public Text BuildingName;
    public Text BuildingDetails;
    public Button assignButton;    

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
        assignButton.gameObject.SetActive(tile.owned);

        if (tile.owned)
        {
            BuildingDetails.text += "Owned By : " + tile.owner.FName + " " + tile.owner.LName;
        }
        else
        {
            BuildingDetails.text += "Unoccupied";
        }
    }

    public void AssignBuildingButton()
    {

    }
}
