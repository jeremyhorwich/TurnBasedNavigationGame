using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour, ITerrainEffect
{
    public Node node { get; private set; }

    [SerializeField] private Node exitDestination;
    private Room exitRoom { get { return exitDestination.ParentRoom; } }
    
    public void OnTerrainPhase()
    {
        GameObject exitObject = node.CurrentObject;
        if (exitObject == null) return;

        exitObject.transform.position = exitDestination.WorldPosition;

        //TODO: The exit script isn't the right place for this call (should be part of UnloadRoom somehow) but I'm reluctant to have gridmanager see commandmanager
        CommandManager.instance.ClearCommandHistory();

        GridManager.instance.LoadRoom(exitRoom);
    }

    public void InitializeTerrain(Node _node)
    {
        node = _node;
        TurnManager.instance.OnExitCheck += OnTerrainPhase;
    }

    public void DisableTerrain()
    {
        TurnManager.instance.OnExitCheck -= OnTerrainPhase;
    }

    //This needs to be deprecated in favor of returning not walkable for non-players
    public bool IsPassable(GameObject _gObject)
    {
        return _gObject.CompareTag("Player");
    }
}
