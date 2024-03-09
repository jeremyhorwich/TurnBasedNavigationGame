using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateObjectCommand : ICommand
{
    private GameObject objectToInstantiate;
    private Action<GameObject> initializeObjectValues;
    private Vector3 position;
    private Quaternion rotation;

    private GameObject objectInstance;
    
    public InstantiateObjectCommand(GameObject _objectToInstantiate, Action<GameObject> _initializeObjectValues, Vector3 _position, Quaternion _rotation)
    {
        objectToInstantiate = _objectToInstantiate;
        initializeObjectValues = _initializeObjectValues;
        position = _position;
        rotation = _rotation;
    }

    public void Execute()
    {
        objectInstance = MonoBehaviour.Instantiate(objectToInstantiate, position, rotation);
        initializeObjectValues(objectInstance);
    }

    public void Undo()
    {
        MonoBehaviour.Destroy(objectInstance);
    }
}
