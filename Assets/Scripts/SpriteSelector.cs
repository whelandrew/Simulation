using UnityEngine;
using System.Collections.Generic;

public class SpriteSelector : MonoBehaviour
{
    public bool isActive = false;
    public bool selectionCompleted = false;
    public bool usingDragNDraw = false;
    public bool usingFourByFour = false;    
    public bool isHighlighting = false;

    public SpriteRenderer constructionSprite;
    public GameObject fourByFourHighlighter;
    public GameObject oneByOneHighlighter;
    private GameObject highlighter;
    private SpriteRenderer[] highlighterChildren;
    public Vector2 highlighterSize = new Vector2(100, 100);
    private int foundForBuilding = 0;
    private int maxNeededForBuilding;
    private bool oneRoad = false;
    private List<TileTypes> neighborTypes = new List<TileTypes>();
    private bool canPlace = false;

    public Color correct;
    public Color wrong;
    public Color road;

    Camera main;
    public TileManager tManager;

    public GameObject highlightedObject;
    

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
            if(isHighlighting)
            {
                PlaceHighlighter();
            }

            if (selectionCompleted) selectionCompleted = false;
            
            // mouse up
            if (Input.GetMouseButtonUp(0))
            {
                if (canPlace)
                {
                    PlaceBuilding();
                }             
            }

            //mouse down
            if (Input.GetMouseButtonDown(0))
            {
                
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

    private void BeginPlacement()
    {
        isActive = true;
        isHighlighting = true;
        highlighter.SetActive(true);
    }

    public void BuildWorkshopSetup()
    {
        Reset();        
        usingFourByFour = true;
        constructionSprite.sprite = tManager.tempWorkshopSprites[0];
        constructionSprite.transform.localScale = constructionSprite.transform.localScale * 2;
        highlighter = fourByFourHighlighter;
        highlighterChildren = highlighter.GetComponentsInChildren<SpriteRenderer>();
        BeginPlacement();
    }

    public void BuildRoadSetup()
    {
        Reset();        
        usingDragNDraw = true;
        constructionSprite.sprite = tManager.tempRoadSprite;
        highlighter = oneByOneHighlighter;
        BeginPlacement();
    }
        
    void HighlightPlaceables(GameObject origin)
    {
        SpriteRenderer[] allSpots = highlighterChildren;
        oneRoad = false;
        foundForBuilding = 0;
        selectedTiles = new List<GameObject>();
        //find tile data   
        for (int i = 0; i < allSpots.Length; i++)
        {
            GameObject square = allSpots[i].gameObject;
            SpriteRenderer sr = square.GetComponent<SpriteRenderer>();
            RaycastHit2D hit = Physics2D.Raycast(square.transform.position, -Vector2.up);
            if (hit.collider != null)
            {
                if (hit.collider.tag == "Tile")
                {
                    TData tile = hit.collider.GetComponent<TData>();
                    if (tile.tileType != TileTypes.Ground)
                    {
                        if (allSpots[i].gameObject.name.Contains("Center"))
                        {
                            foundForBuilding--;
                            sr.color = wrong;
                        }
                        else
                        if (tile.tileType == TileTypes.Road)
                        {
                            //check for one road                        
                            sr.color = road;
                            oneRoad = true;
                        }
                        else
                        {
                            sr.color = Color.clear;
                        }
                    }
                    else
                    {
                        foundForBuilding++;
                        if (allSpots[i].gameObject.name.Contains("Center"))
                        {
                            selectedTiles.Add(square);
                        }
                        else
                        {
                            sr.color = Color.white;
                        }

                        if (i != allSpots.Length-1)
                        {
                            sr.color = Color.clear;
                        }
                        else
                        {   
                            if (!oneRoad)
                            {
                                sr.color = wrong;
                            }
                            else
                            {
                                sr.color = correct;
                            }
                        }
                    }
                }
            }            
        }


        canPlace = (foundForBuilding >= maxNeededForBuilding && oneRoad);
        if (!canPlace)
        {
            
        }
    }

    void PlaceHighlighter()
    {
        //4x4
        if(usingFourByFour)
        {
            maxNeededForBuilding = 12;
            RaycastHit2D hit = Physics2D.Raycast(main.ScreenToWorldPoint(Input.mousePosition), -Vector2.up);
            if (hit.collider != null)
            {
                if (hit.collider.tag == "Tile")
                {
                    TData tile = hit.collider.GetComponent<TData>();
                    //SpriteRenderer sr = highlighter.GetComponent<SpriteRenderer>();

                    highlighter.transform.position = hit.collider.transform.position;
                    highlighter.transform.localScale = highlighterSize;
                    highlighter.SetActive(true);

                    highlightedObject = hit.collider.gameObject;

                    HighlightPlaceables(hit.collider.gameObject);
                }
            }
        }
    }

    public void Reset()
    {
        canPlace = false;
        foundForBuilding = 0;
        oneRoad = false;
        selectedTiles = new List<GameObject>();
        isActive = false;
        selectionCompleted = false;
        usingDragNDraw = false;
        usingFourByFour = false;

        if(highlighter != null)
        {
            highlighter.SetActive(false);
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

    private void PlaceBuilding()
    {
        if(usingFourByFour)
        {

        }

        SelectionCompleted();
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
        if (usingDragNDraw && highlightedObject != null)
        {
            if (selectedTiles.Count >= drawCap)
            {
                Debug.Log("Cap Reached");
                SelectionCompleted();
            }
            else
            {
                TData tile = highlightedObject.GetComponent<TData>();
                SpriteRenderer sRender = highlightedObject.GetComponent<SpriteRenderer>();
                //if tile is type "Ground" && neighbor tile is of type "Road"
                if (tile.tileLayer == TileLayers.Ground)
                {
                    if (IsSelected(tile))
                    {
                        if (selectedTiles[selectedTiles.Count - 1] != highlightedObject)
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
                        if (!selectedTiles.Contains(highlightedObject))
                        {
                            sRender.color = Color.red;
                            selectedTiles.Add(highlightedObject);
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

    private bool IsSelected(TData tileToCheck)
    {
        return selectedTiles.Find(x => x.GetComponent<TData>().pos == tileToCheck.pos);
    }
}