using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeHealthCommand : ICommand
{
	private Func<int, int> changeType;
	private GameObject target;

	private IDamageable healthComponent;
	private int healthBeforeChange;

	public ChangeHealthCommand(Func<int, int> _changeType, GameObject _target)
	{
		changeType = _changeType;
		target = _target;

		healthComponent = target.GetComponent<IDamageable>();
		healthBeforeChange = healthComponent.Health;
	}

	public void Execute()
	{
		healthComponent?.ChangeHealth(changeType);
	}

	public void Undo()
	{
		healthComponent?.ChangeHealth(health => healthBeforeChange);
	}

}
