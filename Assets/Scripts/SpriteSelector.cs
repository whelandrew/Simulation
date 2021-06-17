using UnityEngine;
using System.Collections.Generic;

public class SpriteSelector : MonoBehaviour
{
    public bool isActive = false;
    public bool selectionCompleted = false;
    public bool usingDragNDraw = false;
    public bool usingFourByFour = false;
    public bool isBuilding = false;
    
    Camera main;
    public TileManager tManager;

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
                if (isBuilding)
                {
                    SelectionCompleted();
                }
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

                if(usingFourByFour)
                {
                    FourByFour();
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

    private void FourByFour()
    {
        RaycastHit2D hit = Physics2D.Raycast(main.ScreenToWorldPoint(Input.mousePosition), -Vector2.up);
        if (hit.collider != null)
        {
            if (hit.collider.tag == "Tile")
            {
                //must be on Ground && next to Road
                TData tile = hit.collider.GetComponent<TData>();
                SpriteRenderer sRender = hit.collider.GetComponent<SpriteRenderer>();
                selectedTiles = new List<GameObject>();               

                //Vector3Int[] neighborsPos = new Vector3Int[] { tile.neighbors[7], tile.neighbors[5], tile.pos, tile.neighbors[3] };
                Vector3Int[] neighborsPos = FourByFourPlacement(tile);
                GameObject[] neighbors = tManager.RetrieveTileObjects(neighborsPos);                
                for (int i=0;i<neighbors.Length;i++)
                {
                    selectedTiles.Add(neighbors[i]);
                }                

                isBuilding = true;
            }
        }
    }

    private Vector3Int[] FourByFourPlacement(TData originTile)
    {
        Vector3Int[] neighborSet =new Vector3Int[4];        
        if(originTile.pos.x < 0 && originTile.pos.y <0)
        {
            //upper right 7, 5, O, 3
            neighborSet[0] = originTile.neighbors[7];
            neighborSet[1] = originTile.neighbors[5];
            neighborSet[2] = originTile.pos;
            neighborSet[3] = originTile.neighbors[3];
        }

        if(originTile.pos.x >0 && originTile.pos.y <0)
        {
            //upper left 2, 7, 0, O            
            neighborSet[0] = originTile.neighbors[2];
            neighborSet[1] = originTile.neighbors[7];
            neighborSet[2] = originTile.neighbors[0];
            neighborSet[3] = originTile.pos;            
        }

        if(originTile.pos.x >0 && originTile.pos.y>0)
        {
            //lower left 0, O, 1, 6
            neighborSet[0] = originTile.neighbors[0];
            neighborSet[1] = originTile.pos;
            neighborSet[2] = originTile.neighbors[1];            
            neighborSet[3] = originTile.neighbors[6];
        }

        if(originTile.pos.x <0 && originTile.pos.y >0)
        {
            //lower right O, 3, 6, 4
            neighborSet[0] = originTile.pos;
            neighborSet[1] = originTile.neighbors[3];            
            neighborSet[2] = originTile.neighbors[6];            
            neighborSet[3] = originTile.neighbors[4];
        }

        return neighborSet;
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
                        //if tile is type "Ground" && neighbor tile is of type "Road"
                        if (tile.tileLayer == TileLayers.Ground)
                        {                            
                            if (IsSelected(tile))
                            {
                                if (selectedTiles[selectedTiles.Count - 1] != hit.collider.gameObject)
                                {
                                    selectedTiles[selectedTiles.Count - 1].GetComponent<SpriteRenderer>().color = Color.white;                                        
                                    selectedTiles.RemoveAt(selectedTiles.Count - 1);
                                }
                            }

                            bool canDrag = tManager.FindClosestNeighbor(tile, new TileTypes[] { TileTypes.Road });
                            if (selectedTiles.Count > 0)
                                canDrag = tManager.FindClosestNeighbor(tile, new TileTypes[] { TileTypes.Road }, selectedTiles[selectedTiles.Count - 1].name);

                            if (canDrag)
                            {
                                isBuilding = true;
                                
                                if (!selectedTiles.Contains(hit.collider.gameObject))
                                {
                                    sRender.color = Color.red;
                                    selectedTiles.Add(hit.collider.gameObject);
                                }
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
        isBuilding = false;
    }

    private bool IsSelected(TData tileToCheck)
    {
        return selectedTiles.Find(x => x.GetComponent<TData>().pos == tileToCheck.pos);
    }
}