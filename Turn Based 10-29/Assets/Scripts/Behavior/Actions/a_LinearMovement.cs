using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class a_LinearMovement : MonoBehaviour, IAction
{
    [SerializeField] private int priority;
    public int Priority { get { return priority; } }

    private Vector3 moveDirection;

    private Vector3 moveTarget { get
        {
            return Vector3.right * transform.position.x + Vector3.forward * transform.position.z + moveDirection;
        } }

    private Dictionary<Vector3,Node> grid;

    private void OnEnable()
    {
        moveDirection = Vector3Int.RoundToInt(transform.forward);
        grid = GridManager.instance.grid;
    }

    //Are we able to move in the given direction
    public bool ActConditionIsMet { get
        {
            if (!grid.ContainsKey(moveTarget)) return false;
            return grid[moveTarget].IsWalkableByObject(gameObject);
        } }

    public void Act()
    {
        ICommand moveObject = new MoveObjectCommand(gameObject, moveTarget);
        CommandManager.instance.SendCommand(moveObject);
    }
}
