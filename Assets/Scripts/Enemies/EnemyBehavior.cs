using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemyBehavior : MonoBehaviour
{
    public GameboardController gController;
    public EnemyData eData;
    public SpriteRenderer sprite;

    private bool canMove;

    private int pathVal = 0;

    private void Start()
    {
        //disable after testing
        canMove = true;
    }
    private void Update()
    {
        if(eData.isActive && canMove)
        {
            Behaviors();

            if (eData.currentPath.Length > 0)
            {
                WalkPath();
            }
        }
    }

    private void Behaviors()
    {
        //use day/night schedule
        switch(gController.TOD) 
        {
            case Times.Morning: break;
            case Times.None: break;
            case Times.Night: break;            
        }

        
        //move towards towncenter
        if(eData.currentPath.Length <1)
        {
            pathVal = 0;
            TData target = gController.tManager.GetOneTileOfType(TileTypes.TownCenter);
            eData.currentPath = gController.pathing.FindPath(eData.currentLoc, target).ToArray();
        }
            //locate villager or building
            //move towards target
        //if none locate player
        //attack when target reached
        
        //if stop pathing if player atacks
        
    }

    public void ActivateEnemy()
    {
        eData.isActive = true;
        sprite.enabled = true;
    }

    private void WalkPath()
    {
        if (eData.currentPath.Length < pathVal)
            return;

        if (eData.currentPath.Length > 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, eData.currentPath[pathVal], eData.speed * Time.deltaTime);

            if (transform.position == eData.currentPath[pathVal])
            {
                pathVal++;

                if (pathVal == eData.currentPath.Length)
                {
                    pathVal = 0;
                    eData.isMoving = false;
                    eData.currentPath = new Vector3Int[0];
                }
            }
        }
    }
}