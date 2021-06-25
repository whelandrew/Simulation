using UnityEngine;
using UnityEngine.UI;

//pipeline for controllers to communicate
public class GameboardController : MonoBehaviour
{
    public TileManager tManager;
    public VillagerController vController;
    public BuildingController bController;

    public Text populationTotal;
    public Text buildingTotal;

    public int populationCount;
    public int buildingCount;

    private void Start()
    {
        populationTotal.text = "Population : 0";
        buildingTotal.text = "Buildings : 0";

        //remove after testing ends
        vController.CreateVillager();
    }

    private void Update()
    {        
        if (populationCount != vController.villagerTotal)
        {
            populationCount = vController.villagerTotal;
            populationTotal.text = "Population : " + populationCount;
        }
        
        if (buildingCount != tManager.buildingTotal)
        {
            buildingCount = tManager.buildingTotal;
            buildingTotal.text = "Buildings : " + buildingCount;
        }
    }
}
