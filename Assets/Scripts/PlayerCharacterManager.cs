using System;
using UnityEngine;

public class PlayerCharacterManager : CharacterManager
{
    [Header("Specialized Character Info")]
    [SerializeField] private PlayerSpecializedCharacterInfo playerSpecializedCharacterInfo;
    public PlayerSpecializedCharacterInfo PlayerSpecializedCharacterInfo => playerSpecializedCharacterInfo;
    protected override void Awake()
    {
        base.Awake();
        CharacterInfo.OnTakeDamage += (damage,_,_) => PlayerSpecializedCharacterInfo.TakeDamage(damage);
        CharacterInfo.OnTakeDamage += (_,_,_) => UpdateModelDamageColor();
        PlayerSpecializedCharacterInfo.OnTimeHitReset += () => UpdateModelDamageColor();
        PlayerSpecializedCharacterInfo.OnDeath += () => UpdateModelDamageColor(0);
        PlayerSpecializedCharacterInfo.OnDeath += CharacterInfo.Die;
    }

    protected override void Update()
    {
        base.Update();
        PlayerSpecializedCharacterInfo.HealthUpdate();
    }

    private void UpdateModelDamageColor(int damageLevel = -1)
    {
        if (damageLevel == -1)
        {
            damageLevel = PlayerSpecializedCharacterInfo.TimesHit;
        }
        var color = damageLevel switch
        {
            0 => new Color(1, 1, 1, 1),
            1 => new Color(0.33f, 0, 0, 1),
            2 => new Color(0.66f, 0, 0, 1),
            3 => new Color(1, 0, 0, 1),
            _ => throw new ArgumentOutOfRangeException(nameof(damageLevel), damageLevel, "Damage level invalid.")
        };
        foreach (var mRenderer in GetComponentsInChildren<Renderer>())
        {
            mRenderer.material.color = color;
        }
    }
}
