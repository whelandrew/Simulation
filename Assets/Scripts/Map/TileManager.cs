using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
public class TileManager : MonoBehaviour
{
    public bool finishedLoading = false;
    public Grid GridObject;
    private Tilemap[] tileMaps;
    public List<GameObject> tileObjects = new List<GameObject>();

    public Vector2Int MaxMapSize = new Vector2Int(50, 50);
    public Vector2Int MaxTileSize = new Vector2Int(16,16);

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
        
        for(int i=0;i<tileMaps.Length;i++)
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
        List<TileBase> tiles = new List<TileBase>();

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

                    newTileObject.name = tileData.name;
                    newTileObject.transform.position = tileData.pos;

                    BoxCollider2D newCollider = newTileObject.AddComponent<BoxCollider2D>();
                    newCollider.isTrigger = true;
                    newCollider.size = Vector2.one;
                    //newCollider.offset = new Vector2(.5f, .5f);

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
                }
            }
        }

        //continue proccess?
        int max = tMaps.Count;
        if (max-1 <= 0)
        {
            GridObject.gameObject.SetActive(false);
            finishedLoading = true;
            return;
        }
        else
        {
            tMaps.RemoveAt(max-1);
            BuildAllOtherTiles(tMaps);
        }
    }

    private TileTypes AssignTileType(TData tile)
    {
        string tileName = tile.name;
        if(tileName.Contains("Ground"))
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
            return TileTypes .Road;
        }
        if (tileName.Contains("TownCenter"))
        {
            return TileTypes .TownCenter;
        }
        if (tileName.Contains("Workshop"))
        {
            return TileTypes.Workshop;
        }

        return TileTypes.None;
    }   
}
