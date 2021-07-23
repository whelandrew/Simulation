using UnityEngine;
using UnityEngine.UI;

//does not go into actual game build
public class Overlay : MonoBehaviour
{
    Camera main;
    public GameObject MapPieces;

    public Text tileInfoText;
    private bool tileManagerReady;

    public SimGameboardController gController;
    public TileManager tileManager;
    public SpriteSelector sSelector;

    public PlayerData pData;
    public GameObject playerDataObject;
    public Text playerDataText;

    public VillagerController vController;
         
    void Start()
    {
        main = Camera.main;
    }
    private void Update()
    {
        if (tileManager.finishedLoading && !tileManagerReady)
        {
            tileManagerReady = true;
            AssignParent();
        }

        PlayerInfo();

        //VillagerPath();
    }
    
    void FixedUpdate()
    {
        if (tileManagerReady)
        {
            GetTileInfo();
        }
    }

    private void VillagerPath()
    {
        foreach (VillagerData v in vController.villagerCache.GetComponentsInChildren<VillagerData>())
        {            
            if (v.currentPath.Length > 0)
            {
                GameObject newObj = new GameObject();
                newObj.name = "Villager Path";
                LineRenderer newLine = newObj.AddComponent<LineRenderer>();
                newLine.positionCount = v.currentPath.Length;
                for (int i = 0; i < v.currentPath.Length; i++)
                {
                    newLine.SetPosition(i, v.currentPath[i]);
                }
            }
        }
    }

    private void PlayerInfo()
    {
        if (pData != null)
        {
            playerDataText.text = "";
            var fields = pData.GetType().GetFields();
            foreach (var i in fields)
            {
                playerDataText.text += i.Name + " : " + i.GetValue(pData) + "\n";
            }
        }
    }

    void GetTileInfo()
    {        
        // Cast a ray straight down.
        RaycastHit2D hit = Physics2D.Raycast(main.ScreenToWorldPoint(Input.mousePosition), -Vector2.up);

        // If it hits something...
        if (hit.collider != null)
        {
            if (hit.collider.tag == "Player")
            {

            }
            if(hit.collider.tag == "VillagerSpawns")
            {

            }
            if(hit.collider.tag =="Villager")
            {

            }
            else
            {
                tileInfoText.text = "";
                TData tile = hit.collider.GetComponent<TData>();
                if (tile != null)
                {
                    var fields = tile.GetType().GetFields();
                    foreach (var i in fields)
                    {
                        tileInfoText.text += i.Name + " : " + i.GetValue(tile) + "\n";
                    }
                }
            }
        }
    }
    
    void AssignParent()
    {
        Transform[] tileParents = MapPieces.GetComponentsInChildren<Transform>();
        GameObject[] tileObjects = tileManager.tileObjects;

        foreach (GameObject tile in tileObjects)
        {
            TData tData = tile.GetComponent<TData>();
            if (tData != null)
            {
                switch (tData.tileType)
                {
                    case TileTypes.Forest:
                        foreach (Transform t in tileParents)
                            if (t.gameObject.name.Contains("Forest")) tile.transform.parent = t;
                        break;
                    case TileTypes.Ground:
                        foreach (Transform t in tileParents)
                            if (t.gameObject.name.Contains("Ground")) tile.transform.parent = t;
                        break;
                    case TileTypes.River:
                        foreach (Transform t in tileParents)
                            if (t.gameObject.name.Contains("River")) tile.transform.parent = t;
                        break;
                    case TileTypes.Road:
                        foreach (Transform t in tileParents)
                            if (t.gameObject.name.Contains("Road")) tile.transform.parent = t;
                        break;
                    case TileTypes.TownCenter:
                        foreach (Transform t in tileParents)
                            if (t.gameObject.name.Contains("TownCenter")) tile.transform.parent = t;
                        break;
                    case TileTypes.Workshop:
                        foreach (Transform t in tileParents)
                            if (t.gameObject.name.Contains("Workshop")) tile.transform.parent = t;
                        break;
                    case TileTypes.House:
                        foreach (Transform t in tileParents)
                            if (t.gameObject.name.Contains("Homes")) tile.transform.parent = t;
                        break;
                }
            }
        }
    }
}
