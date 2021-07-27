using UnityEngine;
public class EnemyBehavior : MonoBehaviour
{
    public SimGameboardController gController;
    public EnemyData eData;
    public SpriteRenderer sprite;

    public bool canAttack;
    private bool canMove;

    private int pathVal = 0;

    public float curSpeed;
    public float maxSpeed;

    private bool freezeMovement;

    private void Start()
    {
        //disable after testing
        canMove = true;
        curSpeed = eData.speed;
        maxSpeed = eData.speed;
    }
    private void Update()
    {
        if (canMove)
        {
            if (eData.isActive)
            {
                Behaviors();

                //if (eData.currentPath.Length > 0)
                //{
                //    WalkPath();
                //}
            }
        }
    }

    /*
    IEnumerator ReactionTimer()
    {
        freezeMovement = true;
        yield return new WaitForSeconds(eData.Agility);
        freezeMovement = false;
    }
    */
    public void ChangeTarget(TData target)
    {
        if (!freezeMovement)
        {
            //StartCoroutine(ReactionTimer());

            pathVal = 0;
            TileTypes[] types = new TileTypes[eData.allowedTypes.Length + 2];
            for (int i = 0; i < eData.allowedTypes.Length; i++)
            {
                types[i] = eData.allowedTypes[i];
            }
            types[types.Length - 1] = target.tileType;
            types[types.Length - 2] = eData.currentLoc.tileType;

            eData.currentPath = gController.pathing.FindPath(eData.currentLoc, target, types);            
        }
    }

    public void TileTarget(TileTypes type)
    {
        if (!freezeMovement)
        {
            //StartCoroutine(ReactionTimer());

            pathVal = 0;
            //TData target = gController.tManager.GetOneTileOfType(type);
            TData target = new TData();

            TileTypes[] types = new TileTypes[eData.allowedTypes.Length + 2];
            for (int i = 0; i < eData.allowedTypes.Length; i++)
            {
                types[i] = eData.allowedTypes[i];
            }
            types[types.Length - 1] = target.tileType;
            types[types.Length - 2] = eData.currentLoc.tileType;

            eData.currentPath = gController.pathing.FindPath(eData.currentLoc, target, types);

            if(eData.currentPath == null)
            {
                //go towards player
                ChangeTarget(gController.pController.pData.curLoc);
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
        
        if(eData.currentPath != null && eData.currentPath.Length <1)
        {
            //default tile to move towards
            TileTarget(TileTypes.TownCenter);
        }

        //attack when target reached
        Attack();
        
        //if stop pathing if player atacks
        
    }

    void Attack()
    {
        if(canAttack)
        {

        }
    }

    public void ActivateEnemy()
    {
        gameObject.SetActive(true);
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