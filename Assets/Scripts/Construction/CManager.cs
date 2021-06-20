using UnityEngine;
using UnityEngine.UI;
public class CManager : MonoBehaviour
{
    private bool isActive;
    public TileManager tManager;

    private Camera main;

    public GameObject menuButtons;        

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
        sSScript = boxSelector.GetComponent<SpriteSelector>();

        //REMOVE AFTER TESTING///////////////////
        
        //road
        //SetBuildButtonVal(1);

        //workshop
        SetBuildButtonVal(2);
        ////////////////////////////////////////
    }

    void Update()
    {
        if(isActive && sSScript.selectionCompleted)
        {
            ContructionActions();
        }
    }
    
    //setup
    private void ConstructionSetup()    
    {
        switch (buildButtonVal)
        {
            //setup road 
            case 1: sSScript.BuildRoadSetup(); break;
            //setup workshop 
            case 2: sSScript.BuildWorkshopSetup(); break;
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
            ConstructionSetup();
        }
    }
    private void Reset()
    {
        sSScript.Reset();
        buildButtonVal = 0;
    }

    private void BuildWorkshop()
    {        
        tManager.UpdateTile(sSScript.selectedTiles.ToArray(), TileTypes.Workshop);
        Reset();
    }

    private void BuildRoad()
    {
        tManager.UpdateTile(sSScript.selectedTiles.ToArray(), TileTypes.Road);
        Reset();
    }    
}