using UnityEngine;
using System.Collections.Generic;

public class SpriteSelector : MonoBehaviour
{
    public bool isActive = false;
    public bool selectionCompleted = false;
    public bool usingDragNDraw = false;
    
    Camera main;

    //dragNDraw
    public int drawCap = 10;
    public List<GameObject> selectedTiles = new List<GameObject>();

    private void Start()
    {
        main = Camera.main;
    }

    private void Update()
    {
        if (isActive)
        {
            if (selectionCompleted) selectionCompleted = false;
            
            // mouse up
            if (Input.GetMouseButtonUp(0))
            {
                SelectionCompleted();
            }

            //mouse down
            if (Input.GetMouseButtonDown(0))
            {
                selectedTiles = new List<GameObject>();
            }

            // mouse held down
            if (Input.GetMouseButton(0))
            {
                if (usingDragNDraw)
                {
                    DragNDraw();
                }
            }
        }
    }

    private void MouseButtonUp()
    {
        if(usingDragNDraw)
        {
            Debug.Log("MouseButtonUp");
            SelectionCompleted();
        }
    }

    private void DragNDraw()
    {
        if (usingDragNDraw)
        {
            if (selectedTiles.Count >= drawCap)
            {
                Debug.Log("Cap Reached");
                SelectionCompleted();
            }
            else
            {
                RaycastHit2D hit = Physics2D.Raycast(main.ScreenToWorldPoint(Input.mousePosition), -Vector2.up);
                if (hit.collider != null)
                {
                    if (hit.collider.tag == "Tile")
                    {
                        TData tile = hit.collider.GetComponent<TData>();
                        SpriteRenderer sRender = hit.collider.GetComponent<SpriteRenderer>();

                        if (hit.collider.gameObject.layer == (int)TileLayers.Ground || hit.collider.gameObject.layer == (int)TileLayers.Road)
                        {
                            if (!selectedTiles.Contains(hit.collider.gameObject))
                            {                               
                                sRender.color = Color.yellow;
                                selectedTiles.Add(hit.collider.gameObject);
                            }
                        }
                    }
                }
            }
        }
    }    

    void SelectionCompleted()
    {        
        selectionCompleted = true;
    }
}