using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class a_PlayerReloadRoom : MonoBehaviour, IAction
{
    public int Priority => 0;

    public bool ActConditionIsMet => Input.GetKeyDown(KeyCode.R);

    public void Act()
    {
        throw new System.NotImplementedException();
    }
}
