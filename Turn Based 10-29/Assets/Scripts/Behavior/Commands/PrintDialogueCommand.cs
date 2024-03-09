using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrintDialogueCommand : ICommand
{
	private string dialogue;

	public PrintDialogueCommand(string _dialogue)
	{
		dialogue = _dialogue;
	}

	public void Execute()
	{
		Debug.Log(dialogue);
		//In the future, connect to a UI element
	}

	public void Undo()
	{
		//Erase the dialogue from off the top of the dialogue box?
	}
}

