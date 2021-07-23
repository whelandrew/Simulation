using UnityEngine;

public class VillagerCollider : MonoBehaviour
{
    public VillagerBehavior vBehavior;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            vBehavior.CalculateMood();
            vBehavior.freezeMovement = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag=="Player")
        {
            vBehavior.freezeMovement = false;
        }
    }
}
