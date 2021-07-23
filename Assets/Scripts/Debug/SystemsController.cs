using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
public class SystemsController : MonoBehaviour
{
    public TileManager tManager;
    public SimGameboardController gController;
    public Singleton singleton;

    public Button saveButton;
    public void SaveMap()
    {
        GetSaveData();
    }

    private void GetSaveData()
    {
        //build all Tile Data        
        TData[] tiles = tManager.GetAllTileData();        
        LoadedData savedData = new LoadedData();
        savedData.tileId = new int[tiles.Length];
        savedData.tileType = new int[tiles.Length];
        savedData.tileLayer = new int[tiles.Length];
        savedData.tileName = new string[tiles.Length];
        savedData.tilePos = new Vector3Int[tiles.Length];
        savedData.tileSize = new Vector2Int[tiles.Length];
        savedData.tileHP = new int[tiles.Length];
        savedData.tileAC = new int[tiles.Length];
        savedData.tileFootTraffic = new int[tiles.Length];
        savedData.tileCanInterract = new bool[tiles.Length];
        savedData.tileIsBuilding = new bool[tiles.Length];
        savedData.tileOwned = new bool[tiles.Length];
        savedData.tileNeighbors = new Vector3Int[tiles.Length][];
        savedData.sprite = new string[tiles.Length];
            
        for (int i = 0; i < tiles.Length; i++)
        {
            TData cur = tiles[i];
            savedData.tileId[i] = cur.id;
            savedData.tileType[i] = (int)cur.tileType;
            savedData.tileLayer[i] = (int)cur.tileLayer;
            savedData.tileName[i] = cur.name;
            savedData.tilePos[i] = cur.pos;
            savedData.tileSize[i] = cur.size;
            savedData.tileHP[i] = cur.HP;
            savedData.tileAC[i] = cur.AC;
            savedData.tileFootTraffic[i] = cur.footTraffic;            
            savedData.tileCanInterract[i] = cur.canInterract;
            savedData.tileIsBuilding[i] = cur.isBuilding;
            savedData.tileOwned[i] = cur.owned;
            savedData.sprite[i] = cur.sprite;

            savedData.tileNeighbors[i] = new Vector3Int[cur.neighbors.Length];
            for(int j=0; j<cur.neighbors.Length;j++)
            {
                savedData.tileNeighbors[i][j] = cur.neighbors[j];
            }
        }

        Debug.Log("<color=green>TileData saved</color>");


        string saved = JsonUtility.ToJson(savedData);
        string url = "D:/Projects/Unity/ThePhoenixRing/Assets/Resources/JSON/SimMaps/SimMap0101.txt";        
        

        //Write some text to the test.txt file
        StreamWriter writer = new StreamWriter(url, false);
        writer.WriteLine(saved);
        writer.Close();

        Debug.LogWarning("File Written");
    }
}
