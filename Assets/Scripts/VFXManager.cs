using System.Collections;
using Prefabs;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    [SerializeField] private PrefabContainer _prefabContainer;
    public PrefabContainer PrefabContainer => _prefabContainer;
    public void PlayHitVFX(HittableMaterial material, Vector3 position, Quaternion rotation, Transform parent = null)
    {
        GameObject vfx = null;
        switch (material)
        {
            case HittableMaterial.FleshBloodRed:
                if (_prefabContainer.TryGetByName("FleshHitVFX", out vfx) == false)
                    Debug.LogError("FleshHitVFX not found in PrefabContainer");
                break;
            case HittableMaterial.Metal:
                if (_prefabContainer.TryGetByName("MetalHitVFX", out vfx) == false)
                    Debug.LogError("MetalHitVFX not found in PrefabContainer");
                break;
            default:
                Debug.LogError("Unknown material");
                break;
        }
        PlayVFX(vfx, position, rotation, parent);
    }

    public void PlayVFX(GameObject vfx, Vector3 position, Quaternion rotation, Transform parent = null)
    {
        if (vfx == null) return;
        var vfxInstance = Instantiate(vfx, position, rotation);
        if (parent != null)
        {
            vfxInstance.transform.SetParent(parent);
        }
        StartCoroutine(DestroyVFX(vfxInstance));
    }
    
    private IEnumerator DestroyVFX(GameObject vfxInstance)
    {
        var timer = vfxInstance.GetComponent<ParticleSystem>().main.duration;
        yield return new WaitForSeconds(timer);
        Destroy(vfxInstance);
    }
}
