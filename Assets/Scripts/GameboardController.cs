using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//pipeline for controllers to communicate
public class GameboardController : MonoBehaviour
{
    public TileManager tManager;
    public VillagerController vController;
    public BuildingController bController;

    public Text populationTotal;
    public Text buildingTotal;
    public Text time;

    public int populationCount;
    public int buildingCount;

    public int timeOfDay;
    public Times TOD;

    private void Start()
    {
        populationTotal.text = "Population : 0";
        buildingTotal.text = "Buildings : 0";

        StartCoroutine(AdvanceTime());

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

        

        Schedule();
    }

    private void Schedule()
    {
        //morning
        if(timeOfDay > 0 && timeOfDay<9)
        {
            TOD = Times.Morning;
        }
        //afternoon
        else if(timeOfDay >9&&timeOfDay<19)
        {
            TOD = Times.Noon;
        }
        else if(timeOfDay >19 && timeOfDay<29)
        {
            TOD = Times.Night;
        }
        else
        if(timeOfDay > 29)
        {
            timeOfDay = 0;
        }

        time.text = "Time : " + timeOfDay + " " + TOD.ToString(); ;
    }

    private IEnumerator AdvanceTime()
    {
        timeOfDay++;
        yield return new WaitForSeconds(1);
        StartCoroutine(AdvanceTime());
    }
}
