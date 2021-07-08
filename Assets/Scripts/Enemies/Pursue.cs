using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pursue : MonoBehaviour
{
    public EnemyBehavior eBehavior;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag=="Player")
        {
            eBehavior.ChangeTarget(collision.gameObject.GetComponent<PlayerData>().currentLoc);
                
        }
        else
            if(collision.tag=="Villager")
        {
            if (collision.gameObject.GetComponent<VillagerData>().isActive)
            {
                eBehavior.ChangeTarget(collision.gameObject.GetComponent<VillagerData>().currentLocation);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            eBehavior.DefaultTarget(); 
        }
        else
            if (collision.tag == "Villager")
        {
            if (collision.gameObject.GetComponent<VillagerData>().isActive)
            {
                eBehavior.DefaultTarget();
            }
        }
    }
}