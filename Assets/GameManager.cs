using System;
using System.Collections;
using System.Collections.Generic;
using Prefabs;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PrefabContainer prefabContainer;
    public PrefabContainer PrefabContainer => prefabContainer;
    
    [SerializeField] private MapManager mapManager;
    public MapManager MapManager => mapManager;

    public ItemManager ItemManager { get; private set; }
    
    private void Awake()
    {
        if (prefabContainer == null)
        {
            throw new Exception("GameManager: PrefabContainer not assigned!");
        }
        if (mapManager == null)
        {
            throw new Exception("GameManager: MapManager not assigned!");
        }
        ItemManager = new ItemManager(prefabContainer);
    }
    
    public void AssignWeaponToCharacter(CharacterManager characterManager, WeaponName weaponName)
    {
        var weapon = ItemManager.GetWeaponReference(weaponName);
        characterManager.WeaponObject = Instantiate(weapon);
    }
}
