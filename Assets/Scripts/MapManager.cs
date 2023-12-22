using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class MapManager : MonoBehaviour
{
    private List<GameObject> _mapFloors;
    public NavMeshData navMeshData;

    private void Awake()
    {
        _mapFloors = new List<GameObject>();
        var map = GameObject.Find("MapObjectsCollection");
        if (map == null) throw new Exception("MapObjectsCollection not found!");
        foreach (Transform child in map.transform)
        {
            if (child.gameObject.CompareTag("Floor"))
            {
                _mapFloors.Add(child.gameObject);
            }
        }
    }

    void Update()
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
