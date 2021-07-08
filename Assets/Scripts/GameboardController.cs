using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//pipeline for controllers to communicate
public class GameboardController : MonoBehaviour
{
    public TileManager tManager;
    public VillagerController vController;
    public BuildingController bController;
    public playerController pController;
    public EnemyController eController;
    public Pathfinding pathing;

    public Text populationTotal;
    public Text buildingTotal;
    public Text time;
    public Text homelessTotal;

    public int populationCount;
    public int homelessCount;
    public int homeCount;
    public int buildingCount;

    public int timeOfDay;
    public Times TOD;

    public bool UIOn;

    //public List<TData> tilesInRange = new List<TData>();
    public TData[] tilesInRange;

    public GameObject[] spawnPoints;

    private void Start()
    {
        populationTotal.text = "Population : 0";
        buildingTotal.text = "Buildings : 0";

        StartCoroutine(AdvanceTime());

        //remove after testing ends
        vController.CreateVillager();
        pController.canControl = true;
        //eController.CreateEnemy();
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

        homelessTotal.text = "Homeless : " + (vController.villagerTotal-vController.homeTotal);        

        Schedule();
    }

    private void Schedule()
    {
        //morning
        if(timeOfDay > 0 && timeOfDay<9)
        {
            TOD = Times.Morning;
            vController.timeChange = timeOfDay == 0;
        }
        
        //afternoon
        else if(timeOfDay >9&&timeOfDay<19)
        {
            TOD = Times.Noon;
            vController.timeChange = timeOfDay==10;
        }
        else if(timeOfDay >19 && timeOfDay<29)
        {
            TOD = Times.Night;
            vController.timeChange = timeOfDay==20;
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

    public void SetUIOn(bool isOn)
    {
        UIOn = isOn;
    }

    public Vector2 GetSpawnPoint()
    {
        return spawnPoints[Random.Range(0, spawnPoints.Length - 1)].transform.position;
    }

}
