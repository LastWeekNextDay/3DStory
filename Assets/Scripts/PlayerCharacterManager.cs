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
        CharacterInfo.OnTakeDamage += (_,_,_) => UpdateModelDamageColor();
        CharacterInfo.OnTakeDamage += (damage,_,_) => PlayerSpecializedCharacterInfo.TakeDamage(damage);
        PlayerSpecializedCharacterInfo.OnTimeHitReset += () => UpdateModelDamageColor(0);
        PlayerSpecializedCharacterInfo.OnDeath += () => UpdateModelDamageColor(0);
        PlayerSpecializedCharacterInfo.OnDeath += CharacterInfo.Die;
    }

    private void UpdateModelDamageColor(int damageLevel = -1)
    {
        if (damageLevel == -1)
        {
            damageLevel = playerSpecializedCharacterInfo.TimesHit;
        }
        var color = damageLevel switch
        {
            0 => Color.white,
            1 => Color.red * 0.25f,
            2 => Color.red * 0.5f,
            3 => Color.red,
            _ => throw new ArgumentOutOfRangeException(nameof(damageLevel), damageLevel, "Damage level invalid.")
        };
        foreach (var mRenderer in GetComponentsInChildren<Renderer>())
        {
            mRenderer.material.color = color;
        }
    }
}
