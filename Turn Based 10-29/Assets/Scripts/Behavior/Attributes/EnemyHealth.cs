using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    private int health = 1;
    public int Health => health;

    public void ChangeHealth(Func<int, int> changeType)
    {
        if (changeType(health) < health) MakeDead();
    }

    public void MakeDead()
    {
        ICommand KillUnitCommand = new DisableObjectCommand(gameObject);
        CommandManager.instance.SendCommand(KillUnitCommand);
    }
}
