using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonCharacterManager : CharacterManager
{
    private new void Awake()
    {
        CharacterInfo = new SkeletonCharacterInfo();
        CharacterInfo.OnDeath += () =>
        {
            CharacterInfo.IsDead = true;
            Controller.ExitMethod();
            AnimationController.DoDeathAnimation();
            StartCoroutine(nameof(HideBodyByScaling));
        };
        base.Awake();
    }
}
