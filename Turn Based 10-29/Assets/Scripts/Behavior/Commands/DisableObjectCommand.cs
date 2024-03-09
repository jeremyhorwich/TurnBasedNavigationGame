using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableObjectCommand : ICommand
{
	private GameObject gObject;

	public DisableObjectCommand(GameObject _gObject)
	{
		gObject = _gObject;
	}

	public void Execute()
	{
		gObject.SetActive(false);
		if (Gridf.GetNode(gObject.transform.position) != null)
			Gridf.GetNode(gObject.transform.position).SetCurrentObject(null);
	}

    public void Undo()
	{
		gObject.SetActive(true);

		if (Gridf.GetNode(gObject.transform.position) != null)
			Gridf.GetNode(gObject.transform.position).SetCurrentObject(gObject);
	}
	//Add object destruction after undo limit has been reached
}
