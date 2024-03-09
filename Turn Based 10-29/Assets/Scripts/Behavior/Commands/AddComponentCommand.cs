using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddComponentCommand : ICommand
{
	private Component componentToAdd;
	private GameObject objToCompose;

	private Component createdComponent;

	public AddComponentCommand(Component _componentToAdd, GameObject _objToCompose)
	{
		componentToAdd = _componentToAdd;
		objToCompose = _objToCompose;
	}

	public void Execute()
	{
		createdComponent = objToCompose.AddComponent(componentToAdd.GetType());
	}

	public void Undo()
	{
		MonoBehaviour.Destroy(createdComponent);
	}
}

