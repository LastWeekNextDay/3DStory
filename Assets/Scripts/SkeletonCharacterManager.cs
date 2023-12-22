using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonCharacterManager : CharacterManager
{
    private new void Awake()
    {
        CharacterInfo = new SkeletonCharacterInfo();
        base.Awake();
        CharacterInfo.EquippedWeapon.useCombos = false;
    }
}
