using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class a_DisplayDialogue : MonoBehaviour, IAction
{
    public static event Action<string> OnUpdateDialogueDisplay;

    [SerializeField] private int priority;
    public int Priority { get { return priority; } }

    [SerializeField] private string dialogue;

    //Is the player within an adjacent Node?
    public bool ActConditionIsMet { get
        {
            List<Node> neighbors = Gridf.GetNeighbors(transform.position);
            foreach (Node n in neighbors)
            {
                if (n.CurrentObject == null) continue;
                if (n.CurrentObject.CompareTag("Player")) return true;
            }
            return false;
        } }

    public void Act()
    {
        OnUpdateDialogueDisplay?.Invoke(dialogue);
    }
}
