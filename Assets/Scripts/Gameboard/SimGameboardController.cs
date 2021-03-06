using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class SimGameboardController : MonoBehaviour
{
    public Singleton singleton;
    public TileManager tManager;
    public VillagerController vController;
    public BuildingController bController;
    public SimPlayerController pController;
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

    public TData[] tilesInRange;

    public GameObject SpawnPointsCache;
    private TData[] spawnPoints;

    ChoicesTypes choicesResult = ChoicesTypes.None;
    private void Start()
    {
        populationTotal.text = "Population : 0";
        buildingTotal.text = "Buildings : 0";

        FixSpawnPoints();

        StartCoroutine(AdvanceTime());

        //remove after testing ends
        //vController.CreateVillager();
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

        homelessTotal.text = "Homeless : " + (vController.villagerTotal - vController.homeTotal);

        Schedule();
    }

    private void Schedule()
    {
        //morning
        if (timeOfDay > 0 && timeOfDay < 9)
        {
            TOD = Times.Morning;
            vController.timeChange = timeOfDay == 0;
        }

        //afternoon
        else if (timeOfDay > 9 && timeOfDay < 19)
        {
            TOD = Times.Noon;
            vController.timeChange = timeOfDay == 10;
        }
        else if (timeOfDay > 19 && timeOfDay < 29)
        {
            TOD = Times.Night;
            vController.timeChange = timeOfDay == 20;
        }
        else
        if (timeOfDay > 29)
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

    private void FixSpawnPoints()
    {
        int count = 0;
        spawnPoints = new TData[SpawnPointsCache.GetComponentsInChildren<Transform>().Length - 1];
        foreach (BoxCollider2D i in SpawnPointsCache.GetComponentsInChildren<BoxCollider2D>())
        {
            Vector2 orgPos = i.transform.position;
            TData spawnTile = tManager.FindTileData(new Vector3Int((int)Mathf.Round(orgPos.x), (int)Mathf.Round(orgPos.y), 0));
            spawnPoints[count] = spawnTile;
            count++;
        }
    }
    public TData GetSpawnPoint()
    {
        return spawnPoints[Random.Range(0, spawnPoints.Length - 1)];
    }

    public void SetTilesInRange(TData[] inRange)
    {
        tilesInRange = inRange;
    }
    public void ChoiceOnClick(int val)
    {
        Debug.Log("ChoiceOnClick");
        switch (val)
        {
            case 0: choicesResult = ChoicesTypes.Positive; break;
            case 1: choicesResult = ChoicesTypes.Neutral; break;
            case 2: choicesResult = ChoicesTypes.Mean; break;
            case 3: choicesResult = ChoicesTypes.Flirty; break;
            default: choicesResult = ChoicesTypes.None; break;
        }
    }

    public VillagerData[] GetAllVillagers()
    {                
        return vController.GetActiveVillagers();
    }
}
