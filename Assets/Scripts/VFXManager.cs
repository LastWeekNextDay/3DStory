using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
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
