using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITerrainEffect
{
    void InitializeTerrain(Node node);
    void DisableTerrain();
    void OnTerrainPhase();
    bool IsPassable(GameObject _gameObject);
}
