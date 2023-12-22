using System;
using UnityEngine;

public class PlayerCharacterManager : CharacterManager
{
    private new void Awake()
    {
        CharacterInfo = new PlayerCharacterInfo(dashDuration: 0.1f);
        CharacterInfo.OnTakeDamage += amount => UpdateModelColor();
        if (CharacterInfo is PlayerCharacterInfo playerCharacterInfo)
        {
            playerCharacterInfo.OnTimeHitReset += UpdateModelColor;
        }
        base.Awake();
    }

    private void UpdateModelColor()
    {
        if (CharacterInfo is PlayerCharacterInfo playerCharacterInfo)
        {
            var color = playerCharacterInfo.TimesHit switch
            {
                0 => Color.white,
                1 => Color.red * 0.25f,
                2 => Color.red * 0.5f,
                3 => Color.red,
                _ => throw new ArgumentOutOfRangeException()
            };
            foreach (var mRenderer in GetComponentsInChildren<Renderer>())
            {
                mRenderer.material.color = color;
            }
        }
    }
}
