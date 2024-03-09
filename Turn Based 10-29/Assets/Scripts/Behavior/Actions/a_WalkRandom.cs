using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class a_WalkRandom : MonoBehaviour, IAction
{
    public int Priority { get { return priority; } }
    [SerializeField] private int priority;
    private List<Node> walkableNeighbors { get
        {
            if (_walkableNeighbors == null || _walkableNeighbors.Count == 0)
                return _walkableNeighbors = Gridf.GetWalkableNeighbors(transform.position);
            return _walkableNeighbors;
        } }
    private List<Node> _walkableNeighbors;

    public bool ActConditionIsMet { get
        {
            return (walkableNeighbors.Count > 0);
        } }

    public void Act()
    {
        int randomChoice = Random.Range(0, walkableNeighbors.Count);

        ICommand randomMove = new MoveObjectCommand(gameObject, walkableNeighbors[randomChoice].WorldPosition);
        CommandManager.instance.SendCommand(randomMove);

        walkableNeighbors.Clear();
    }
}
