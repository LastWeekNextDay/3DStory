using System;
using UnityEngine;

public class PlayerCharacterInfo : CharacterInfo
{
    public int TimesHit;
    
    private float _timeForTimesHitReset = 3f;

    public Action OnTimeHitReset;
    
    public PlayerCharacterInfo(float logicalSpeed = 5, float dashSpeed = 50, float dashDuration = 0.15f,
        float dashCooldown = 1.5f) 
        : base(0, logicalSpeed, dashSpeed, dashDuration, dashCooldown, canCombo: true)
    {
    }

    public override void HealthUpdate()
    {
        if (TimesHit <= 0) return;
        _timeForTimesHitReset -= Time.deltaTime;
        if (_timeForTimesHitReset <= 0)
        {
            TimesHit = 0;
            _timeForTimesHitReset = 3f;
            OnTimeHitReset?.Invoke();
        }
    }
    
    public override void TakeDamage(float amount)
    {
        TimesHit++;
        base.TakeDamage(amount);
        if (TimesHit >= 3)
        {
            Die();
        }
    }
}
