using UnityEngine;
using System.Collections.Generic;

public class SpriteSelector : MonoBehaviour
{
    public bool isActive = false;
    public bool selectionCompleted = false;
    public bool usingDragNDraw = false;
    public bool usingFourByFour = false;    
    public bool isHighlighting = false;

    private SpriteRenderer constructionSprite;
    public GameObject fourByFourHighlighter;
    public GameObject oneByOneHighlighter;
    private GameObject highlighter;
    private SpriteRenderer[] highlighterChildren;
    public Vector2 highlighterSize = new Vector2(100, 100);
    private int foundForBuilding = 0;
    private int maxNeededForBuilding;
    private bool oneRoad = false;
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
                if (usingDragNDraw && selectedTiles.Count>0)
                {
                    PlaceBuilding();
                }
                else
                {
                    if (canPlace)
                    {
                        PlaceBuilding();
                    }
                }
            }

            //mouse down
            if (Input.GetMouseButtonDown(0))
            {
                
            }

            // mouse held down
            if (Input.GetMouseButton(0))
            {
                if (usingDragNDraw && canPlace)
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

    public Sprite tempWorkshopSpriteIcon;
    public Sprite tempHouseSpriteIcon;
    public Sprite tempFarmSpriteIcon;
    public Sprite tempRoadSpriteIcon;
    public Sprite tempDefenseSpriteIcon;
    
    public void BuildDefenseSetup()
    {
        Reset();
        usingFourByFour = true;
        constructionSprite = fourByFourHighlighter.GetComponent<SpriteRenderer>();
        constructionSprite.transform.localScale = constructionSprite.transform.localScale * 2;
        highlighter = fourByFourHighlighter;
        highlighterChildren = highlighter.GetComponentsInChildren<SpriteRenderer>();
        highlighterChildren[highlighterChildren.Length - 1].sprite = tempDefenseSpriteIcon;
        BeginPlacement();
    }

    public void BuildFarmSetup()
    {
        Reset();
        usingFourByFour = true;
        constructionSprite = fourByFourHighlighter.GetComponent<SpriteRenderer>();
        constructionSprite.transform.localScale = constructionSprite.transform.localScale * 2;
        highlighter = fourByFourHighlighter;
        highlighterChildren = highlighter.GetComponentsInChildren<SpriteRenderer>();
        highlighterChildren[highlighterChildren.Length - 1].sprite = tempFarmSpriteIcon;
        BeginPlacement();
    }

    public void BuildHouseSetup()
    {
        Reset();
        usingFourByFour = true;
        constructionSprite = fourByFourHighlighter.GetComponent<SpriteRenderer>();
        constructionSprite.transform.localScale = constructionSprite.transform.localScale * 2;
        highlighter = fourByFourHighlighter;
        highlighterChildren = highlighter.GetComponentsInChildren<SpriteRenderer>();
        highlighterChildren[highlighterChildren.Length - 1].sprite = tempHouseSpriteIcon;
        BeginPlacement();
    }

    public void BuildWorkshopSetup()
    {
        Reset();        
        usingFourByFour = true;
        constructionSprite = fourByFourHighlighter.GetComponent<SpriteRenderer>();        
        constructionSprite.transform.localScale = constructionSprite.transform.localScale * 2;
        highlighter = fourByFourHighlighter;
        highlighterChildren = highlighter.GetComponentsInChildren<SpriteRenderer>();
        highlighterChildren[highlighterChildren.Length - 1].sprite = tempWorkshopSpriteIcon;
        BeginPlacement();
    }

    public void BuildRoadSetup()
    {
        Reset();        
        usingDragNDraw = true;
        constructionSprite = oneByOneHighlighter.GetComponent<SpriteRenderer>();
        constructionSprite.sprite = tManager.tempRoadSprite;
        constructionSprite.transform.localScale = Vector3.one;
        highlighter = oneByOneHighlighter;
        highlighterChildren = highlighter.GetComponentsInChildren<SpriteRenderer>();
        highlighterChildren[highlighterChildren.Length - 1].sprite = tempRoadSpriteIcon;
        BeginPlacement();
    }
        
    void HighlightPlaceables(GameObject origin)
    {
        SpriteRenderer[] allSpots = highlighterChildren;
        oneRoad = false;
        foundForBuilding = 0;
        if (!usingDragNDraw)
        {
            selectedTiles = new List<GameObject>();
        }
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
                        if(allSpots[i].gameObject.name.Contains("Center"))
                        {
                            foundForBuilding--;
                            sr.color = wrong;
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

        if(usingDragNDraw)
        {
            TData tile = origin.GetComponent<TData>();
            canPlace = tManager.FindClosestNeighbor(tile, new TileTypes[] { TileTypes.Road });
            if (selectedTiles.Count > 0)
            {
                if (!selectedTiles[selectedTiles.Count - 1].name.Contains(tile.name))
                {
                    canPlace = tManager.FindClosestNeighbor(tile, new TileTypes[] { TileTypes.Road }, selectedTiles[selectedTiles.Count - 1].name);
                }
            }
        }
    }

    void PlaceHighlighter()
    {
        //4x4
        if (usingFourByFour)
        {
            maxNeededForBuilding = 12;
        }
        else
        {
            maxNeededForBuilding = 1;
        }

        RaycastHit2D hit = Physics2D.Raycast(main.ScreenToWorldPoint(Input.mousePosition), -Vector2.up);
        if (hit.collider != null)
        {
            if (hit.collider.tag == "Tile")
            {
                TData tile = hit.collider.GetComponent<TData>();

                highlighter.transform.position = hit.collider.transform.position;
                highlighter.transform.localScale = highlighterSize;
                highlighter.SetActive(true);

                highlightedObject = hit.collider.gameObject;

                HighlightPlaceables(hit.collider.gameObject);
            }
        }
    }
    
    private void DragNDraw()
    {
        Debug.Log("DragNDraw");
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

                    if (!selectedTiles.Contains(highlightedObject))
                    {
                        sRender.color = Color.white;
                        sRender.sprite = tempRoadSpriteIcon;
                        selectedTiles.Add(highlightedObject);
                    }
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

    private void PlaceBuilding()
    {
        SelectionCompleted();
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