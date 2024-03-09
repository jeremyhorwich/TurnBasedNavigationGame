using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public Vector3 WorldPosition { get { return gameObject.transform.position; } }
    public ITerrainEffect terrainEffect;
    private GameObject currentObject;
    
    public bool IsWall => isWall;
    [SerializeField] private bool isWall;

    public Room ParentRoom { get { return transform.parent.parent.GetComponent<Room>(); } }
    public GameObject CurrentObject { get { return currentObject; } }
    public Vector3 nodeRight { get { return WorldPosition + Vector3.right; } }
    public Vector3 nodeForward { get { return WorldPosition + Vector3.forward; } }
    public Vector3 nodeLeft { get { return (WorldPosition + Vector3.left); } }
    public Vector3 nodeBack { get { return WorldPosition + Vector3.back; } }
    public List<Vector3> AllNeighborLocations { get { return new List<Vector3>() { nodeRight, nodeForward, nodeLeft, nodeBack }; } }

    public bool IsWalkableByObject(GameObject _gObject)
    {
        //Is the node already occupied by an object of higher priority?
        if (isWall) return false;
        if (currentObject != null)
        {
            if (_gObject.layer <= currentObject.layer) return false;
        }

        //Will the terrain on the node permit us to move?
        if (terrainEffect == null) return true;
        return terrainEffect.IsPassable(_gObject);
    }


    private void Awake()
    {
        terrainEffect = GetComponent<ITerrainEffect>();
    }

    private void OnEnable()
    {
        if (terrainEffect != null) terrainEffect.InitializeTerrain(this);
    }

    private void OnDisable()
    {
        if (terrainEffect != null) terrainEffect.DisableTerrain();
        currentObject = null;
    }

    public void SetCurrentObject(GameObject _currentObject)
    {
        currentObject = _currentObject;
        if (_currentObject != null) _currentObject.transform.position = WorldPosition + Vector3.up;
    }
}
