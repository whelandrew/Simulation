using UnityEngine;

public class PathCollider : MonoBehaviour
{
    public VillagerData vData;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (vData.isActive)
        {
            if (collision.tag == "Tile")
            {
                TData tile = collision.GetComponent<TData>();
                if (tile != vData.currentLocation)
                {
                    Debug.Log("Enter");
                    vData.currentLocation = collision.GetComponent<TData>();
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (vData.isActive)
        {
            Debug.Log("Exit");
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (vData.isActive)
        {
            if (collision.tag == "Tile")
            {
                TData tile = collision.GetComponent<TData>();
                if (tile != vData.currentLocation)
                {
                    Debug.Log("Stay");
                    vData.currentLocation = collision.GetComponent<TData>();
                }
            }
        }
    }
}
