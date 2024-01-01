using UnityEngine;

public class SkeletonCharacterManager : CharacterManager
{
    [Header("Specialized Character Info")]
    [SerializeField] private SkeletonSpecializedCharacterInfo skeletonSpecializedCharacterInfo;
    public SkeletonSpecializedCharacterInfo SkeletonSpecializedCharacterInfo => skeletonSpecializedCharacterInfo;
    protected override void Awake()
    {
        base.Awake();
        CharacterInfo.OnTakeDamage += (_,_,_) => AnimationController.DoHurtAnimation();
        CharacterInfo.OnTakeDamage += (damage,_,_) => SkeletonSpecializedCharacterInfo.TakeDamage(damage);
        SkeletonSpecializedCharacterInfo.OnDeath += CharacterInfo.Die;
    }
}
