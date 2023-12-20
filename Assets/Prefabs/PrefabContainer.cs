using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Prefabs
{
    [CreateAssetMenu(fileName = "PrefabContainer", menuName = "ScriptableObjects/PrefabContainer", order = 1)]
    public class PrefabContainer : ScriptableObject
    {
        public List<GameObject> prefabs;
    
        public bool TryGetByName(string objName, out GameObject prefab)
        {
            foreach (var p in prefabs.Where(p => p.name == objName))
            {
                prefab = p;
                return true;
            }

            prefab = null;
            return false;
        }
    }
}
