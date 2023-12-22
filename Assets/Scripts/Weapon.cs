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
    
    [SerializeField] private List<string> attackAnimations;
    public List<string> AttackAnimation { get => attackAnimations; protected set => attackAnimations = value; }

    public bool useCombos;
    
    [SerializeField] private int maxComboNumber;
    public int MaxComboNumber { get => maxComboNumber; protected set => maxComboNumber = value; }
    
    public int ComboNumber { get; protected set; }
    
    private Collider _weaponCollider;
    
    private List<Collider> _hitColliders;
    
    public int Side { get; set; }

    private void Awake()
    {
        _weaponCollider = GetComponent<Collider>();
        _hitColliders = new List<Collider>();
        var localScale = _weaponCollider.transform.localScale;
        _weaponCollider.transform.localScale = new Vector3(localScale.x, attackRange, localScale.z);;
        ComboNumber = 1;
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
    
    public void IterateCombo()
    {
        ComboNumber++;
        if (ComboNumber > MaxComboNumber)
        {
            ComboNumber = 1;
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        // Movable object
        if (other.gameObject.CompareTag($"MovableSceneMisc"))
        {
            if (_hitColliders.Contains(other)) return;
            _hitColliders.Add(other);
            if (other.TryGetComponent(out Rigidbody rigidBody))
            {
                var direction = other.transform.position - transform.position;
                rigidBody.AddForce(direction * rigidBody.mass * 1000);
            }
            return;
        }
        
        // Character
        if (other.TryGetComponent(out CharacterManager characterManager))
        {
            if (characterManager.CharacterInfo.IsDead) return;
            if (characterManager.CharacterInfo.Side == Side) return;
            if (_hitColliders.Contains(other)) return;
            _hitColliders.Add(other);
            characterManager.CharacterInfo.TakeDamage(AttackDamage);
            if (other.CompareTag("Player")) return;
            if (other.TryGetComponent(out Rigidbody rigidBody))
            {
                var direction = other.transform.position - transform.position;
                rigidBody.AddForce(direction * rigidBody.mass * 200);
            }
        }
    }
}
