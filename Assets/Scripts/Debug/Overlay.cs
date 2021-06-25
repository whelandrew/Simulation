using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;
using System;

//does not go into actual game build
public class Overlay : MonoBehaviour
{
    Camera main;
    public GameObject MapPieces;

    public Text tileInfoText;
    private bool tileManagerReady;

    public GameboardController gController;
    public TileManager tileManager;
    public SpriteSelector sSelector;
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
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (tileManagerReady)
        {
            GetTileInfo();
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
        List<GameObject> tileObjects = tileManager.tileObjects;

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
                            if (t.gameObject.name.Contains("House")) tile.transform.parent = t;
                        break;
                }
            }
        }
    }
}
