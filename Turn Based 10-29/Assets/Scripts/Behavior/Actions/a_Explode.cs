using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class a_Explode : MonoBehaviour, IAction
{
	[SerializeField] private int damageAmount;
	[SerializeField] private int priority;

	protected Vector3 targetDir;
	private Vector3 target { get { return targetDir + transform.position; } }
	private GameObject targetObject { get { return Gridf.GetNode(target).CurrentObject; } }

	public int Priority { get { return priority; } }

    public bool ActConditionIsMet { get 
		{
			if (Gridf.GetNode(target) == null) return true;
			return (!Gridf.GetNode(target).IsWalkableByObject(gameObject) && targetObject != gameObject); 
		} }

    public void Act()
	{
		//TODO: Because disableObject changes the node's object, we have to deal damage before disabling arrow. 
		//Node should be changed to eliminate these and other resulting problems
		
		if (Gridf.GetNode(target) != null && (targetObject?.GetComponent<IDamageable>() != null))
        {
			ICommand dealDamage = new ChangeHealthCommand(health => health - damageAmount, targetObject);
			CommandManager.instance.SendCommand(dealDamage);
		}

		ICommand disableObject = new DisableObjectCommand(gameObject);
		CommandManager.instance.SendCommand(disableObject);
	}
}
