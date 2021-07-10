using UnityEngine;

public class EnemyAttackCollider : MonoBehaviour
{
    public EnemyBehavior eBahavior;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {            
            eBahavior.canAttack = true;
        }

        if (collision.tag == "Villager")
        {
            eBahavior.canAttack = true;
        }
    }
}
