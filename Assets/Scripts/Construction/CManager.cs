using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CManager : MonoBehaviour
{
    public GameObject menuButtons;
    private Button[] cButtons;

    private bool spriteSelecterActive;
    public GameObject spriteSelecter;
    // Start is called before the first frame update
    void Start()
    {
        cButtons = menuButtons.GetComponentsInChildren<Button>();
        spriteSelecter.SetActive(false);

        spriteSelecterActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(spriteSelecterActive)
        {
            UseSpriteSelecter();
        }
    }

    void UseSpriteSelecter()
    {
        spriteSelecter.SetActive(true);

        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), -Vector2.up);

        // If it hits something...
        if (hit.collider != null)
        {
            if (hit.collider.tag == "Tile")
            {
                Debug.Log("OnTile");
                TData tile = hit.transform.GetComponent<TData>();
                spriteSelecter.transform.localPosition = tile.pos;
            }
        }
    }

    public void BuildRoadSelected()
    {
        spriteSelecterActive = true;
    }
}
