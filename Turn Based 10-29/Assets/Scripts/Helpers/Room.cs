using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Room : MonoBehaviour
{
	public List<Node> TerrainNodes { get; private set; } = new List<Node>();
	public Dictionary<GameObject, Vector3> DefaultActors { get; private set; } = new Dictionary<GameObject, Vector3>();
	public Vector3 CameraPosition { get { return cameraPosition; } }
	[SerializeField] private Vector3 cameraPosition;

    public Dictionary<GameObject, Vector3> actorLocations { get; private set; } = new Dictionary<GameObject, Vector3>();
	public bool actorOverrideExists { get; private set; }


    private void Awake()
    {
		TerrainNodes.Clear();
		actorLocations.Clear();
		DefaultActors.Clear();

		Transform terrainData;
		Transform actorData;
		terrainData = transform.Find("TerrainData");
		foreach (Transform child in terrainData)
			TerrainNodes.Add(child.GetComponent<Node>());
		actorData = transform.Find("ActorData");
		foreach (Transform child in actorData)
		{
			DefaultActors.Add(child.gameObject, child.position);
			actorLocations.Add(child.gameObject, child.position);
		}
	}

    public void GetRoomData()
	{

	}

	//Will probably need to recreate some version of the below methods for terrain overrides

	public void AddActorToOverride(GameObject actor)
	{
		foreach (KeyValuePair<GameObject, Vector3> a in actorLocations)
			if (a.Value == actor.transform.position)
			{
				Debug.Log("Couldn't add " + actor.ToString() + ": room already has actor at position");
				return;
			}

		actorLocations.Add(actor, actor.transform.position);
		actorOverrideExists = true;
	}

    public void RemoveActorFromOverride(GameObject actor)
	{
		if (!actorLocations.ContainsKey(actor)) return;

		actorLocations.Remove(actor);
		actorOverrideExists = true;
	}

	public void OverrideActorPosition(GameObject actor)
	{
		if (!actorLocations.ContainsKey(actor)) return;

		actorLocations[actor] = actor.transform.position;
		actorOverrideExists = true;
	}

	public void ResetActorOverride()
	{
		actorLocations.Clear();
		foreach (KeyValuePair<GameObject, Vector3> a in DefaultActors)
			actorLocations.Add(a.Key, a.Value);

		actorOverrideExists = false;
	}
}
