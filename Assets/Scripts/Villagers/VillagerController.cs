﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerController : MonoBehaviour
{
    public GameObject villagerCache;
    private List<GameObject> villagers = new List<GameObject>();

    public GameObject VillagerSpawnPoints;
    private BoxCollider2D[] spawnPoints;

    private void Start()
    {
        foreach(SpriteRenderer i in villagerCache.GetComponentsInChildren<SpriteRenderer>())
        {
            villagers.Add(i.gameObject);
        }

        spawnPoints = VillagerSpawnPoints.GetComponentsInChildren<BoxCollider2D>();

        //Remove after testing
        //CreateVillager();
        //////////////////////
    }

    public void Reset(int val)
    {
        VillagerData vData = villagers[val].GetComponent<VillagerData>();
        SpriteRenderer sr = villagers[val].GetComponent<SpriteRenderer>();

        vData.isActive = false;
        vData.FName = "";
        vData.LName = "";
        vData.id = "";

        sr.enabled = false;

        villagers[val].name = "Unused Villager";
    }

    Vector2 GetSpawnPoint()
    {        
        return spawnPoints[Random.Range(0, spawnPoints.Length - 1)].transform.position;
    }

    public void CreateVillager()
    {
        for(int i = 0; i < villagers.Count; i++) 
        {
            VillagerData vData = villagers[i].GetComponent<VillagerData>();
            SpriteRenderer sr = villagers[i].GetComponent<SpriteRenderer>();
            VillagerBehavior vB = villagers[i].GetComponent<VillagerBehavior>();
            if (!vData.isActive)
            {
                VillagerNames vNames = new VillagerNames();

                vData.isActive = true;
                vData.Gender = 0;

                vData.FName = vNames.GetRandomFirstName(vData.Gender);
                vData.LName = vNames.GetRandomLastName(vData.Gender);

                vData.id = i + vData.FName + vData.LName;

                sr.enabled = true;

                villagers[i].name = vData.id;
                villagers[i].transform.position = GetSpawnPoint();

                vData.pos = new Vector2Int((int)villagers[i].transform.position.x, (int)villagers[i].transform.position.y);

                vB.ActivateVillager();
                break;
            }
        }
    }
}