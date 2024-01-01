using System;
using UnityEngine;

[Serializable]
public class PlayerSpecializedCharacterInfo : ISpecializedCharInfo
{
    [NonSerialized] public int TimesHit = 0;
    
    private float _timeForTimesHitReset;

    public int hitsTillDeath;
    public int timeTillHitReset;

    public Action OnTimeHitReset;
    public Action OnDeath;

    public void HealthUpdate()
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
    
    public void TakeDamage(float amount)
    {
        TimesHit++;
        if (TimesHit >= hitsTillDeath)
        {
            OnDeath?.Invoke();
        }
    }
}
