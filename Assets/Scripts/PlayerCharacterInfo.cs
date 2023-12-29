using System;
using UnityEngine;

[Serializable]
public class PlayerCharacterInfo : CharacterInfo
{
    [NonSerialized] public int TimesHit;
    
    private float _timeForTimesHitReset = 3f;

    public int hitsTillDeath;
    public int timeTillHitReset;

    public Action OnTimeHitReset;
    
    public PlayerCharacterInfo(float logicalSpeed = 5, float dashSpeed = 50, float dashDuration = 0.15f,
        float dashCooldown = 1.5f) 
        : base(0, logicalSpeed, HittableMaterial.FleshBloodRed, dashSpeed, dashDuration, dashCooldown, canCombo: true)
    {
    }

    public override void HealthUpdate()
    {
        if (TimesHit <= 0) return;
        _timeForTimesHitReset -= Time.deltaTime;
        if (_timeForTimesHitReset <= 0)
        {
            TimesHit = 0;
            _timeForTimesHitReset = timeTillHitReset;
            OnTimeHitReset?.Invoke();
        }
    }
    
    public override void TakeDamage(float amount)
    {
        TimesHit++;
        base.TakeDamage(amount);
        if (TimesHit >= hitsTillDeath)
        {
            Die();
        }
    }
}
