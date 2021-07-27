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
        savedData.sprite = new string[tiles.Length];

        savedData.tileNeighbor1 = new Vector3Int[tiles.Length];
        savedData.tileNeighbor2 = new Vector3Int[tiles.Length];
        savedData.tileNeighbor3 = new Vector3Int[tiles.Length];
        savedData.tileNeighbor4 = new Vector3Int[tiles.Length];
        savedData.tileNeighbor5 = new Vector3Int[tiles.Length];
        savedData.tileNeighbor6 = new Vector3Int[tiles.Length];
        savedData.tileNeighbor7 = new Vector3Int[tiles.Length];
        savedData.tileNeighbor8 = new Vector3Int[tiles.Length];

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

            savedData.tileNeighbor1[i] = cur.neighbors[0];
            savedData.tileNeighbor2[i] = cur.neighbors[1];
            savedData.tileNeighbor3[i] = cur.neighbors[2];
            savedData.tileNeighbor4[i] = cur.neighbors[3];
            savedData.tileNeighbor5[i] = cur.neighbors[4];
            savedData.tileNeighbor6[i] = cur.neighbors[5];
            savedData.tileNeighbor7[i] = cur.neighbors[6];
            savedData.tileNeighbor8[i] = cur.neighbors[7];

        }
        Debug.Log("<color=green>TileData saved</color>");

        //build all player data
        PlayerData pDate = singleton.pData;
        savedData.playerCurLoc = pDate.curLoc.pos;
        savedData.playerCurPath = pDate.curPath;
        savedData.playerCurSimLoc = pDate.curSimLoc;
        Debug.Log("<color=green>PlayerData saved</color>");

        //build active villager data
        VillagerData[] vData = gController.vController.GetActiveVillagers();
        savedData.vId = new string[vData.Length];
        savedData.vpos = new Vector2Int[vData.Length];
        savedData.vspeed = new int[vData.Length];
        savedData.visActive = new bool[vData.Length];
        savedData.visMoving = new bool[vData.Length];        
        savedData.vFName = new string[vData.Length];
        savedData.vLName = new string[vData.Length];
        savedData.vGender = new int[vData.Length];
        savedData.vhasJob = new bool[vData.Length];
        savedData.vjob = new int[vData.Length];
        savedData.vjobLoc = new Vector3Int[vData.Length];
        savedData.vjobID = new int[vData.Length];
        savedData.vatWork = new bool[vData.Length];
        savedData.vhasHome = new bool[vData.Length];
        savedData.vhomeLoc = new Vector3Int[vData.Length];
        savedData.vhomeID = new int[vData.Length];
        savedData.vatHome = new bool[vData.Length];
        savedData.vcurrentLocation = new Vector3Int[vData.Length];
        savedData.vgoingTo = new int[vData.Length];
        savedData.vtarget = new Vector3Int[vData.Length];
        savedData.vatLocation = new bool[vData.Length];
        savedData.vmood = new int[vData.Length];
        savedData.vAC = new int[vData.Length];
        savedData.vHP = new int[vData.Length];
        savedData.vAGI = new int[vData.Length];        

        for (int i = 0; i < vData.Length; i++)
        {
            savedData.vId[i] = vData[i].id;
            savedData.vpos[i] = vData[i].pos;
            savedData.vspeed[i] = vData[i].speed;
            savedData.visActive[i] = vData[i].isActive;
            savedData.visMoving[i] = vData[i].isMoving;            
            savedData.vFName[i] = vData[i].FName;
            savedData.vLName[i] = vData[i].LName;
            savedData.vGender[i] = vData[i].Gender;
            savedData.vhasJob[i] = vData[i].hasJob;
            savedData.vjob[i] = (int)vData[i].job;
            savedData.vjobLoc[i] = vData[i].jobLoc;
            savedData.vjobID[i] = vData[i].jobID;
            savedData.vatWork[i] = vData[i].atWork;
            savedData.vhasHome[i] = vData[i].hasHome;
            savedData.vhomeLoc[i] = vData[i].homeLoc;
            savedData.vhomeID[i] = vData[i].homeID;
            savedData.vatHome[i] = vData[i].atHome;
            savedData.vcurrentLocation[i] = vData[i].currentLocation.pos;
            savedData.vgoingTo[i] = (int)vData[i].goingTo;
            savedData.vcurrentPath = new Vector3Int[vData.Length];

            if (vData[i].target != null)
            {
                savedData.vtarget[i] = vData[i].target.pos;
            }
            savedData.vatLocation[i] = vData[i].atLocation;

            if (vData[i].currentPath.Length > 0)
            {
                savedData.vcurrentPath[i] = vData[i].currentPath[0];
            }

            for (int j = 0; j < vData[i].allowedTypes.Length; j++)
            {
                switch (j)
                {
                    case 0: savedData.vallowedTypes0 = (int)vData[i].allowedTypes[j]; break;
                    case 1: savedData.vallowedTypes1 = (int)vData[i].allowedTypes[j]; break;
                    case 2: savedData.vallowedTypes2 = (int)vData[i].allowedTypes[j]; break;
                    case 3: savedData.vallowedTypes3 = (int)vData[i].allowedTypes[j]; break;
                    case 4: savedData.vallowedTypes4 = (int)vData[i].allowedTypes[j]; break;
                    case 5: savedData.vallowedTypes5 = (int)vData[i].allowedTypes[j]; break;
                }
            }

            savedData.vmood[i] = vData[i].mood;
            savedData.vAC[i] = vData[i].AC;
            savedData.vHP[i] = vData[i].HP;
            savedData.vAGI[i] = vData[i].AGI;
        }
        Debug.Log("<color=green>VillagerData saved</color>");

        EnemyData[] eData = gController.eController.GetActiveEnemies();
        savedData.eid = new string[eData.Length];
        savedData.epos = new Vector2Int[eData.Length];
        savedData.eisMoving = new bool[eData.Length];
        savedData.eisActive = new bool[eData.Length];
        savedData.espeed = new int[eData.Length];
        savedData.eHP = new int[eData.Length];
        savedData.eAC = new int[eData.Length];
        savedData.eAgility = new int[eData.Length];
        savedData.efName = new string[eData.Length];
        savedData.elName = new string[eData.Length];
        savedData.egender = new int[eData.Length];
        savedData.eslowed = new bool[eData.Length];
        savedData.ecurrentLoc = new Vector3[eData.Length];        
        savedData.etargetTile = new Vector3Int[eData.Length];

        for (int i=0;i<eData.Length;i++)
        {
            savedData.eid[i] = eData[i].id;
            savedData.epos[i] = eData[i].pos;
            savedData.eisMoving[i] = eData[i].isMoving;
            savedData.eisActive[i] = eData[i].isActive;
            savedData.espeed[i] = eData[i].speed;
            savedData.eHP[i] = eData[i].HP;
            savedData.eAC[i] = eData[i].AC;
            savedData.eAgility[i] = eData[i].Agility;
            savedData.efName[i] = eData[i].fName;
            savedData.elName[i] = eData[i].lName;
            savedData.egender[i] = eData[i].gender;
            savedData.eslowed = new bool[eData.Length];
            savedData.ecurrentPath = new Vector3Int[eData.Length];
            savedData.ecurrentLoc[i] = eData[i].currentLoc.pos;
            if (eData[i].targetTile != null)
            {
                savedData.etargetTile[i] = eData[i].targetTile.pos;
            }

            for (int j = 0; j < eData[i].allowedTypes.Length; j++)
            {
                switch (j)
                {
                    case 0: savedData.eallowedTypes0 = (int)eData[i].allowedTypes[j]; break;
                    case 1: savedData.eallowedTypes1 = (int)eData[i].allowedTypes[j]; break;
                    case 2: savedData.eallowedTypes2 = (int)eData[i].allowedTypes[j]; break;
                    case 3: savedData.eallowedTypes3 = (int)eData[i].allowedTypes[j]; break;
                    case 4: savedData.eallowedTypes4 = (int)eData[i].allowedTypes[j]; break;
                    case 5: savedData.eallowedTypes5 = (int)eData[i].allowedTypes[j]; break;
                }
            }
            if (eData[i].currentPath.Length > 0)
            {
                savedData.ecurrentPath[i] = eData[i].currentPath[0];
            }
        }
        Debug.Log("<color=green>EnemyData saved</color>");

        string saved = JsonUtility.ToJson(savedData);
        string url = "D:/Projects/Unity/ThePhoenixRing/Assets/Resources/JSON/SimMaps/SimMap0101.txt";                
        //Write some text to the test.txt file
        StreamWriter writer = new StreamWriter(url, false);
        writer.WriteLine(saved);
        writer.Close();

        Debug.LogWarning("File Written");
    }
}
