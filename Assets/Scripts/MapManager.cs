using System;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class MapManager : MonoBehaviour
{
    private List<GameObject> _mapFloors;
    public NavMeshData navMeshData;

    private void Awake()
    {
        if (navMeshData == null) throw new Exception("MapManager: Initial NavMeshData not assigned!");
        _mapFloors = new List<GameObject>();
        var mapObjCollection = GameObject.Find("MapObjectsCollection");
        if (mapObjCollection == null) throw new Exception("MapObjectsCollection not found!");
        foreach (Transform child in mapObjCollection.transform)
        {
            if (child.gameObject.CompareTag("Floor"))
            {
                _mapFloors.Add(child.gameObject);
            }
        }
    }

    private void Update()
    {
        UpdateMapNavMesh();
    }

    private void UpdateMapNavMesh()
    {
        foreach (var floor in _mapFloors)
        {
            if (floor.TryGetComponent(out NavMeshSurface surface))
            {
                surface.UpdateNavMesh(navMeshData);
            }
        }
    }
}
