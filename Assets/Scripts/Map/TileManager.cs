using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
public class TileManager : MonoBehaviour
{
    public Grid GridObject;
    private Tilemap[] tileMaps;    
    private List<TData> tData = new List<TData>();
    private List<GameObject> tileObjects = new List<GameObject>();

    public Vector2Int MaxMapSize = new Vector2Int(50, 50);
    public Vector2Int MaxTileSize = new Vector2Int(16,16);

    private void Start()
    {
        tileMaps = GridObject.GetComponentsInChildren<Tilemap>();
        CreateMap();
    }

    private void Update()
    {

    }

    private void CreateMap()
    {
        InitializeBaseTileMap();
        InitializeBaseTileObjects();

        //Debug
        AssignParent();
        //
    }

    private void InitializeBaseTileMap()
    {
        for (int i = tileMaps.Length-1; i >=0; i--)
        {
            Tilemap tMap = tileMaps[i];
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
                        //make tData for all tiles
                        TData newTile = new TData();
                        newTile.pos = new Vector3Int(x - (MaxMapSize.x - 1), y - (MaxMapSize.y + 1), 0);
                        newTile.size = MaxTileSize;
                        newTile.name = (tMap.gameObject.name + tile.name + (x).ToString() + (y).ToString());
                        newTile.id = x + y;
                        newTile.tileType = AssignTileType(newTile);
                        
                        tData.Add(newTile);
                    }
                }
            }
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

    private void InitializeBaseTileObjects()
    {
        //create new gameobject for each tile
        for(int i=0;i<tData.Count;i++)
        {
            GameObject newTileObject = new GameObject();
            TData tileData = newTileObject.AddComponent<TData>();
            
            tileData = tData[i];
            newTileObject.name = tileData.name;
            newTileObject.transform.position = tileData.pos;            

            BoxCollider2D newCollider = newTileObject.AddComponent<BoxCollider2D>();
            newCollider.isTrigger = true;
            newCollider.size = Vector2.one;
            newCollider.offset = new Vector2(.5f, .5f);

            tileObjects.Add(newTileObject);
        }
    }

    /// DEBUG FEATURE
    /// ////////////
    /// Organize various tiletypes into their own gameobject parents (so that I don't go crazy)
    public Transform[] tileParents;
    void AssignParent()
    {
        foreach (GameObject tile in tileObjects)
        {
            TData tData = tile.GetComponent<TData>();
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
            }
        }
    }
}
