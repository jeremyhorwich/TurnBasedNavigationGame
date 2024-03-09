using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Actor
{

    private void OnEnable()
    {
        TurnManager.instance.OnEnemyTurn += ChooseAction;
    }

    private void OnDisable()
    {
        TurnManager.instance.OnEnemyTurn -= ChooseAction;
    }
}
