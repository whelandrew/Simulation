using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System.Linq;
public class TileManager : MonoBehaviour
{
    public bool finishedLoading = false;
    public Grid GridObject;
    private Tilemap[] tileMaps;
    public List<GameObject> tileObjects = new List<GameObject>();

    public Vector2Int MaxMapSize = new Vector2Int(50, 50);
    public Vector2Int MaxTileSize = new Vector2Int(16, 16);

    public int buildingTotal = 0;

    private void Start()
    {
        tileMaps = GridObject.GetComponentsInChildren<Tilemap>();
        CreateMap();
    }

    private void CreateMap()
    {
        InitializeBaseTileMap();
    }

    private void InitializeBaseTileMap()
    {
        //build ground set (base layout of all tiles)
        int max = tileMaps.Length;
        Tilemap groundTiles = new Tilemap();
        List<Tilemap> otherTiles = new List<Tilemap>();

        for (int i = 0; i < tileMaps.Length; i++)
        {
            if (i == (max - 1)) groundTiles = tileMaps[i];
            else otherTiles.Add(tileMaps[i]);
        }

        BuildBaseTiles(groundTiles);
        BuildAllOtherTiles(otherTiles);

        Debug.Log("Tilemaps Sorted");
    }

    private void BuildBaseTiles(Tilemap tMap)
    {
        BoundsInt bounds = tMap.cellBounds;
        TileBase[] allTiles = tMap.GetTilesBlock(bounds);

        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                TileBase tile = allTiles[x + y * bounds.size.x];
                if (tile != null)
                {
                    GameObject newTileObject = new GameObject();
                    newTileObject.tag = "Tile";
                    TData tileData = newTileObject.AddComponent<TData>();
                    tileData.pos = new Vector3Int(x - (MaxMapSize.x - 1), y - (MaxMapSize.y + 1), 0);
                    tileData.size = MaxTileSize;
                    tileData.name = (tMap.gameObject.name + tile.name + (x).ToString() + (y).ToString());
                    tileData.id = x + y;
                    tileData.tileType = AssignTileType(tileData);
                    SetupNeighbors(tileData);

                    AssignLayer(newTileObject, tileData);

                    newTileObject.name = tileData.name;
                    newTileObject.transform.position = tileData.pos;

                    BoxCollider2D newCollider = newTileObject.AddComponent<BoxCollider2D>();
                    newCollider.isTrigger = true;
                    newCollider.size = Vector2.one;

                    SpriteRenderer spriteRenderer = newTileObject.AddComponent<SpriteRenderer>();
                    Sprite sprite = tMap.GetSprite(tileData.pos);
                    spriteRenderer.sprite = sprite;

                    tileObjects.Add(newTileObject);
                }
            }
        }
    }

    private void BuildAllOtherTiles(List<Tilemap> tMaps)
    {
        Tilemap tMap = tMaps[tMaps.Count - 1];
        List<TData> newData = new List<TData>();
        BoundsInt bounds = tMap.cellBounds;
        TileBase[] allTiles = tMap.GetTilesBlock(bounds);

        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                TileBase tile = allTiles[x + y * bounds.size.x];
                if (tile != null)
                {
                    TData updateData = new TData();
                    updateData.name = (tMap.gameObject.name + tile.name + (x).ToString() + (y).ToString());
                    updateData.tileType = AssignTileType(updateData);
                    updateData.id = x + y;
                    updateData.pos = new Vector3Int(x - (MaxMapSize.x - 1), y - (MaxMapSize.y + 1), 0);
                    newData.Add(updateData);
                }
            }
        }

        //update tiles with overlapping values
        for (int i = 0; i < tileObjects.Count; i++)
        {
            TData tData = tileObjects[i].GetComponent<TData>();
            SpriteRenderer spriteRenderer = tileObjects[i].GetComponent<SpriteRenderer>();

            for (int j = 0; j < newData.Count; j++)
            {
                if (tData.pos == newData[j].pos)
                {
                    tData.id = newData[j].id;
                    tData.name = newData[j].name;
                    tData.tileType = newData[j].tileType;
                    tileObjects[i].name = newData[j].name;

                    Sprite sprite = tMap.GetSprite(tData.pos);
                    spriteRenderer.sprite = sprite;

                    AssignLayer(tileObjects[i], tData);
                }
            }
        }

        //continue proccess?
        int max = tMaps.Count;
        if (max - 1 <= 0)
        {
            GridObject.gameObject.SetActive(false);
            finishedLoading = true;
            return;
        }
        else
        {
            tMaps.RemoveAt(max - 1);
            BuildAllOtherTiles(tMaps);
        }
    }

    private void AssignLayer(GameObject tile, TData data)
    {
        switch (data.tileType)
        {
            case TileTypes.Road:
                data.tileLayer = TileLayers.Road;
                data.isBuilding = false;
                break;
            case TileTypes.Ground:
                data.tileLayer = TileLayers.Ground;
                data.isBuilding = false;
                break;
            case TileTypes.Forest:
                data.tileLayer = TileLayers.Tree;
                data.isBuilding = false;
                break;
            case TileTypes.River:
                data.tileLayer = TileLayers.River;
                data.isBuilding = false;
                break;
            case TileTypes.Workshop:
                data.tileLayer = TileLayers.Workshop;
                data.isBuilding = true;
                break;
            case TileTypes.TownCenter:
                data.tileLayer = TileLayers.TownCenter;
                data.isBuilding = true;
                break;
            case TileTypes.House:
                data.tileLayer = TileLayers.House;
                data.isBuilding = true;
                break;
            case TileTypes.Farm:
                data.tileLayer = TileLayers.Farm;
                data.isBuilding = true;
                break;
            case TileTypes.Defense:
                data.tileLayer = TileLayers.Defense;
                data.isBuilding = true;
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

    public Sprite tempRoadSprite;
    public Sprite[] tempWorkshopSprites;
    public Sprite[] tempHouseSprites;
    public Sprite[] tempFarmSprites;
    public Sprite[] tempDefenseSprites;

    public void UpdateTile(GameObject[] tilesToUpdate, TileTypes updateTo)
    {
        int spriteCount = 0;
        int newBuildingCount = 0;
        for (int i =0;i<tilesToUpdate.Length;i++)
        {
            GameObject existingTile = tileObjects.Where(x => x.GetComponent<TData>().pos == tilesToUpdate[i].transform.position).FirstOrDefault();
            TData tile = existingTile.GetComponent<TData>();
            SpriteRenderer sr = existingTile.GetComponent<SpriteRenderer>();
            if (existingTile)
            {
                switch (updateTo)
                {
                    case TileTypes.Road:
                        existingTile.name = ("Road" + (tile.pos.x).ToString() + (tile.pos.y).ToString());
                        tile.isBuilding = false;
                        sr.sprite = tempRoadSprite;
                        sr.color = Color.white;
                        newBuildingCount = 1;
                        break;
                    case TileTypes.Workshop:
                        existingTile.name = ("Workshop" + (tile.pos.x).ToString() + (tile.pos.y).ToString());
                        tile.isBuilding = true;
                        sr.sprite = tempWorkshopSprites[spriteCount];
                        spriteCount++;
                        sr.color = Color.white;
                        newBuildingCount = 1;
                        break;
                    case TileTypes.House:
                        existingTile.name = ("House" + (tile.pos.x).ToString() + (tile.pos.y).ToString());
                        tile.isBuilding = true;
                        sr.sprite = tempHouseSprites[spriteCount];
                        spriteCount++;
                        sr.color = Color.white;
                        newBuildingCount = 1;
                        break;
                    case TileTypes.Farm:
                        existingTile.name = ("Farm" + (tile.pos.x).ToString() + (tile.pos.y).ToString());
                        tile.isBuilding = true;
                        sr.sprite = tempFarmSprites[spriteCount];
                        spriteCount++;
                        sr.color = Color.white;
                        newBuildingCount = 1;
                        break;
                    case TileTypes.Defense:
                        existingTile.name = ("Defense" + (tile.pos.x).ToString() + (tile.pos.y).ToString());
                        tile.isBuilding = true;
                        sr.sprite = tempDefenseSprites[spriteCount];
                        spriteCount++;
                        sr.color = Color.white;
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

    private void SetupNeighbors(TData tile)
    {
        tile.neighbors = new Vector3Int[8];
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
        
        for(int i =0;i<tile.neighbors.Length;i++)
        {
            if(tile.neighbors[i]==null)
            {
                tile.neighbors[i] = Vector3Int.zero;
            }
        }
    }

    public bool FindSecondClosestNeighbor(TData curTile, TileTypes[] tileTypeToFind, bool useOnlyFour, string forcedNeighbor = "null")
    {
        Vector3Int[] neighbors = curTile.neighbors;
        for (int i = 0; i < neighbors.Length; i++)
        {
            if (useOnlyFour)
            {
                if (i == 0 || i == 3 || i == 6 || i == 7)
                {
                    TData neighborTile = tileObjects.Where(j => j.GetComponent<TData>().pos == neighbors[i]).FirstOrDefault().GetComponent<TData>();
                    if (FindClosestNeighbor(neighborTile, tileTypeToFind))
                        return true;
                }
            }
            else
            {
                TData neighborTile = tileObjects.Where(j => j.GetComponent<TData>().pos == neighbors[i]).FirstOrDefault().GetComponent<TData>();
                if (FindClosestNeighbor(neighborTile, tileTypeToFind))
                    return true;
            }
        }

        return false;
    }

    public bool FindClosestNeighbor(TData curTile, TileTypes[] tileTypeToFind, string forcedNeighbor = "null")
    {
        Debug.Log("FindNeighbors");
        for (int i = 0; i < curTile.neighbors.Length; i++)
        {
            TData data = tileObjects.Where(j => j.GetComponent<TData>().pos == curTile.neighbors[i]).FirstOrDefault().GetComponent<TData>();

            if (forcedNeighbor == data.name)
            {
                return true;
            }

            if(tileTypeToFind.Contains(data.tileType))
            {
                return true;
            }

        }
        return false;
    }

    public GameObject[] RetrieveTileObjects(Vector3Int[] tilePos)
    {
        List<GameObject> foundTiles = new List<GameObject>();

        for (int i = 0; i < tilePos.Length; i++)
        {
            for(int j=0;j<tileObjects.Count;j++)
            {
                if(tileObjects[j].GetComponent<TData>().pos == tilePos[i])
                {
                    foundTiles.Add(tileObjects[j]);
                }
            }
        }

        return foundTiles.ToArray();
    }

    public TData FindTileData(Vector3Int tilePos)
    {
        for (int i = 0; i < tileObjects.Count; i++)
        {
            if (tileObjects[i].GetComponent<TData>().pos == tilePos)
            {
                return tileObjects[i].GetComponent<TData>();
            }
        }

        return new TData();
    }
        
    public List<TData> GetAllTilesOfType(TileTypes type)
    {
        List<TData> tiles = new List<TData>();
        for(int i = 0; i < tileObjects.Count; i++)
        {
            TData tile = tileObjects[i].GetComponent<TData>();
            if(tile!=null)
            {
                if(tile.tileType == type)
                {
                    tiles.Add(tile);
                }
            }
        }

        return tiles;
    }

    public List<TData> GetAllTileData()
    {
        List<TData> tiles = new List<TData>();
        for(int i=0;i<tileObjects.Count;i++)
        {
            tiles.Add(tileObjects[i].GetComponent<TData>());
        }

        return tiles;
    }

    public Vector2Int GetTargetCenter(TileTypes type, string owner=null)
    {
        List<Vector3Int> pos = new List<Vector3Int>();
        if(owner==null)
        {
            for(int i =0;i<tileObjects.Count;i++)
            {
                if(tileObjects[i].GetComponent<TData>().tileType == type)
                {
                    pos.Add(tileObjects[i].GetComponent<TData>().pos);
                }
            }
        }

        Vector3Int final = Vector3Int.zero;
        for (int i = 0; i < pos.Count; i++)
        {
            final += pos[i];
        }

        return new Vector2Int((final.x / pos.Count), final.y / pos.Count);
    }

    public TData GetOneTileOfType(TileTypes type, string owner=null)
    {
        for(int i=0;i<tileObjects.Count;i++)
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
}
