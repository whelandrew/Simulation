using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameboardController gController;
    public GameObject enemyCache;
    private List<GameObject> enemies = new List<GameObject>();    

    private void Awake()
    {
        foreach(EnemyData i in enemyCache.GetComponentsInChildren<EnemyData>())
        {
            enemies.Add(i.gameObject);
        }
    }

    public void CreateEnemy()
    {
        for( int i=0;i<enemies.Count;i++)
        {
            EnemyData eData = enemies[i].GetComponent<EnemyData>();
            EnemyBehavior eBehavior = enemies[i].GetComponent<EnemyBehavior>();
            if(!eData.isActive)
            {              
                eData.id = eData.fName + eData.lName + i;

                enemies[i].name = eData.id;
                enemies[i].transform.position = gController.GetSpawnPoint();

                eData.pos = new Vector2Int((int)transform.position.x, (int)transform.position.x);

                eData.speed = 15;

                eData.gender = 0;

                eData.slowed = false;

                eData.currentLoc = GetCurrentLocation(enemies[i].transform.position);

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
