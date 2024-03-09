using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    int Health { get; }
    void ChangeHealth(Func<int,int> changeType);
    void MakeDead();
}
