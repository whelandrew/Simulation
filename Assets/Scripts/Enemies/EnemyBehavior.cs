using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemyBehavior : MonoBehaviour
{
    public GameboardController gController;
    public EnemyData eData;
    public SpriteRenderer sprite;
    //public CircleCollider2D targetCollider;

    private bool canMove;

    private int pathVal = 0;

    public float curSpeed;
    public float maxSpeed;

    private bool haltReaction;

    private void Start()
    {
        //disable after testing
        canMove = true;
        curSpeed = eData.speed;
        maxSpeed = eData.speed;
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

    IEnumerator ReactionTimer()
    {
        haltReaction = true;
        yield return new WaitForSeconds(eData.Agility);
        haltReaction = false;
    }

    public void ChangeTarget(TData target)
    {
        if (!haltReaction)
        {
            StartCoroutine(ReactionTimer());

            pathVal = 0;
            List<TileTypes> types = new List<TileTypes>();
            for (int i = 0; i < eData.allowedTypes.Length; i++)
            {
                types.Add(eData.allowedTypes[i]);
            }
            types.Add(target.tileType);

            //eData.currentPath = gController.pathing.FindPath(eData.currentLoc, target, types.ToArray());            
        }
    }

    public void DefaultTarget()
    {
        if (!haltReaction)
        {
            StartCoroutine(ReactionTimer());

            pathVal = 0;
            TData target = gController.tManager.GetOneTileOfType(TileTypes.TownCenter);

            List<TileTypes> types = new List<TileTypes>();
            for (int i = 0; i < eData.allowedTypes.Length; i++)
            {
                types.Add(eData.allowedTypes[i]);
            }
            types.Add(target.tileType);

            //eData.currentPath = gController.pathing.FindPath(eData.currentLoc, target, types.ToArray());
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
        if(eData.currentPath != null && eData.currentPath.Length <1)
        {
            DefaultTarget();
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
            transform.position = Vector3.MoveTowards(transform.position, eData.currentPath[pathVal], curSpeed * Time.deltaTime);

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