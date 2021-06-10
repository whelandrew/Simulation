using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
public class TileManager : MonoBehaviour
{
    public Grid GridObject;
    private Tilemap[] tileMaps;
    private TData[] tData;

    // Start is called before the first frame update
    void Start()
    {
        tileMaps = GridObject.GetComponentsInChildren<Tilemap>();
        InitializeTileMap(tileMaps[tileMaps.Length-1]);
        //InitializeTileMap(tileMaps[tileMaps.Length - 2]);
    }

    // Update is called once per frame
    void Update()
    {

    }

    //TODO get tile data
    private void InitializeTileMap(Tilemap tMap)
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
                    Debug.Log("x: " + x + " y: " + y +  " tile: " + tile);
                    //tiles.Add(tile);
                }
            }
        }

        //UpdateTData(tiles);
    }

    private void UpdateTData(List<TileBase> tile)
    {
        Debug.Log("UpdateTData");
        for (int i = 0; i < tile.Count; i++)
        {            
            
        }
        
    }

    public void GetTileInfo(Tilemap[] maps)
    {
        /*
        for(int i=0;i<maps.Length;i++)
        {
            Tilemap curMap = maps[i];
            Debug.Log(curMap);
            curMap
        }
        */
    }
}
