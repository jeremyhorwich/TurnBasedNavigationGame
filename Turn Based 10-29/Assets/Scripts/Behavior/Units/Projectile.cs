using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Actor
{
    private int turnEnabled;

    private void OnEnable()
    {
        turnEnabled = TurnManager.instance.CurrentTurnNumber;
        TurnManager.instance.OnProjectileTurn += CheckIfShouldChooseAction;
    }

    private void OnDisable()
    {
        TurnManager.instance.OnProjectileTurn -= ChooseAction;
        TurnManager.instance.OnProjectileTurn -= CheckIfShouldChooseAction;
    }
    
    void CheckIfShouldChooseAction()
    {
        if (!(TurnManager.instance.CurrentTurnNumber > turnEnabled)) return;
        TurnManager.instance.OnProjectileTurn -= CheckIfShouldChooseAction;
        TurnManager.instance.OnProjectileTurn += ChooseAction;
        ChooseAction();
    }
}
