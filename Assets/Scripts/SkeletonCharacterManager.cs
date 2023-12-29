using UnityEngine;

public class SkeletonCharacterManager : CharacterManager
{
    [Header("Character Info")]
    public SkeletonCharacterInfo skeletonCharacterInfo;
    protected override void Awake()
    {
        CharacterInfo = skeletonCharacterInfo;
        base.Awake();
        CharacterInfo.OnTakeDamage += (_,_,_) => AnimationController.DoHurtAnimation();
    }
}
