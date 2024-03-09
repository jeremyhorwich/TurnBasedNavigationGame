using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Actor))]
public class a_Follow : MonoBehaviour, IAction
{
	[SerializeField] private int priority;
    public int Priority { get { return priority; } }

	private List<string> enemyTags => _enemyTags ?? (_enemyTags = GetComponent<Actor>().EnemyTags);
	private List<string> _enemyTags;
	private List<List<Vector3>> validPaths { get
		{
			if (_validPaths == null || _validPaths.Count == 0)
				return _validPaths = GetValidPaths();
			return _validPaths;
		} }
	private List<List<Vector3>> _validPaths;

    private List<List<Vector3>> GetValidPaths()
    {
		var pathsToReturn = new List<List<Vector3>>();

		int characterLayer = 9;
		List<GameObject> allTargets = Roomf.GetAllActorsWithTag(enemyTags, characterLayer);

		var pathToCheck = new List<Vector3>();

		foreach (GameObject possibleTarget in allTargets)
        {
			pathToCheck = Gridf.GetPathToTarget(transform.position, possibleTarget.transform.position);
			if (pathToCheck != null)
				pathsToReturn.Add(pathToCheck);
        }

		return pathsToReturn;
    }

	//Do we have anywhere to go?
	public bool ActConditionIsMet { get
        {
			return validPaths.Count > 0;
		} }

	//Step towards the closest target
	public void Act()
    {
		var bestPath = new List<Vector3>();
		int leastPathCount = 100;
		int pathLength;

		foreach (List<Vector3> path in validPaths)
		{
			pathLength = path.Count;
			if (pathLength > leastPathCount) continue;
			if (pathLength == leastPathCount)
			{
				//If two targets have equal distance, pick one at random
				if (UnityEngine.Random.Range(0, 1) == 0) continue;
			}
			leastPathCount = pathLength;
			bestPath = path;
		}

		List<Node> walkableNeighbors = Gridf.GetWalkableNeighbors(transform.position);

		ICommand moveObjectOnPath = new MoveObjectCommand(gameObject, bestPath[bestPath.Count - 1]);
		CommandManager.instance.SendCommand(moveObjectOnPath);

		_validPaths.Clear();
	}
}
