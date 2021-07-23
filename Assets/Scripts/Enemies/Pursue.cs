using UnityEngine;
public class Pursue : MonoBehaviour
{
    public EnemyBehavior eBehavior;
    private void OnTriggerEnter2D(Collider2D collision)
    {        
        if(collision.tag=="Player")
        {
            PlayerData pData = collision.GetComponentInParent<PlayerData>();
            eBehavior.ChangeTarget(pData.curLoc);                
        }
        
        if(collision.tag=="Villager")
        {
            if (collision.gameObject.layer == 19)
            {
                if (collision.GetComponentInParent<VillagerData>().isActive)
                {
                    VillagerData vData = collision.GetComponentInParent<VillagerData>();
                    eBehavior.ChangeTarget(vData.currentLocation);
                }
            }
        }        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        /*
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
        */
    }
}