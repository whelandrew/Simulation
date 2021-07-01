using System.Collections.Generic;
using UnityEngine;

public class VillagerController : MonoBehaviour
{
    public GameboardController gController;

    public GameObject villagerCache;
    private List<GameObject> villagers = new List<GameObject>();

    public GameObject VillagerSpawnPoints;
    private BoxCollider2D[] spawnPoints;

    public int villagerTotal;
    public int homeTotal;

    public bool timeChange;

    public Sprite speechBubbleHome;
    public Sprite speechBubbleWork;
    public Sprite speechBubbleSleep;
    public Sprite speechBubbleHomeless;
    public Sprite speechBubbleActions;
    public Sprite speechBubbleIdle;

    private void Awake()
    {
        foreach(VillagerData i in villagerCache.GetComponentsInChildren<VillagerData>())
        {
            villagers.Add(i.gameObject);
        }

        spawnPoints = VillagerSpawnPoints.GetComponentsInChildren<BoxCollider2D>();
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            VillagerInfo();
        }
    }

    private void VillagerInfo()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), -Vector2.up);
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.tag == "Villager")
            {                
                hit.collider.GetComponent<VillagerBehavior>().ShowStats();             
            }
        }
    }

    public void Reset(int val)
    {
        VillagerData vData = villagers[val].GetComponent<VillagerData>();
        SpriteRenderer sr = villagers[val].GetComponent<SpriteRenderer>();
        BoxCollider2D bCollider = villagers[val].GetComponent<BoxCollider2D>();

        vData.isActive = false;
        vData.FName = "";
        vData.LName = "";
        vData.id = "";

        sr.enabled = false;

        bCollider.enabled = false;

        villagers[val].name = "Unused Villager";
    }

    Vector2 GetSpawnPoint()
    {        
        return spawnPoints[Random.Range(0, spawnPoints.Length - 1)].transform.position;
    }

    public void CreateVillager()
    {
        for(int i = 0; i < villagers.Count; i++) 
        {
            VillagerData vData = villagers[i].GetComponent<VillagerData>();
            SpriteRenderer sr = villagers[i].GetComponent<SpriteRenderer>();
            VillagerBehavior vB = villagers[i].GetComponent<VillagerBehavior>();
            if (!vData.isActive)
            {
                VillagerNames vNames = new VillagerNames();

                vData.isActive = true;
                vData.Gender = 0;

                vData.speed = 5;

                vData.FName = vNames.GetRandomFirstName(vData.Gender);
                vData.LName = vNames.GetRandomLastName(vData.Gender);

                vData.id = i + vData.FName + vData.LName;

                sr.enabled = true;

                villagers[i].name = vData.id;
                villagers[i].transform.position = GetSpawnPoint();

                vData.pos = new Vector2Int((int)villagers[i].transform.position.x, (int)villagers[i].transform.position.y);                                
                vData.currentLocation = gController.tManager.FindTileData(new Vector3Int((int)villagers[i].transform.position.x, (int)villagers[i].transform.position.y, 0));
                vData.job = JobType.None;
                vData.hasJob = false;
                villagerTotal++;

                vB.ActivateVillager();
                break;
            }
        }
    }

    public List<VillagerData> GetActiveVillagers()
    {
        List<VillagerData> actives = new List<VillagerData>();
        for(int i=0;i<villagers.Count;i++)
        {
            VillagerData vData = villagers[i].GetComponent<VillagerData>();
            if(vData.isActive)
            {
                actives.Add(vData);
            }
        }

        return actives;
    }
}
