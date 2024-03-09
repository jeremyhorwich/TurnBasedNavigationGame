using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Actor
{
    private void OnEnable()
    {
        TurnManager.instance.OnNPCTurn += ChooseAction;
    }

    private void OnDisable()
    {
        TurnManager.instance.OnNPCTurn -= ChooseAction;
    }
}
