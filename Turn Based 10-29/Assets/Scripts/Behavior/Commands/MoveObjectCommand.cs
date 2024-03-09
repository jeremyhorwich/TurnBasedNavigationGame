using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObjectCommand : ICommand
{
    GameObject moveObject;
    Vector3 targetLocation;
    Vector3 origin;
    Dictionary<Vector3, Node> grid = GridManager.instance.grid;

    public MoveObjectCommand(GameObject _moveObject, Vector3 _targetLoc)
    {
        moveObject = _moveObject;
        targetLocation = Gridf.GetNode(_targetLoc).WorldPosition;
        origin = Gridf.GetNode(moveObject.transform.position).WorldPosition;
    }

    public void MoveObjectToNode(Vector3 _origin, Vector3 _target)
    {
        if (grid.ContainsKey(_origin))
            grid[_origin].SetCurrentObject(null);
        if (grid.ContainsKey(_target))
        {
            grid[_target].SetCurrentObject(moveObject);
            return;
        }

        moveObject.transform.position = _target + Vector3.up;
    }

    public void Execute()
    {
        MoveObjectToNode(origin, targetLocation);
    }

    public void Undo()
    {
        MoveObjectToNode(targetLocation, origin);
    }
}
