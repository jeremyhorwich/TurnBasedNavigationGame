using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class Roomf
{
    public static List<GameObject> GetAllObjectsInRoom()
    {
        return GridManager.instance.CurrentRoom.actorLocations.Keys.ToList();
    }

    public static List<GameObject> GetAllActorsWithTag(List<string> tags, int layer)
    {
        List<GameObject> allActors = GetAllObjectsInRoom();
        var matchingActors = new List<GameObject>();

        foreach (GameObject actor in allActors)
        {
            if (actor.layer != layer) continue;
            foreach (string tag in tags)
            {
                if (!actor.CompareTag(tag)) continue;
                matchingActors.Add(actor);
                break;
            }
        }

        if (tags.Contains("Player")) matchingActors.Add(GameObject.Find("Player"));

        return matchingActors;
    }
}
