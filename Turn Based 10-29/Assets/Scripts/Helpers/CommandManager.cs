using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandManager : MonoBehaviour
{
    public static CommandManager instance { get; private set; }

    public List<ICommand> phaseCommands { get; private set; } = new List<ICommand>();
    public List<ICommand> turnCommands { get; private set; } = new List<ICommand>();
    public Stack<ICommand[]> turnHistory { get; private set; } = new Stack<ICommand[]>();

    private void Awake()
    {
        //Protect Singleton
        if (instance != null && instance != this) Destroy(this);
        else instance = this;
    }


    public void SendCommand(ICommand command)
    {
        turnCommands.Add(command);
        command.Execute();
    }

    public void ClearCommandHistory()
    {
        turnHistory.Clear();
        turnCommands.Clear();
        phaseCommands.Clear();
    }

    public void CacheTurnCommands()
    {
        turnHistory.Push(turnCommands.ToArray());
        turnCommands.Clear();
    }

    public void UndoLastTurn()
    {
        if (turnHistory.Count == 0)
        {
            Debug.Log("No History in Undo Stack", this);
            return;
        }

        ICommand[] commandsToUndo = turnHistory.Pop();

        for (int i = commandsToUndo.Length - 1; i >= 0; i--)
        {
            commandsToUndo[i].Undo();
        }

        TurnManager.instance.RewindTurn();
    }
}
