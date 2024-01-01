using UnityEngine;

public class SkeletonCharacterManager : CharacterManager
{
    [Header("Specialized Character Info")]
    [SerializeField] private SkeletonSpecializedCharacterInfo skeletonSpecializedCharacterInfo;
    public SkeletonSpecializedCharacterInfo SkeletonSpecializedCharacterInfo => skeletonSpecializedCharacterInfo;
    protected override void Awake()
    {
        base.Awake();
        CharacterInfo.OnTakeDamage += (damage,_,_) => SkeletonSpecializedCharacterInfo.TakeDamage(damage);
        CharacterInfo.OnTakeDamage += (_,_,_) => { if (CharacterInfo.IsDead == false) AnimationController.DoHurtAnimation(); };
        SkeletonSpecializedCharacterInfo.OnDeath += CharacterInfo.Die;
    }
}
