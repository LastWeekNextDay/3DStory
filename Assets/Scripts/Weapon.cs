using System;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField] private float attackTime;
    public float AttackTime { get => attackTime; protected set => attackTime = value; }
    [SerializeField] private float attackCooldown;
    public float AttackCooldown { get => attackCooldown; protected set => attackCooldown = value; }
    [SerializeField] private List<AnimationState> attackAnimations;
    public List<AnimationState> AttackAnimations { get => attackAnimations; protected set => attackAnimations = value; }
    [SerializeField] private int maxComboNumber; // First animations up to this number will be used in combos
    public int MaxComboNumber { get => maxComboNumber; protected set => maxComboNumber = value; }
    public int CurrentComboNumber { get; protected set; }
    private Collider _weaponCollider;
    private List<Collider> _hitColliders;
    public int Side { get; set; }
    public AudioSource AudioSource { get; private set; }
    [SerializeField] private AudioClip weaponInitialAttackSound;
    public AudioClip WeaponInitialAttackSound => weaponInitialAttackSound;
    [SerializeField] private AudioClip weaponHitSound;
    public AudioClip WeaponHitSound => weaponHitSound;
    public int weaponPushStrength;

    public Action OnHitHardObstacle;
    
    private void Awake()
    {
        _weaponCollider = GetComponent<Collider>();
        _hitColliders = new List<Collider>();
        AudioSource = GetComponent<AudioSource>();
        CurrentComboNumber = 1;
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
        CurrentComboNumber++;
        if (CurrentComboNumber > MaxComboNumber)
        {
            CurrentComboNumber = 1;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_hitColliders.Contains(other)) return;

        var validHit = false;
        var hitHardObstacle = false;
        Vector3 closestPointWeapon;
        Vector3 closestPointOther;

        switch (other)
        {
            case var _ when other.TryGetComponent(out MovableMiscManager movableMiscManager):
                _hitColliders.Add(other);
                closestPointWeapon = _weaponCollider.ClosestPoint(other.transform.position);
                closestPointOther = other.ClosestPoint(closestPointWeapon);
                movableMiscManager.MovableMiscInfo.LastHitPosition = closestPointOther;
                movableMiscManager.MovableMiscInfo.LastHitDirection = closestPointWeapon - (closestPointWeapon - movableMiscManager.MovableMiscInfo.LastHitPosition).normalized;
                movableMiscManager.MovableMiscInfo.TakeDamage(AttackDamage);
                hitHardObstacle = true;
                validHit = true;
                break;

            case var _ when other.TryGetComponent(out CharacterManager characterManager):
                if (characterManager.CharacterInfo.IsDead) break;
                if (characterManager.CharacterInfo.side == Side) break;
                _hitColliders.Add(other);
                closestPointWeapon = _weaponCollider.ClosestPoint(other.transform.position);
                closestPointOther = other.ClosestPoint(closestPointWeapon);
                characterManager.CharacterInfo.LastHitPosition = closestPointOther;
                characterManager.CharacterInfo.LastHitDirection = closestPointWeapon - (closestPointWeapon - closestPointOther).normalized;
                characterManager.CharacterInfo.TakeDamage(AttackDamage);
                validHit = true;
                break;
            
            default:
                return;
        }
        
        if (validHit)
        {
            AudioSource.PlayOneShot(weaponHitSound);
            if (hitHardObstacle)
            {
                OnHitHardObstacle.Invoke();
                return;
            }
            if (other.TryGetComponent(out Rigidbody rigidBodyCharacter))
            {
                var direction = other.transform.position - transform.position;
                rigidBodyCharacter.AddForce(direction * weaponPushStrength);
            }
        }
    }
}
