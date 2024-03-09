using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rand = UnityEngine.Random;

public abstract class Actor : MonoBehaviour
{
	[SerializeField] private List<IAction>  actions = new List<IAction>();
    public List<string> EnemyTags { get; protected set; } = new List<string>();
	public List<string> AllyTags { get; protected set; } = new List<string>();

	[SerializeField] protected List<string> enemyTags;
	[SerializeField] protected List<string> allyTags;

	private void Awake()
	{
		GetComponents(actions);
		actions = actions.OrderBy(action => action.Priority).ToList();

		EnemyTags = enemyTags;
		AllyTags = allyTags;
	}

	public void ChooseAction()
	{
		//Find all actions in the highest priority bucket where an action condition is met

    		var possibleActions = new List<IAction>();

		for (int i = 0; i < actions.Count; i++)
		{
			if (actions[i].ActConditionIsMet) possibleActions.Add(actions[i]);
			if (i == actions.Count - 1) break;
			if (possibleActions.Count > 0 && actions[i].Priority < actions[i + 1].Priority) break;
		}

		if (possibleActions.Count == 0) return;

		//Choose a random action in the priority bucket and act on it

		int actionChoice = Rand.Range(0, possibleActions.Count);
		possibleActions[actionChoice].Act();
	}
	
}
