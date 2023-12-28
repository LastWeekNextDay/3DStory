using System;
using UnityEngine;

public class PlayerCharacterManager : CharacterManager
{
    [Header("Character Info")]
    public PlayerCharacterInfo playerCharacterInfo;
    protected override void Awake()
    {
        playerCharacterInfo.OnTimeHitReset += () => UpdateModelDamageColor(0);
        playerCharacterInfo.OnDeath += () => UpdateModelDamageColor(0);
        playerCharacterInfo.OnTakeDamage += (_,_) => UpdateModelDamageColor();
        CharacterInfo = playerCharacterInfo;
        base.Awake();
    }

    private void UpdateModelDamageColor(int damageLevel = -1)
    {
        if (damageLevel == -1)
        {
            damageLevel = playerCharacterInfo.TimesHit;
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
