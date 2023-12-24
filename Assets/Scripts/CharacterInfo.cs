using System;

public class CharacterInfo
{
    public int Side;
    
    public bool IsAttacking;
    public bool IsDashing;
    public bool IsRunning;
    public bool IsDead;

    public float LogicalSpeed;
    public float RealSpeed;
    
    public float DashSpeed;
    public float DashDuration;
    public float DashCooldown;

    public float CurrentAttackCooldown;
    public float CurrentDashCooldown;
    
    public bool CanCombo { get; protected set; }
    
    public Weapon EquippedWeapon;
    
    public Action OnDeath;
    public Action<float> OnTakeDamage;

    protected CharacterInfo(int side, float logicalSpeed, float dashSpeed = 0, float dashDuration = 0, float dashCooldown = 0, bool canCombo = false)
    {
        CanCombo = canCombo;
        IsDead = false;
        Side = side;
        LogicalSpeed = logicalSpeed;
        RealSpeed = logicalSpeed;
        DashSpeed = dashSpeed;
        DashDuration = dashDuration;
        DashCooldown = dashCooldown;
        OnDeath += () => IsDead = true;
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
        OnTakeDamage?.Invoke(amount);
    }

    public virtual void HealthUpdate()
    {
        
    }

    public void Die()
    {
        OnDeath?.Invoke();
    }
}
