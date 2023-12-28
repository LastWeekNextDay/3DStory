using UnityEngine;

public class SkeletonCharacterManager : CharacterManager
{
    [Header("Character Info")]
    public SkeletonCharacterInfo skeletonCharacterInfo;
    protected override void Awake()
    {
        CharacterInfo = skeletonCharacterInfo;
        base.Awake();
    }
}
