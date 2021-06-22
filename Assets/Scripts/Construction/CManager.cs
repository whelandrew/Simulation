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

    public Text placementText;

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
        //SetBuildButtonVal(2);

        //house
        //SetBuildButtonVal(5);

        //farm
        //SetBuildButtonVal(3);

        //defense
        //SetBuildButtonVal(4);
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
            case 1: sSScript.BuildRoadSetup(); placementText.text = "Road";  break;
            //setup workshop 
            case 2: sSScript.BuildWorkshopSetup(); placementText.text = "Workshop"; break;
            //setup farm
            case 3: sSScript.BuildFarmSetup(); placementText.text = "Farm"; break; 
            //setup defense
            case 4: sSScript.BuildDefenseSetup(); placementText.text = "Defense"; break;
            //setup house
            case 5: sSScript.BuildHouseSetup(); placementText.text = "House"; break;

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
            case 3: BuildFarm();  break;
            //build defense
            case 4: BuildDefense();  break;
            //build house
            case 5: BuildHouse(); break;

            //do nothing
            default:
                buildButtonVal = 0;
                break;
        }
    }

    public void SetBuildButtonVal(int val)
    {
        buildButtonVal = val;
        isActive = true;
        ConstructionSetup();
    }
    private void Reset()
    {
        sSScript.Reset();
        buildButtonVal = 0;
    }

    private void BuildDefense()
    {
        tManager.UpdateTile(sSScript.selectedTiles.ToArray(), TileTypes.Defense);
        Reset();
    }

    private void BuildFarm()
    {
        tManager.UpdateTile(sSScript.selectedTiles.ToArray(), TileTypes.Farm);
        Reset();
    }    

    private void BuildHouse()
    {
        tManager.UpdateTile(sSScript.selectedTiles.ToArray(), TileTypes.House);
        Reset();
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