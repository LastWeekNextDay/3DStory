using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonCharacterInfo : CharacterInfo
{
    public int CurrentHealth { get; private set; }
    
    public SkeletonCharacterInfo(int health = 100, float logicalSpeed = 1) 
        : base(1, logicalSpeed)
    {
        CurrentHealth = health;
    }
    
    public override void TakeDamage(float amount)
    {
        base.TakeDamage(amount);
        CurrentHealth -= (int) amount;
        if (CurrentHealth <= 0)
        {
            Die();
        }
    }
}
