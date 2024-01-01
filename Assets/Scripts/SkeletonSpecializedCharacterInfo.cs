using System;
using UnityEngine;

[Serializable]
public class SkeletonSpecializedCharacterInfo : ISpecializedCharInfo
{
    [SerializeField] private int currentHealth;
    public int CurrentHealth { get => currentHealth; private set => currentHealth = value; }
    public Action OnDeath;
    
    public SkeletonSpecializedCharacterInfo(int health = 100)
    {
        CurrentHealth = health;
    }
    
    public void TakeDamage(float amount)
    {
        CurrentHealth -= (int) amount;
        if (CurrentHealth <= 0)
        {
            OnDeath?.Invoke();
        }
    }
}
