using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootCollider : MonoBehaviour
{
    public EnemyData eData;
    public EnemyBehavior eBehavior;
        

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Tile")
        {
            TData tile = collision.GetComponent<TData>();
            if(tile.tileType == TileTypes.Ground)
            {
                eBehavior.curSpeed = eData.speed / 2;
            }
            else
            {
                eBehavior.curSpeed = eData.speed;
            }
        }
    }

}
