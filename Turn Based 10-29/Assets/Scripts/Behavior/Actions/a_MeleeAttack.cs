using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Actor))]
public class a_MeleeAttack : MonoBehaviour, IAction
{
    [SerializeField] private int damageAmount;      //May want to tie this to a weapon class eventually
    [SerializeField] private int priority;
    public int Priority { get { return priority; } }

    private List<string> enemyTags => _enemyTags ?? (_enemyTags = GetComponent<Actor>().EnemyTags);
    private List<string> _enemyTags;

    private List<GameObject> targetsWithinRange = new List<GameObject>();

    //Are there any valid targets within range?
    public bool ActConditionIsMet { get 
        {
            foreach (Node neighbor in Gridf.GetNeighbors(transform.position))
            {
                GameObject neighborObject = neighbor.CurrentObject;
                foreach (string tag in enemyTags)
                {
                    if (neighborObject == null) continue;
                    if (!neighborObject.CompareTag(tag)) continue;

                    targetsWithinRange.Add(neighborObject);
                    break;
                }
            }
            return targetsWithinRange.Count > 0; 
        } }

    //Attack the player and deal damage
    public void Act()
    {
        //Play the weapon animation, etc.

        GameObject target = targetsWithinRange[Random.Range(0, targetsWithinRange.Count)];
        targetsWithinRange.Clear();

        ICommand dealDamage = new ChangeHealthCommand(targetHealth => targetHealth - damageAmount, target);
        CommandManager.instance.SendCommand(dealDamage);
    }
}
