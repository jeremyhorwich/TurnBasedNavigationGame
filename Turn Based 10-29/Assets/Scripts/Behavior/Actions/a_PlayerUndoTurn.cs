using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class a_PlayerUndoTurn : MonoBehaviour
{
    private Action UndoLastTurn;

    private void Awake()
    {
        UndoLastTurn = CommandManager.instance.UndoLastTurn;
    }

    private void Update()
    {
        if (TurnManager.instance.CurrentPhase != Enums.TurnPhase.playerPhase) return;
        if (Input.GetKeyDown(KeyCode.Backspace))
            UndoLastTurn();
    }
}
