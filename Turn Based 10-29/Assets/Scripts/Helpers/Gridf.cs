using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Gridf
{
    private static Dictionary<Vector3, Node> grid = new Dictionary<Vector3, Node>();

    public static void UpdateGrid(Dictionary<Vector3,Node> _grid)
    {
        grid = _grid;
    }

    public static Node GetNode(Vector3 _pos)
    {
        _pos = GetGridPosition(_pos);
        if (!grid.ContainsKey(_pos)) return null;
        return grid[_pos];
    }

    private static Vector3 GetGridPosition(Vector3 pos)
    {
        Vector3 projectedPos = pos.x * Vector3.right + pos.z * Vector3.forward;
        projectedPos = Vector3Int.RoundToInt(projectedPos);
        for (int i = (int)pos.y; i >= 0; i--)
            if (grid.ContainsKey(projectedPos + Vector3.up * i)) return projectedPos + Vector3.up * i;
        return projectedPos;
    }

    public static  List<Node> GetWalkableNeighbors(Vector3 location)
    {
        List<Node> walkableNeighbors = GetNeighbors(location);
        foreach (Node neighbor in walkableNeighbors.ToArray())
            if (!neighbor.IsWalkableByObject(GetNode(location).CurrentObject)) walkableNeighbors.Remove(neighbor);
        return walkableNeighbors;

    }

    public static List<Node> GetWalkableNeighbors(Node node) => GetWalkableNeighbors(node.WorldPosition);

    public static List<Node> GetNeighbors(Vector3 location)
    {
        location = GetGridPosition(location);

        var neighbors = new List<Node>();
        var neighborLoc = new Vector3();
        List<Vector3> neighborLocationUnrounded = grid[location].AllNeighborLocations;
        foreach (Vector3 neighbor in neighborLocationUnrounded)
        {
            neighborLoc = Vector3Int.RoundToInt(neighbor);
            if (grid.ContainsKey(neighborLoc)) neighbors.Add(grid[neighborLoc]);
        }
        return neighbors;
    }

    public static List<Node> GetNeighbors(Node node) => GetNeighbors(node.WorldPosition);

    public static List<Vector3> GetPathToTarget(Node origin, Node target) =>
        GetPathToTarget(origin.WorldPosition, target.WorldPosition);

    public static List<Vector3> GetPathToTarget(Vector3 origin, Vector3 target)
    {
        origin = GetGridPosition(origin);
        target = GetGridPosition(target);

        GameObject originObject = grid[origin].CurrentObject;

        //A* algorithm


        /* Declare lists and dictionaries. Using dictionaries instead of adding these
         * properties to the node class because nodes won't need to use these values
         * outside of the pathfinding method. This probably breaks a coding principle
         * but I don't have the knowledge of how to implement a cleaner way */

        List<Vector3> closedLocations = new List<Vector3>() { origin };
        List<Vector3> openLocations = new List<Vector3>();
        Dictionary<Vector3, Vector3> parentLocation = new Dictionary<Vector3, Vector3>();
        Dictionary<Vector3, int> storedFCost = new Dictionary<Vector3, int>();

        //Populate and initialize
        storedFCost[origin] = 0;
        List<Node> startingNeighbors = GetWalkableNeighbors(origin);

        //A little randomness built in so that (a) target doesn't get stuck between object and wall (b) behavior isn't the same every time
        int indexPush = Random.Range(0, startingNeighbors.Count);
        for (int i = 0; i < startingNeighbors.Count; i++)
        {
            int j = i;
            j = (j + indexPush) % startingNeighbors.Count;

            openLocations.Add(startingNeighbors[j].WorldPosition);
            SetParentLocation(startingNeighbors[j].WorldPosition, origin);
        }

        bool targetFound = false;

        //Execute the algorithm
        while (!targetFound)
        {
            if (openLocations.Count == 0) return null;          //Check if we think a path is still possible
            FindNextBestNode();
            if (closedLocations[closedLocations.Count - 1] == target) targetFound = true;
        }
        return MakePath();

        //Helper functions

        void FindNextBestNode()
        {
            int leastHCost = 50;        //should improve what we set max bounds to be
            int gCostOfLeast = 50;
            Vector3 leastCostLocation = Vector3.zero;


            //find best open node
            foreach (Vector3 location in openLocations)
            {
                if (hCost(location) > leastHCost) continue;
                if ((hCost(location) == leastHCost) && gCost(location) >= gCostOfLeast) continue;
                leastHCost = hCost(location);
                gCostOfLeast = gCost(location);
                leastCostLocation = location;
            }

            //add the best node's neighbors to open nodes, add the best node to closed node
            closedLocations.Add(leastCostLocation);
            openLocations.Remove(leastCostLocation);

            if (leastCostLocation == target) return;            //we found the target

            foreach (Node neighbor in GetNeighbors(leastCostLocation))
            {
                if (neighbor.WorldPosition == target)
                {
                    openLocations.Clear();
                    openLocations.Add(target);
                    SetParentLocation(target, leastCostLocation);
                    break;
                }
                if (!neighbor.IsWalkableByObject(originObject)) continue;
                if (closedLocations.Contains(neighbor.WorldPosition)) continue;
                if (openLocations.Contains(neighbor.WorldPosition))
                {
                    if (storedFCost[leastCostLocation] + 1 < storedFCost[neighbor.WorldPosition])
                        SetParentLocation(neighbor.WorldPosition, leastCostLocation);
                    continue;
                }
                openLocations.Add(neighbor.WorldPosition);
                SetParentLocation(neighbor.WorldPosition, leastCostLocation);
            }
        }

        List<Vector3> MakePath()
        {
            Vector3 currentLocation = closedLocations[closedLocations.Count - 1];
            List<Vector3> path = new List<Vector3>() { currentLocation };

            bool pathComplete = (parentLocation[currentLocation] == origin);        //This will check if we're done already

            while (!pathComplete)
            {
                //add parent node to path
                currentLocation = parentLocation[currentLocation];
                path.Add(currentLocation);

                //check if we're done
                if (parentLocation[currentLocation] == origin) pathComplete = true;
            }
            return path;
        }

        void SetParentLocation(Vector3 _location, Vector3 _parentlocation)
        {
            if (!parentLocation.ContainsKey(_location)) parentLocation.Add(_location, _parentlocation);
            else parentLocation[_location] = _parentlocation;
            storedFCost[_location] = fCost(_location);
        }

        int fCost(Vector3 location) { return 1 + storedFCost[parentLocation[location]]; }   //f cost is 1 + what the parent node's was, by definition

        int gCost(Vector3 location) { return GetDistance(location, target); }

        int hCost(Vector3 location) { return fCost(location) + gCost(location); }

        int GetDistance(Vector3 _origin, Vector3 _target)
        {
            return (int)(Mathf.Abs(_target.x - _origin.x) + Mathf.Abs(_target.y - _origin.y) + Mathf.Abs(_target.z - _origin.z));
        }
    }

    public static Node GetFarthestNodeVisible(Vector3 origin, Vector3 direction) =>
        GetFarthestNodeVisible(Gridf.GetNode(origin), direction);

    public static Node GetFarthestNodeVisible(Node origin, Vector3 direction) => 
        GetFarthestNodeVisible(origin, direction, new List<int> { 9 });

    public static Node GetFarthestNodeVisible(Node origin, Vector3 direction, List<string> blockerLayerNames)
    {
        var blockerLayers = new List<int>();
        foreach (string name in blockerLayerNames)
            blockerLayers.Add(LayerMask.NameToLayer(name));
        return GetFarthestNodeVisible(origin, direction, blockerLayers);
    }

    public static Node GetFarthestNodeVisible(Node origin, Vector3 direction, List<int> blockerLayers)
    {
        Node currentNodeCheck = origin;
        Node nextNodeToCheck = GetNode(origin.WorldPosition + direction);
        while (nextNodeToCheck != null)
        {
            currentNodeCheck = nextNodeToCheck;
            nextNodeToCheck = GetNode(nextNodeToCheck.WorldPosition + direction);

            if (currentNodeCheck.IsWall) return currentNodeCheck;

            foreach (int layer in blockerLayers)
                if (currentNodeCheck.CurrentObject?.layer == layer) return currentNodeCheck;
        }

        //If we can see all the way to the edge of the grid, return the node that lies on the edge
        return currentNodeCheck;
    }

    public static bool CanSeeUnitOfType(Node origin, List<string> typeTags)
    {
        //Hiding spots are valid if we can't see any enemies

        //If we get blocked by a wall in a direction, we don't check beyond that wall in future rows. That information is stored in this dictionary.
        var farthestNodes = new Dictionary<Vector3, Node>
        {
            {Vector3.left, GetFarthestNodeVisible(origin, Vector3.left)},
            {Vector3.right, GetFarthestNodeVisible(origin, Vector3.right)},
            {Vector3.forward, GetFarthestNodeVisible(origin, Vector3.forward)},
            {Vector3.back, GetFarthestNodeVisible(origin, Vector3.back)}
        };

        if (!CheckIfDirectionValid(Vector3.forward)) return true;
        return !CheckIfDirectionValid(Vector3.back);

        bool CheckIfDirectionValid(Vector3 yDirection)
        {
            foreach (string tag in typeTags)
                if (farthestNodes[yDirection].CurrentObject?.CompareTag(tag) == true) return false;

            bool allRowsChecked = false; 
            Node nextRowToCheck = origin;

            //Minor algorithm inefficiency here, we are checking the origin row twice (once for forward and back apiece),
            //since CheckIfDirectionValid Gets called twice
            while (!allRowsChecked)
            {
                if (!CheckIfRowValid(nextRowToCheck, Vector3.left)) return false;
                if (!CheckIfRowValid(nextRowToCheck, Vector3.right)) return false;

                if (nextRowToCheck == farthestNodes[yDirection]) allRowsChecked = true;
                nextRowToCheck = GetNode(nextRowToCheck.WorldPosition + yDirection);
            }

            return true;
        }

        bool CheckIfRowValid(Node rowNode, Vector3 xDirection)
        {
            Node farthestNodeInRow = GetFarthestNodeVisible(rowNode, xDirection);

            int xDistanceToFarthestNodeInRow = (int)Mathf.Abs(origin.WorldPosition.x - farthestNodeInRow.WorldPosition.x);
            int xDistanceToFarthestNodeInDirection = (int)Mathf.Abs(origin.WorldPosition.x - farthestNodes[xDirection].WorldPosition.x);

            if (xDistanceToFarthestNodeInRow > xDistanceToFarthestNodeInDirection) return true;
            foreach (string tag in typeTags)
                if (farthestNodeInRow.CurrentObject?.CompareTag(tag) == true) return false;

            farthestNodes[xDirection] = farthestNodeInRow;
            return true;
        }

        //Future optimizations on this algorithm: check if we should do horizontal -> vertical or vertical -> horizontal
    }


}
