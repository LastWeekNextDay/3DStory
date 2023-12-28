using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class SkeletonCharacterInfo : CharacterInfo
{
    [SerializeField] private int currentHealth;
    public int CurrentHealth { get => currentHealth; private set => currentHealth = value; }
    
    public SkeletonCharacterInfo(int health = 100, float logicalSpeed = 1) 
        : base(1, logicalSpeed)
    {
        CurrentHealth = health;
    }
    
    public SkeletonCharacterInfo(CharacterInfo characterInfo, int health = 100, float logicalSpeed = 1)
        : base(characterInfo.side, characterInfo.logicalSpeed, characterInfo.dashSpeed, characterInfo.dashDuration, characterInfo.dashCooldown, characterInfo.CanCombo)
    {
        CurrentHealth = 100;
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
