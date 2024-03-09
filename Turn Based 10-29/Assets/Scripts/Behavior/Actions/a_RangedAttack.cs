using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class a_RangedAttack : MonoBehaviour, IAction
{
    [SerializeField] private RangedWeapon shootWeapon;
    //These should all be tied to a weapon ScriptableObject eventually
    [SerializeField][Range(1,6)] private int shootRange;
    [SerializeField][Range(0,5)] private int fieldOfVision;
    [SerializeField][Range(0,10)] private int shootDelay;

    private int nextShootTurn;
    [SerializeField] int priority;
    public int Priority { get { return priority; } }

    private Vector3 shootDirection;

    private List<string> enemyTags => _enemyTags ?? (_enemyTags = GetComponent<Actor>().EnemyTags);
    private List<string> _enemyTags;

    private List<GameObject> validTargets { get
        {
            if (_validTargets == null || _validTargets.Count == 0)
                return _validTargets = GetValidTargets();
            return _validTargets;
        } }
    private List<GameObject> _validTargets;

    private List<GameObject> GetValidTargets()
    {
        //Get list of all possible targets
        int characterLayer = 9;
        List<GameObject> allPossibleTargets = Roomf.GetAllActorsWithTag(enemyTags, characterLayer);
        var targetsToReturn = new List<GameObject>();

        //Remove targets who are outside range or sight
        foreach (GameObject target in allPossibleTargets)
        {
            //Is the target in range?
            int _xDirectionToTarget = (int)(target.transform.position.x - transform.position.x);
            int _zDirectionToTarget = (int)(target.transform.position.z - transform.position.z);
            int _xDistanceToTarget = Mathf.Abs(_xDirectionToTarget);
            int _zDistanceToTarget = Mathf.Abs(_zDirectionToTarget);

            if (_xDistanceToTarget > fieldOfVision  && _zDistanceToTarget > fieldOfVision) continue;
            if (_xDistanceToTarget > shootRange || _zDistanceToTarget > shootRange) continue;

            //Is something in the way of us shooting?
            Vector3 farthestNodePosition;
            if (_xDistanceToTarget > _zDistanceToTarget)
            {
                farthestNodePosition = Gridf.GetFarthestNodeVisible(transform.position, Mathf.Sign(_xDirectionToTarget) * Vector3.right).WorldPosition;
                if (Mathf.Abs(farthestNodePosition.x - transform.position.x) > _xDistanceToTarget) continue;
            }
            else
            {
                farthestNodePosition = Gridf.GetFarthestNodeVisible(transform.position, Mathf.Sign(_zDirectionToTarget) * Vector3.forward).WorldPosition;
                if (Mathf.Abs(farthestNodePosition.z - transform.position.z) > _zDistanceToTarget) continue;
            }

            targetsToReturn.Add(target);
        }
        return targetsToReturn; 
    }

    private void OnEnable()
    {
        nextShootTurn = 0;
    }

    public void InitializeProjectile(GameObject projectileObject)
    {
        Projectile p = projectileObject.GetComponent<Projectile>();
    }

    //Can we see a target in our rough line of vision, and have we waited long enough between shots?
    public bool ActConditionIsMet { get
        {
            if (nextShootTurn > TurnManager.instance.CurrentTurnNumber) return false;
            return validTargets.Count > 0;
        } }

    //Shoot a projectile towards the most optimal target 
    public void Act()
    {
        //Find the valid target with the least distance

        GameObject bestTarget = null;
        float leastDistance = 100;
        float distanceToTarget;

        foreach (GameObject target in validTargets)
        {
            distanceToTarget = Vector3.Magnitude(transform.position - target.transform.position);
            if (distanceToTarget > leastDistance) continue;
            if (distanceToTarget == leastDistance)
            {
                //If two targets have equal distance, pick one at random
                if (UnityEngine.Random.Range(0, 1) == 0) continue;
            }
            leastDistance = distanceToTarget;
            bestTarget = target;
        }

        //Calculate the direction to shoot

        float relativePositionX = bestTarget.transform.position.x - transform.position.x;
        float relativePositionZ = bestTarget.transform.position.z - transform.position.z;
        if (Mathf.Abs(relativePositionX) > Mathf.Abs(relativePositionZ))
        {
            shootDirection = new Vector3(Mathf.Sign(relativePositionX), 0, 0);
        }
        else shootDirection = new Vector3(0, 0, Mathf.Sign(relativePositionZ));

        //Call the instantiate command with the valid position, rotation, and projectile initializer

        Action<GameObject> initializer = InitializeProjectile;
        ICommand shootAtTarget = new InstantiateObjectCommand(shootWeapon.ProjectileToShoot.gameObject, initializer, transform.position + shootDirection, Quaternion.LookRotation(shootDirection));
        CommandManager.instance.SendCommand(shootAtTarget);

        nextShootTurn = TurnManager.instance.CurrentTurnNumber + shootDelay;
        validTargets.Clear();
    }
}
