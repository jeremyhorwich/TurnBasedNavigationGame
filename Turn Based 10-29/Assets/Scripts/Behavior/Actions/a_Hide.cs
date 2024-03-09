using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rand = UnityEngine.Random;

public class a_Hide : MonoBehaviour, IAction
{
    public int Priority => priority;
    [SerializeField] private int priority;

    [SerializeField] private List<string> tagsToHideFrom = new List<string>();
  
    private List<List<Vector3>> validPaths = new List<List<Vector3>>();

    //Are we already in a valid hiding spot, and if we aren't, do we have a valid path to a valid hiding spot?
    public bool ActConditionIsMet { get
        {
            bool objectIsNextToWall = false;
            Node currentNode = Gridf.GetNode(transform.position);

            foreach (Node n in Gridf.GetNeighbors(currentNode))
            {
                if (!n.IsWall) continue;
                objectIsNextToWall = true;
            }

            if (objectIsNextToWall && !Gridf.CanSeeUnitOfType(currentNode, tagsToHideFrom)) return false;

            validPaths.Clear();

            //Get list of possible hiding spots
            List<Node> wallNeighbors = GetAllWallNeighbors();
            List<Vector3> path;
            foreach (Node n in wallNeighbors)
            {
                if (!n.IsWalkableByObject(gameObject)) continue;
                if (Gridf.CanSeeUnitOfType(n, tagsToHideFrom)) continue;
                path = Gridf.GetPathToTarget(Gridf.GetNode(transform.position), n);
                if (path.Count > 0) validPaths.Add(path);
            }

            //Did we get any valid spots?
            return validPaths.Count > 0;
        } }

    public void Act()
    {
        //From possibles list, return the one with least distanced path (if more than one least path, choose one at random)
        int leastPathLength = 50;
        var leastPaths = new List<List<Vector3>>();
        foreach (List<Vector3> path in validPaths)
        {
            if (path.Count > leastPathLength) continue;

            leastPathLength = path.Count;
            leastPaths.Add(path);
        }

        List<Vector3> chosenPath = leastPaths[Rand.Range(0, leastPaths.Count)];

        //Tell the unit to move one square along that path
        ICommand moveObjectTowardsHidingSPot = new MoveObjectCommand(gameObject, chosenPath[chosenPath.Count - 1]);
        CommandManager.instance.SendCommand(moveObjectTowardsHidingSPot);
    }

    private List<Node> GetAllWallNeighbors()
    {
        var walls = new List<Node>();
        var wallNeighbors = new List<Node>();

        //Loop over all nodes in the grid and find walls 
        foreach (Node n in GridManager.instance.grid.Values)
        {
            if (!n.IsWall) continue;

            //For each neighbor, check that it isn't a wall and that it isn't already in the list
            foreach (Node neighbor in Gridf.GetNeighbors(n))
            {
                if (neighbor.IsWall) continue;
                if (!wallNeighbors.Contains(neighbor)) wallNeighbors.Add(neighbor);
            }
        }

        return wallNeighbors;
    }
}

