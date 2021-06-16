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

    public GameObject highligher;
    
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

        highligher.SetActive(false);

        //REMOVE AFTER TESTING
        isActive = true;
        sSScript.isActive = true;

        //REMOVE AFTER TESTING
        buildButtonVal = 1;
        ConstructionSetup();

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
        RaycastHit2D hit = Physics2D.Raycast(main.ScreenToWorldPoint(Input.mousePosition), -Vector2.up);
        if (hit.collider != null)
        {
            if (hit.collider.tag == "Tile")
            {
                Debug.Log(hit.collider.gameObject.layer);
                if (hit.collider.gameObject.layer == (int)TileLayers.Road || hit.collider.gameObject.layer == (int)TileLayers.Ground)
                {
                    highligher.SetActive(true);
                    highligher.transform.localPosition = hit.collider.GetComponent<TData>().pos;
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
            case 1: ; BuildRoadSetup(); break;
            //setup workshop 
            case 2: break;
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
            case 2: break;
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
        sSScript.isActive = false;
        sSScript.usingDragNDraw = false;
        sSScript.selectionCompleted = false;
        highligher.SetActive(false);
        buildButtonVal = 0;
    }

    private void BuildRoadSetup()
    {
        sSScript.usingDragNDraw = true;
    }

    private void BuildRoad()
    {
        List<GameObject> tiles = sSScript.selectedTiles;
        for (int i = 0; i < tiles.Count; i++)
        {
            tManager.UpdateTile(tiles[i], TileTypes.Road);
        }

        Reset();
    }    
}