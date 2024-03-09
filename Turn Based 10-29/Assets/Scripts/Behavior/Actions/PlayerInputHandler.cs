using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    List<IAction> turnActions = new List<IAction>();

    private void Awake()
    {
        GetComponents(turnActions);
    }

    void Update()
    {
        //Other inputs, like pausing the game, etc...
        if (TurnManager.instance.CurrentPhase == Enums.TurnPhase.playerPhase) 
        {
            if (!Input.anyKeyDown) return;
            foreach(IAction action in turnActions)
                if (action.ActConditionIsMet)
                {
                    action.Act();
                    TurnManager.instance.EndPlayerPhase();
                }
        }
    }
}
