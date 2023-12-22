using System;

public class CharacterInfo
{
    public int Side;
    
    public bool IsAttacking;
    public bool IsDashing;
    public bool IsRunning;
    public bool IsDead;

    public float DefaultSpeed;
    public float CurrentSpeed;
    
    public float DashSpeed;
    public float DashDuration;
    public float DashCooldown;

    public float CurrentAttackCooldown;
    public float CurrentDashCooldown;
    
    public Weapon EquippedWeapon;
    
    public Action OnDeath;
    public Action<float> OnTakeDamage;

    protected CharacterInfo(int side, float defaultSpeed, float dashSpeed = 0, float dashDuration = 0, float dashCooldown = 0)
    {
        IsDead = false;
        Side = side;
        DefaultSpeed = defaultSpeed;
        CurrentSpeed = defaultSpeed;
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
