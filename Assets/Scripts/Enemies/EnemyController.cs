using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameboardController gController;
    public GameObject enemyCache;
    GameObject[] enemies = new GameObject[0];

    private void Awake()
    {
        int count = 0;
        enemies = new GameObject[enemyCache.GetComponentsInChildren<EnemyData>().Length];        
        foreach(EnemyData i in enemyCache.GetComponentsInChildren<EnemyData>())
        {
            enemies[count] = i.gameObject;
            enemies[count].SetActive(false);
            count++;
        }
    }
    
    public void Reset(int val)
    {
        EnemyData eData = enemies[val].GetComponent<EnemyData>();
        SpriteRenderer sr = enemies[val].GetComponent<SpriteRenderer>();
        BoxCollider2D bCollider = enemies[val].GetComponent<BoxCollider2D>();

        eData.isActive = false;
        eData.fName = "";
        eData.lName = "";
        eData.id = "";

        sr.enabled = false;

        bCollider.enabled = false;
        eData.slowed = false;


        enemies[val].name = "Unused Villager";
        enemies[val].SetActive(false);
    }

    public void CreateEnemy()
    {
        for( int i=0;i<enemies.Length;i++)
        {
            EnemyData eData = enemies[i].GetComponent<EnemyData>();            
            if(!eData.isActive)
            {
                EnemyBehavior eBehavior = enemies[i].GetComponent<EnemyBehavior>();

                eData.id = eData.fName + eData.lName + i;

                enemies[i].name = eData.id;
                enemies[i].transform.position = gController.GetSpawnPoint().transform.position;

                eData.pos = new Vector2Int((int)transform.position.x, (int)transform.position.x);

                eData.speed = 15;

                eData.gender = 0;

                eData.slowed = false;

                eData.currentLoc = gController.GetSpawnPoint();

                eData.allowedTypes = new TileTypes[] { TileTypes.Road, TileTypes.Ground };

                eData.pData = gController.pController.pData;

                eData.HP = 1;
                eData.AC = 1;
                eData.Agility = 2;

                eBehavior.ActivateEnemy();
            }
        }
    }
}
