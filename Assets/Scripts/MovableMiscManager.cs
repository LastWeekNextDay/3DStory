using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableMiscManager : MonoBehaviour
{
    [SerializeField] private MovableMiscInfo _movableMiscInfo;
    public MovableMiscInfo MovableMiscInfo => _movableMiscInfo;
    [Header("Object Essentials")]
    [SerializeField] private Rigidbody rigidBody;
    public Rigidbody RigidBody => rigidBody;
    [SerializeField] private Collider collisionCollider;
    public Collider Collider => collisionCollider;

    private void Awake()
    {
        MovableMiscInfo.OnTakeDamage += (_, vector3_dir, vector3_pos) => GameObject.FindGameObjectsWithTag("GameController")[0].
        GetComponent<VFXManager>().PlayHitVFX(MovableMiscInfo.Material, vector3_pos, Quaternion.LookRotation(vector3_dir), transform); 
    }
}
