using System;
using UnityEngine;

[Serializable]
public class CharacterInfo
{
    public int side;
    
    [NonSerialized] public bool IsAttacking;
    [NonSerialized] public bool IsDashing;
    [NonSerialized] public bool IsRunning;
    [NonSerialized] public bool IsDead;

    public float logicalSpeed;
    [NonSerialized] public float RealSpeed;
    
    public float dashSpeed;
    public float dashDuration;
    public float dashCooldown;

    [NonSerialized] public float CurrentAttackCooldown;
    [NonSerialized] public float CurrentDashCooldown;
    
    [SerializeField] private bool canCombo;
    public bool CanCombo { get => canCombo; protected set => canCombo = value; }
    
    [NonSerialized] public Weapon EquippedWeapon;
    [NonSerialized] public Vector3 LastHitDirection;
    
    public Action OnDeath;
    public Action<float, Vector3> OnTakeDamage;

    protected CharacterInfo(int side, float logicalSpeed, float dashSpeed = 0, float dashDuration = 0, float dashCooldown = 0, bool canCombo = false)
    {
        CanCombo = canCombo;
        IsDead = false;
        this.side = side;
        this.logicalSpeed = logicalSpeed;
        RealSpeed = logicalSpeed;
        this.dashSpeed = dashSpeed;
        this.dashDuration = dashDuration;
        this.dashCooldown = dashCooldown;
    }
    
    public void LowerAttackCooldown(float amount)
    {
        CurrentAttackCooldown -= amount;
    }
    
    public void LowerDashCooldown(float amount)
    {
        CurrentDashCooldown -= amount;
    }

    public virtual void TakeDamage(float amount)
    {
        OnTakeDamage?.Invoke(amount, LastHitDirection);
    }

    public virtual void HealthUpdate()
    {
        
    }

    public void Die()
    {
        OnDeath?.Invoke();
    }
}
