using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public enum AttackType
{
    Melee,
    Ranged
}

public class Weapon : MonoBehaviour
{
    [SerializeField] private AttackType attackType;
    public AttackType AttackType { get => attackType; protected set => attackType = value; }
    
    [SerializeField] private float attackRange;
    public float AttackRange { get => attackRange; protected set => attackRange = value; }
    
    [SerializeField] private float attackDamage;
    public float AttackDamage { get => attackDamage; protected set => attackDamage = value; }
    
    [SerializeField] private float attackSpeed;
    public float AttackSpeed { get => attackSpeed; protected set => attackSpeed = value; }
    
    [SerializeField] private float attackCooldown;
    public float AttackCooldown { get => attackCooldown; protected set => attackCooldown = value; }
    
    [SerializeField] private string attackAnimation;
    public string AttackAnimation { get => attackAnimation; protected set => attackAnimation = value; }
    
    private Collider _weaponCollider;
    
    private List<Collider> _hitColliders = new List<Collider>();

    private void Awake()
    {
        _weaponCollider = GetComponent<Collider>();
        
        var localScale = _weaponCollider.transform.localScale;
        _weaponCollider.transform.localScale = new Vector3(localScale.x, attackRange, localScale.z);;
    }
    
    public void AllowDamageCollision()
    {
        _weaponCollider.enabled = true;
    }
    
    public void DisallowDamageCollision()
    {
        _hitColliders.Clear();
        _weaponCollider.enabled = false;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag($"Enemy"))
        {
            if (_hitColliders.Contains(other)) return;
            _hitColliders.Add(other);
            if (other.TryGetComponent(out Rigidbody rigidBody))
            {
                Debug.Log("Hit");
                Vector3 direction = other.transform.position - transform.position;
                rigidBody.AddForce(direction * rigidBody.mass * 1000);
            }
        }
    }
    
}
