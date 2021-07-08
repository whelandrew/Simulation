using System.Collections.Generic;
using UnityEngine;

public class VillagerController : MonoBehaviour
{
    public GameboardController gController;

    public GameObject villagerCache;
    private GameObject[] villagers = new GameObject[100];

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
        int count = 0;
        foreach(VillagerData i in villagerCache.GetComponentsInChildren<VillagerData>())
        {
            villagers[count] = i.gameObject;
            foreach(Transform j in i.transform)
            {
                j.gameObject.SetActive(false);
            }
            count++;
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

        foreach(Transform i in villagers[val].transform)
        {
            i.gameObject.SetActive(false);
        }

        vData.isActive = false;
        vData.FName = "";
        vData.LName = "";
        vData.id = "";

        sr.enabled = false;

        bCollider.enabled = false;

        villagers[val].name = "Unused Villager";
    }

    public void CreateVillager()
    {
        for(int i = 0; i < villagers.Length; i++) 
        {
            VillagerData vData = villagers[i].GetComponent<VillagerData>();
            
            if (!vData.isActive)
            {
                SpriteRenderer sr = villagers[i].GetComponent<SpriteRenderer>();
                VillagerBehavior vB = villagers[i].GetComponent<VillagerBehavior>();
                Transform[] children = villagers[i].GetComponentsInChildren<Transform>();
                VillagerNames vNames = new VillagerNames();

                for(int j=0;j<children.Length;j++)
                {
                    children[j].gameObject.SetActive(true);
                }

                vData.isActive = true;
                vData.Gender = 0;

                vData.speed = 5;

                vData.FName = vNames.GetRandomFirstName(vData.Gender);
                vData.LName = vNames.GetRandomLastName(vData.Gender);

                vData.id = i + vData.FName + vData.LName;

                sr.enabled = true;

                villagers[i].name = vData.id;
                villagers[i].transform.position = gController.GetSpawnPoint();

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

    public VillagerData[] GetActiveVillagers()
    {
        VillagerData[] actives = new VillagerData[villagers.Length];
        for(int i=0;i<villagers.Length;i++)
        {
            VillagerData vData = villagers[i].GetComponent<VillagerData>();
            if(vData.isActive)
            {
                actives[i] = vData;
            }
        }

        return actives;
    }
}
