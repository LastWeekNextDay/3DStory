using System;
using UnityEngine;

public enum HittableMaterial
{
    FleshBloodRed,
    Metal
}

[Serializable]
public class HittableObject
{
    public HittableMaterial Material;
    [NonSerialized] public Vector3 LastHitDirection;
    [NonSerialized] public Vector3 LastHitPosition;
    public Action<float, Vector3, Vector3> OnTakeDamage; // amount, direction, position
    public virtual void TakeDamage(float amount)
    {
        OnTakeDamage?.Invoke(amount, LastHitDirection, LastHitPosition);
    }
}
