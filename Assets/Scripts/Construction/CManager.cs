using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
public class CManager : MonoBehaviour
{
    private bool isActive;
    public TileManager tManager;

    private Camera main;

    public GameObject menuButtons;
    private Button[] cButtons;

    public GameObject highlighter;
    private SpriteRenderer srHighlighter;
    public Vector2 highlighterSize = new Vector2(100, 100);
    private bool IsFourByFour;

    public GameObject boxSelector;
    public SpriteSelector sSScript;



    //0 - none
    //1 - road
    //2 - workshop
    //3 - farm
    //4 - defense
    //5 - house
    private int buildButtonVal = 0;

    void Start()
    {
        main = Camera.main;
        cButtons = menuButtons.GetComponentsInChildren<Button>();
        sSScript = boxSelector.GetComponent<SpriteSelector>();
        srHighlighter = highlighter.GetComponent<SpriteRenderer>();
        highlighter.SetActive(false);


        //REMOVE AFTER TESTING
        //road
        //SetBuildButtonVal(1);
        //sSScript.usingDragNDraw = true;
        //workshop
        SetBuildButtonVal(2);
    }

    void Update()
    {
        if (buildButtonVal != 0)
        {
            PlaceHighlighter();
        }

        if(isActive && sSScript.selectionCompleted)
        {
            ContructionActions();
        }
    }

    void PlaceHighlighter()
    {
        if (!IsFourByFour)
        {
            //1x1
            RaycastHit2D hit = Physics2D.Raycast(main.ScreenToWorldPoint(Input.mousePosition), -Vector2.up);
            if (hit.collider != null)
            {
                if (hit.collider.tag == "Tile")
                {
                    TData tile = hit.collider.GetComponent<TData>();
                    if (tManager.FindClosestNeighbor(tile, new TileTypes[] { TileTypes.Road }) && tile.tileType == TileTypes.Ground)
                    {
                        highlighter.transform.position = hit.collider.transform.position;
                        highlighter.transform.localScale = highlighterSize;
                        srHighlighter.color = Color.yellow;
                        highlighter.SetActive(true);
                    }
                    else
                    {
                        srHighlighter.color = Color.red;
                    }
                }
            }
        }
        else
        {
            //4x4
            Vector2 mPos = main.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(mPos, -Vector2.up);
            if (hit.collider != null)
            {
                if (hit.collider.tag == "Tile")
                {
                    TData tile = hit.collider.GetComponent<TData>();
                    if (tManager.FindClosestNeighbor(tile, new TileTypes[] { TileTypes.Ground }) 
                        && tile.tileType == TileTypes.Ground 
                        && !tManager.FindClosestNeighbor(tile, new TileTypes[] { TileTypes.Road }))
                    {
                        if (tManager.FindSecondClosestNeighbor(tile, new TileTypes[] { TileTypes.Road, TileTypes.Forest, TileTypes.River, TileTypes.TownCenter, TileTypes.Workshop }))
                        {
                            float x = hit.collider.transform.position.x - .5f;
                            float y = hit.collider.transform.position.y + .5f;
                            if (hit.collider.transform.position.x < 0)
                                x = hit.collider.transform.position.x + .5f;
                            if (hit.collider.transform.position.y > 0)
                                y = hit.collider.transform.position.y - .5f;

                            highlighter.transform.position = new Vector2(x,y);
                            highlighter.transform.localScale = highlighterSize*2;
                            srHighlighter.color = Color.yellow;
                            highlighter.SetActive(true);
                        }
                    }
                    else
                    {
                        srHighlighter.color = Color.red;
                    }
                }
            }                
        }
    }
    //setup
    private void ConstructionSetup()    
    {
        switch (buildButtonVal)
        {
            //setup road 
            case 1: BuildRoadSetup(); break;
            //setup workshop 
            case 2: BuildWorkshopSetup(); break;
            //setup farm
            case 3: break;
            //setup defense
            case 4: break;
            //setup house
            case 5: break;

            //do nothing
            default:
                buildButtonVal = 0;
                break;
        }
    }

    //run the different build commands for SpriteSelector
    private void ContructionActions()
    {
        sSScript.isActive = false;
        sSScript.usingDragNDraw = false;

        switch (buildButtonVal)
        {
            //build road 
            case 1: BuildRoad(); break;
            //build workshop 
            case 2: BuildWorkshop(); break;
            //build farm
            case 3: break;
            //build defense
            case 4: break;
            //build house
            case 5: break;

            //do nothing
            default:
                buildButtonVal = 0;
                break;
        }
    }

    public void SetBuildButtonVal(int val)
    {
        if (buildButtonVal == 0)
        {
            buildButtonVal = val;
            isActive = true;                        
            sSScript.isActive = true;
            ConstructionSetup();
        }
    }
    private void Reset()
    {
        isActive = false;
        IsFourByFour = false;
        sSScript.isActive = false;
        sSScript.usingDragNDraw = false;
        sSScript.selectionCompleted = false;
        highlighter.SetActive(false);
        buildButtonVal = 0;
    }

    private void BuildWorkshopSetup()
    {
        sSScript.usingFourByFour = true;
        IsFourByFour = true;
    }

    private void BuildWorkshop()
    {
        tManager.UpdateTile(sSScript.selectedTiles.ToArray(), TileTypes.Workshop);
        Reset();
    }

    private void BuildRoadSetup()
    {
        sSScript.usingDragNDraw = true;   
    }

    private void BuildRoad()
    {
        tManager.UpdateTile(sSScript.selectedTiles.ToArray(), TileTypes.Road);
        Reset();
    }    
}