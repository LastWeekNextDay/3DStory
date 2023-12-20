using System;

public class CharacterInfo
{
    public bool IsAttacking;
    public bool IsDashing;
    public bool IsRunning;

    public float DefaultSpeed;
    public float CurrentSpeed;
    
    public float DashSpeed;
    public float DashDuration;
    public float DashCooldown;

    public float CurrentAttackCooldown;
    public float CurrentDashCooldown;
    
    public Weapon EquippedWeapon;

    protected CharacterInfo(float defaultSpeed, float dashSpeed = 0, float dashDuration = 0, float dashCooldown = 0)
    {
        DefaultSpeed = defaultSpeed;
        CurrentSpeed = defaultSpeed;
        DashSpeed = dashSpeed;
        DashDuration = dashDuration;
        DashCooldown = dashCooldown;
    }
    
    public void LowerAttackCooldown(float amount)
    {
        CurrentAttackCooldown -= amount;
    }
    
    public void LowerDashCooldown(float amount)
    {
        CurrentDashCooldown -= amount;
    }
}
