using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class a_PlayerSkipTurn : MonoBehaviour, IAction
{
    public int Priority { get { return 0; } }

    public bool ActConditionIsMet { get
        {
            return (Input.GetKeyDown(KeyCode.Space));
        } }

    public void Act()
    {
        ICommand skipTurn = new DoNothingCommand();
        CommandManager.instance.SendCommand(skipTurn);
        return;
    }
}
