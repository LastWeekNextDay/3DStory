using Prefabs;
using UnityEngine;

public enum WeaponName
{
    DefaultSword
}

public class ItemManager
{
    private readonly PrefabContainer _prefabContainer;
    
    public ItemManager(PrefabContainer prefabContainer)
    {
        if (prefabContainer == null)
        {
            throw new System.Exception("Item Manager: PrefabContainer not assigned!");
        }
        _prefabContainer = prefabContainer;
    }
    
    public GameObject GetWeaponReference(WeaponName weaponName)
    {
        if (_prefabContainer.TryGetByName(weaponName.ToString(), out var weapon))
        {
            return weapon;
        }
        throw new System.Exception("Weapon not found!");
    }
}
