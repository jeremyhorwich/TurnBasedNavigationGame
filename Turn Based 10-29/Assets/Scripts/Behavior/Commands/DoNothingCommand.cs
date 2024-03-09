using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoNothingCommand : ICommand
{
    public void Execute()
    {
        return;
    }

    public void Undo()
    {
        return;
    }
}
