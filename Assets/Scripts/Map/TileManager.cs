using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
public class TileManager : MonoBehaviour
{
    ArrayManager aManager = new ArrayManager();
    public bool finishedLoading = false;
    public Grid GridObject;
    private Tilemap[] tileMaps;
    public GameObject[] tileObjects = new GameObject[10000];    

    public Vector2Int MaxMapSize = new Vector2Int(50, 50);
    public Vector2Int MaxTileSize = new Vector2Int(16, 16);

    public int buildingTotal = 0;

    public Sprite groundSprite;
    public Sprite roadsprite;
    public Sprite workshopSprite;
    public Sprite homeSprite;
    public Sprite riverSprite;
    public Sprite treeSprite;
    public Sprite townCenterSprite;
    public Sprite defenseSprite;

    public Sprite tempRoadSprite;
    public Sprite[] tempWorkshopSprites;
    public Sprite[] tempHouseSprites;
    public Sprite[] tempFarmSprites;
    public Sprite[] tempDefenseSprites;

    private void Awake()
    {
        tileMaps = GridObject.GetComponentsInChildren<Tilemap>();
        StartCoroutine(CreateMap());
    }
    
    private IEnumerator CreateMap()
    {
        InitializeBaseTileMap();
        finishedLoading = true;
        yield return null;
    }    
    
    private void InitializeBaseTileMap()
    {
        tileObjects = new GameObject[10000];
        int tileObjectTotal = 0;

        for (int i = 0; i < tileMaps.Length; i++)
        {
            BoundsInt bounds = tileMaps[i].cellBounds;
            TileBase[] allTiles = tileMaps[i].GetTilesBlock(bounds);

            for (int x = 0; x < bounds.size.x; x++)
            {
                for (int y = 0; y < bounds.size.y; y++)
                {
                    TileBase tile = allTiles[x + y * bounds.size.x];
                    if(tile !=null)
                    {
                        Vector3Int tilePos = new Vector3Int(x - (MaxMapSize.x - 1), y - (MaxMapSize.y + 1), 0);
                        TData tData = FindTileData(tilePos);
                        if(tData == null)
                        {
                            NewTile(tilePos, tileMaps[i], tile, x, y, tileObjectTotal);
                            tileObjectTotal++;
                        }
                    }
                }
            }
        }

        GameObject[] finalSet = new GameObject[tileObjectTotal];
        for(int i=0;i<tileObjectTotal;i++)
        {
            finalSet[i] = tileObjects[i];
        }
        tileObjects = finalSet;

        if(tileMaps.Length >0)
        {
            GridObject.gameObject.SetActive(false);
        }

        Debug.Log(tileObjects.Length);
        Debug.Log("Tilemaps Sorted");
    }

    private void NewTile(Vector3Int pos, Tilemap tMap, TileBase tile, int x, int y, int val)
    {
        GameObject newTileObject = new GameObject();
        tileObjects[val] = newTileObject;

        TData tileData = newTileObject.AddComponent<TData>();        
        tileData.pos = pos;
        tileData.size = MaxTileSize;
        tileData.name = (tMap.gameObject.name + tile.name + (x).ToString() + (y).ToString());
        tileData.id = x + y;
        tileData.tileType = AssignTileType(tileData);        
        
        newTileObject.tag = "Tile";
        newTileObject.name = tileData.name;
        newTileObject.transform.position = tileData.pos;

        BoxCollider2D newCollider = newTileObject.AddComponent<BoxCollider2D>();
        newCollider.isTrigger = true;
        newCollider.size = Vector2.one;
        
        SpriteRenderer spriteRenderer = newTileObject.AddComponent<SpriteRenderer>();
        Sprite sprite = tMap.GetSprite(tileData.pos);
        spriteRenderer.sprite = sprite;
        spriteRenderer.color = new Color(1, 1, 1, 1);

        tileData.sprite = sprite.name;

        SetupNeighbors(tileData);

        AssignLayer(newTileObject, tileData);        
    }

    private void AssignLayer(GameObject tile, TData data)
    {
        switch (data.tileType)
        {
            case TileTypes.Road:
                data.tileLayer = TileLayers.Road;
                data.pathVal = 0;
                data.isBuilding = false;
                data.canInterract = false;
                break;
            case TileTypes.Ground:
                data.tileLayer = TileLayers.Ground;
                data.pathVal = 1;
                data.isBuilding = false;
                data.canInterract = false;
                break;
            case TileTypes.Forest:
                data.tileLayer = TileLayers.Tree;
                data.pathVal = 2;
                data.isBuilding = false;
                data.canInterract = false;
                break;
            case TileTypes.River:
                data.tileLayer = TileLayers.River;
                data.pathVal = 3;
                data.isBuilding = false;
                data.canInterract = false;
                break;
            case TileTypes.Workshop:
                data.tileLayer = TileLayers.Workshop;
                data.pathVal = 4;
                data.isBuilding = true;
                data.canInterract = true;
                break;
            case TileTypes.TownCenter:
                data.tileLayer = TileLayers.TownCenter;
                data.pathVal = 5;
                data.isBuilding = true;
                data.canInterract = true;
                break;
            case TileTypes.House:
                data.tileLayer = TileLayers.House;
                data.pathVal = 6;
                data.isBuilding = true;
                data.canInterract = true;
                break;
            case TileTypes.Farm:
                data.tileLayer = TileLayers.Farm;
                data.pathVal = 7;
                data.isBuilding = true;
                data.canInterract = true;
                break;
            case TileTypes.Defense:
                data.tileLayer = TileLayers.Defense;
                data.pathVal = 8;
                data.isBuilding = true;
                data.canInterract = true;
                break;
        }

        tile.layer = (int)data.tileLayer;
    }

    private TileTypes AssignTileType(TData tile)
    {
        string tileName = tile.name;
        if (tileName.Contains("Ground"))
        {
            return TileTypes.Ground;
        }
        if (tileName.Contains("Forest"))
        {
            return TileTypes.Forest;
        }
        if (tileName.Contains("River"))
        {
            return TileTypes.River;
        }
        if (tileName.Contains("Road"))
        {
            return TileTypes.Road;
        }
        if (tileName.Contains("TownCenter"))
        {
            return TileTypes.TownCenter;
        }
        if (tileName.Contains("Workshop"))
        {
            return TileTypes.Workshop;
        }
        if (tileName.Contains("House"))
        {
            return TileTypes.House;
        }

        return TileTypes.None;
    }    

    public void UpdateTile(GameObject[] tilesToUpdate, TileTypes updateTo)
    {
        int spriteCount = 0;
        int newBuildingCount = 0;
        for (int i =0;i<tilesToUpdate.Length;i++)
        {
            for (int j = 0; j < tileObjects.Length; j++)
            {
                if (tileObjects[j].GetComponent<TData>().pos == tilesToUpdate[i].transform.position)
                {
                    //GameObject existingTile = tileObjects.Where(x => x.GetComponent<TData>().pos == tilesToUpdate[i].transform.position).FirstOrDefault();
                    GameObject existingTile = tileObjects[j];
                    TData tile = existingTile.GetComponent<TData>();
                    SpriteRenderer sr = existingTile.GetComponent<SpriteRenderer>();
                    if (existingTile)
                    {
                        switch (updateTo)
                        {
                            case TileTypes.Road:
                                existingTile.name = ("Road" + (tile.pos.x).ToString() + (tile.pos.y).ToString());
                                tile.isBuilding = false;
                                tile.canInterract = false;
                                sr.sprite = tempRoadSprite;
                                sr.color = new Color(1, 1, 1, 1);
                                newBuildingCount = 1;
                                break;
                            case TileTypes.Workshop:
                                existingTile.name = ("Workshop" + (tile.pos.x).ToString() + (tile.pos.y).ToString());
                                tile.isBuilding = true;
                                tile.canInterract = true;
                                sr.sprite = tempWorkshopSprites[spriteCount];
                                spriteCount++;
                                sr.color = new Color(1, 1, 1, 1);
                                newBuildingCount = 1;
                                break;
                            case TileTypes.House:
                                existingTile.name = ("House" + (tile.pos.x).ToString() + (tile.pos.y).ToString());
                                tile.isBuilding = true;
                                tile.canInterract = true;
                                sr.sprite = tempHouseSprites[spriteCount];
                                spriteCount++;
                                sr.color = new Color(1, 1, 1, 1);
                                newBuildingCount = 1;
                                break;
                            case TileTypes.Farm:
                                existingTile.name = ("Farm" + (tile.pos.x).ToString() + (tile.pos.y).ToString());
                                tile.isBuilding = true;
                                tile.canInterract = true;
                                sr.sprite = tempFarmSprites[spriteCount];
                                spriteCount++;
                                sr.color = new Color(1, 1, 1, 1);
                                newBuildingCount = 1;
                                break;
                            case TileTypes.Defense:
                                existingTile.name = ("Defense" + (tile.pos.x).ToString() + (tile.pos.y).ToString());
                                tile.isBuilding = true;
                                tile.canInterract = true;
                                sr.sprite = tempDefenseSprites[spriteCount];
                                spriteCount++;
                                sr.color = new Color(1, 1, 1, 1);
                                newBuildingCount = 1;
                                break;
                        }

                        tile.name = existingTile.name;
                        tile.tileType = updateTo;

                        buildingTotal += newBuildingCount;

                        AssignLayer(tilesToUpdate[i], tile);
                    }
                }
            }
        }
    }

    private void SetupNeighbors(TData tile)
    {               
        //x-1, y left       
        tile.neighbors[0] = new Vector3Int(tile.pos.x - 1, tile.pos.y, 0);

        //x-1, y-1 lower left
        tile.neighbors[1] = new Vector3Int(tile.pos.x - 1, tile.pos.y - 1, 0);

        //x-1, y+1 upper left
        tile.neighbors[2] = new Vector3Int(tile.pos.x - 1, tile.pos.y + 1, 0);

        //x+1, y right
        tile.neighbors[3] = new Vector3Int(tile.pos.x + 1, tile.pos.y, 0);

        //x+1, y-1 lower right
        tile.neighbors[4] = new Vector3Int(tile.pos.x + 1, tile.pos.y - 1, 0);

        //x+1, y+1 upper right
        tile.neighbors[5] = new Vector3Int(tile.pos.x + 1, tile.pos.y + 1, 0);

        //x  , y-1 down
        tile.neighbors[6] = new Vector3Int(tile.pos.x, tile.pos.y - 1, 0);

        //x  , y+1 up
        tile.neighbors[7] = new Vector3Int(tile.pos.x, tile.pos.y + 1, 0);
    }

    public bool FindSecondClosestNeighbor(TData curTile, TileTypes[] tileTypeToFind, bool useOnlyFour, string forcedNeighbor = "null")
    {        
        for (int i = 0; i < curTile.neighbors.Length; i++)
        {
            if (useOnlyFour)
            {
                if (i == 0 || i == 3 || i == 6 || i == 7)
                {
                    TData neighborTile = FindTileData(curTile.neighbors[i]);
                    if (neighborTile != null)
                    {
                        if (FindClosestNeighbor(neighborTile, tileTypeToFind))
                        {
                            return true;
                        }
                    }
                }
            }
            else
            {
                TData neighborTile = FindTileData(curTile.neighbors[i]);
                if (neighborTile != null)
                {
                    if (FindClosestNeighbor(neighborTile, tileTypeToFind))
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    public bool FindClosestNeighbor(TData curTile, TileTypes[] tileTypeToFind, string forcedNeighbor = "null")
    {
        Debug.Log("FindNeighbors");
        
        for (int i = 0; i < curTile.neighbors.Length; i++)
        {
            TData data = FindTileData(curTile.neighbors[i]);
            if (data != null)
            {

                if (forcedNeighbor == data.name)
                {
                    return true;
                }

                if (tileTypeToFind.Contains(data.tileType))
                {
                    return true;
                }
            }
        }
        
        return false;
    }

    public TData[] GetClosestNeighborsOfType(TData curTile, TileTypes tileTypeToFind)
    {
        TData[] foundTiles = new TData[curTile.neighbors.Length];
        for (int i = 0; i < curTile.neighbors.Length; i++)
        {
            TData tile = FindTileData(curTile.neighbors[i]);
            if (tile != null)
            {
                if (tile.tileType == tileTypeToFind && !tile.owned)
                {
                    foundTiles[i] = tile;
                }
            }
        }

        return aManager.ResizeTData(foundTiles);
    }
    public GameObject[] RetrieveTileObjects(Vector3Int[] tilePos)
    {
        //List<GameObject> foundTiles = new List<GameObject>();
        GameObject[] foundTiles = new GameObject[100];
        int count = 0;
        for (int i = 0; i < tilePos.Length; i++)
        {
            for(int j=0;j<tileObjects.Length; j++)
            {
                if(tileObjects[j].GetComponent<TData>().pos == tilePos[i])
                {
                    //foundTiles.Add(tileObjects[j]);
                    foundTiles[count] = tileObjects[j];
                    count++;
                }
            }
        }        

        return foundTiles;        
    }

    public TData FindTileData(Vector3Int tilePos)
    {
        for (int i = 0; i < tileObjects.Length; i++)
        {
            if (tileObjects[i] != null && tileObjects[i].GetComponent<TData>().pos == tilePos)
            {
                return tileObjects[i].GetComponent<TData>();
            }
        }

        return null;
    }

    //public List<TData> GetAllTilesOfType(TileTypes type)
    public TData[] GetAllTilesOfType(TileTypes type)
    {
        TData[] tiles = new TData[100];
        int count = 0;
        for(int i = 0; i < tileObjects.Length; i++)
        {
            TData tile = tileObjects[i].GetComponent<TData>();
            if(tile!=null)
            {
                if(tile.tileType == type)
                {
                    //tiles.Add(tile);
                    tiles[count] = tile;
                    count++;
                }
            }
        }

        return tiles;
    }

    public TData[] GetAllTileData()
    {
        TData[] tiles = new TData[tileObjects.Length];
        for(int i=0;i<tileObjects.Length; i++)
        {
            tiles[i] = tileObjects[i].GetComponent<TData>();
        }
        return tiles;
    }
    
    public Vector2Int GetTargetCenter(TileTypes type, string owner=null)
    {
        //List<Vector3Int> pos = new List<Vector3Int>();
        Vector3Int[] pos = new Vector3Int[100];
        int count = 0;
        if(owner==null)
        {
            for(int i =0;i<tileObjects.Length; i++)
            {
                if(tileObjects[i].GetComponent<TData>().tileType == type)
                {
                    //pos.Add(tileObjects[i].GetComponent<TData>().pos);
                    pos[0] = tileObjects[i].GetComponent<TData>().pos;
                    count++;
                }
            }
        }

        Vector3Int final = Vector3Int.zero;
        for (int i = 0; i < pos.Length; i++)
        {
            final += pos[i];
        }

        return new Vector2Int((final.x / count), final.y / count);
    }

    public TData GetOneTileOfType(TileTypes type, string owner=null)
    {
        for(int i=0;i<tileObjects.Length; i++)
        {
            if(tileObjects[i].GetComponent<TData>().tileType==type)
            {
                if(owner!=null)
                {
                    //find specific type owned by villager
                }
                else
                {
                    return tileObjects[i].GetComponent<TData>();
                }
            }
        }

        return null;
    }

    public Sprite GetTileSprite(TileTypes type)
    {
        switch(type)
        {
            case TileTypes.Ground: return groundSprite;
            case TileTypes.Forest: return treeSprite;
            case TileTypes.Defense: return defenseSprite;
            case TileTypes.River: return riverSprite;
            case TileTypes.Road: return roadsprite;
            case TileTypes.TownCenter: return townCenterSprite;
            case TileTypes.Workshop: return workshopSprite;
            case TileTypes.House: return homeSprite;
            default: return null;
        }
    }

    
    //public List<TData> GetNeighborTilesOfType(TData[] neighbors, TileTypes[] type)
    public TData[] GetNeighborTilesOfType(TData[] neighbors, TileTypes[] type)
    {
        TData[] neigh = new TData[100]; ;
        int count = 0;
        for(int i =0;i<neighbors.Length;i++)
        {
            if(type.Contains(neighbors[i].tileType))
            {
                //neigh.Add(neighbors[i]);
                neigh[count] = neighbors[i];
                count++;
                continue;
            }
        }        

        return neigh;
    }    
    
    public TData[] GetAllBuildings()
    {
        TData[] allBuildings = new TData[0];
        for(int i=0;i<tileObjects.Length;i++)
        {
            TData tile = tileObjects[i].GetComponent<TData>();
            if(tile.isBuilding)
            {
                allBuildings = aManager.AddTData(allBuildings, tile);
            }
        }

        return allBuildings;
    }
}
