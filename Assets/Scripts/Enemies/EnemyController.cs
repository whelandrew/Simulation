using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameboardController gController;
    public GameObject enemyCache;
    GameObject[] enemies = new GameObject[100];

    private void Awake()
    {
        int count = 0;
        foreach(EnemyData i in enemyCache.GetComponentsInChildren<EnemyData>())
        {
            enemies[count] = i.gameObject;
            foreach(Transform j in i.transform)            
            {                
                j.gameObject.SetActive(false);
            }
            count++;
        }
    }
    
    public void Reset(int val)
    {
        EnemyData eData = enemies[val].GetComponent<EnemyData>();
        SpriteRenderer sr = enemies[val].GetComponent<SpriteRenderer>();
        BoxCollider2D bCollider = enemies[val].GetComponent<BoxCollider2D>();

        foreach (Transform i in enemies[val].transform)
        {
            i.gameObject.SetActive(false);
        }

        eData.isActive = false;
        eData.fName = "";
        eData.lName = "";
        eData.id = "";

        sr.enabled = false;

        bCollider.enabled = false;
        eData.slowed = false;


        enemies[val].name = "Unused Villager";
    }

    public void CreateEnemy()
    {
        for( int i=0;i<enemies.Length;i++)
        {
            EnemyData eData = enemies[i].GetComponent<EnemyData>();            
            if(!eData.isActive)
            {
                EnemyBehavior eBehavior = enemies[i].GetComponent<EnemyBehavior>();

                foreach(Transform j in enemies[i].transform)
                {
                    j.gameObject.SetActive(true);
                }

                eData.id = eData.fName + eData.lName + i;

                enemies[i].name = eData.id;
                enemies[i].transform.position = gController.GetSpawnPoint();

                eData.pos = new Vector2Int((int)transform.position.x, (int)transform.position.x);

                eData.speed = 15;

                eData.gender = 0;

                eData.slowed = false;

                eData.currentLoc = GetCurrentLocation(enemies[i].transform.position);

                eData.allowedTypes = new TileTypes[] { TileTypes.Road, TileTypes.Ground };

                eData.pData = gController.pController.pData;

                eData.HP = 1;
                eData.AC = 1;
                eData.Agility = 2;

                eBehavior.ActivateEnemy();
            }
        }
    }

    TData GetCurrentLocation(Vector2 target)
    {
        RaycastHit2D hit = Physics2D.Raycast(target, Vector2.down);
        if(hit.collider != null)
        {
            if(hit.collider.tag == "Tile")
            {
                return hit.collider.GetComponent<TData>();                    
            }
        }

        return null;
    }
}
