using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GridManager : MonoBehaviour
{
    public static GridManager instance { get; private set; }

    private List<Room> roomList = new List<Room>();
    private Room startingRoom;
    public Room CurrentRoom { get; private set; }

    public Dictionary<Vector3, Node> grid = new Dictionary<Vector3, Node>();    

    private void Awake()
    {
        //Protect Singleton
        if (instance != null && instance != this) Destroy(this);
        else instance = this;

        roomList = GetRoomList();
    }

    private void Start()
    {
        foreach (Room r in roomList)
            r.gameObject.SetActive(false);
        startingRoom = GetStartingRoom();
        LoadRoom(startingRoom);
    }

    private List<Room> GetRoomList()
    {        
        GameObject roomsObject = GameObject.Find("Rooms");

        List<Room> _roomList = new List<Room>();
        foreach (Transform child in roomsObject.transform)
            _roomList.Add(child.GetComponent<Room>());

        return _roomList;
    }

    private Room GetStartingRoom()
    {
        foreach (Room r in roomList)
        {
            if (CheckRoomForPlayer(r)) return r;
        }
        return roomList[0];
    }

    private bool CheckRoomForPlayer(Room _room)
    {
        GameObject player = GameObject.Find("Player");
        foreach (Node _node in _room.TerrainNodes)
        {
            if (player.transform.position == _node.WorldPosition + Vector3.up) 
                return true;
        }
        return false;
    }

    public void LoadRoom(Room _room)
    {
        if (CurrentRoom != null) UnloadRoom(CurrentRoom);

        CurrentRoom = _room;
        _room.gameObject.SetActive(true);
        
        foreach (Node tNode in _room.TerrainNodes)
            LoadNode(tNode);

        Gridf.UpdateGrid(grid);

        GameObject player = GameObject.Find("Player");
        LoadActorAtPosition(player,player.transform.position);
        foreach (KeyValuePair<GameObject, Vector3> actor in _room.actorLocations)
            LoadActorAtPosition(actor);

        GameObject camera = GameObject.Find("Main Camera");
        camera.transform.position = _room.CameraPosition;
    }

    private void LoadNode(Node _node)
    {
        grid.Add(_node.WorldPosition, _node);
    }

    private void LoadActorAtPosition(GameObject _gameObject, Vector3 _vector3)
    {
        LoadActorAtPosition(new KeyValuePair<GameObject, Vector3>(_gameObject, _vector3));
    }
    private void LoadActorAtPosition(KeyValuePair<GameObject,Vector3> actorPosition)
    {
        Node nodeToSet = Gridf.GetNode(actorPosition.Value);
        if (nodeToSet != null) nodeToSet.SetCurrentObject(actorPosition.Key);
    }

    private void UnloadRoom(Room _room)
    {
        foreach (Node node in grid.Values)
        {
            GameObject nodeObject = node.CurrentObject;
            if (nodeObject == null) continue;
            if (!nodeObject.CompareTag("Player") && !_room.actorLocations.ContainsKey(nodeObject))
                Destroy(nodeObject); 
        }
        grid.Clear();
        _room.gameObject.SetActive(false);
    }
}
