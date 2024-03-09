 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInputHandler))]
public class a_PlayerMovement : MonoBehaviour, IAction
{
    private Vector3 moveDirection { get
        {
            Vector3 _moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
            if (_moveDir == new Vector3(1, 0, 1)) _moveDir = new Vector3(1, 0, 0);      //Protect against diagonal movement
            return _moveDir;
        } }

    private Vector3 moveTarget { get
        {
            Vector3 _moveTar = transform.position + moveDirection;
            return Gridf.GetNode(_moveTar).WorldPosition;
        } }

    public int Priority { get { return 0; } }

    //Are we trying to move; and if we are, is it to a valid square?
    public bool ActConditionIsMet { get
        {
            if (moveDirection == Vector3.zero) return false;
            if (!GridManager.instance.grid.ContainsKey(moveTarget)) return false;
            return GridManager.instance.grid[moveTarget].IsWalkableByObject(gameObject);
        } }

    //Move based on the move direction
    public void Act()
    {
        ICommand command = new MoveObjectCommand(gameObject, moveTarget);
        CommandManager.instance.SendCommand(command);
    }
}
